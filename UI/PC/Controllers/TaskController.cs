using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using FFLTask.GLB.Global.Enum;
using FFLTask.GLB.Global.UrlParameter;
using FFLTask.SRV.ServiceInterface;
using FFLTask.SRV.ViewModel.Shared;
using FFLTask.SRV.ViewModel.Task;
using FFLTask.UI.PC.Filter;
using Global.Core.ExtensionMethod;
using Global.Core.Helper;
using Newtonsoft.Json;
using Task = FFLTask.SRV.ViewModel.Task;

namespace FFLTask.UI.PC.Controllers
{
    [NeedAuthorized]
    public class TaskController : BaseController
    {
        #region Constructor

        private ITaskService _taskService;
        private IProjectConfigService _projectConfigService;
        private IUserService _userService;
        private IProjectService _projectService;
        private IAuthroizationService _authService;
        public TaskController(ITaskService taskService,
            IProjectConfigService projectConfigService,
            IUserService userService,
            IProjectService projectService,
            IAuthroizationService authService)
        {
            _taskService = taskService;
            _projectConfigService = projectConfigService;
            _userService = userService;
            _projectService = projectService;
            _authService = authService;
        }

        #endregion

        #region Common Actions

        #region URL: /Task/List

        public ActionResult List(int? projectId, bool setShownColoumns = false)
        {
            if (setShownColoumns)
            {
                Request.Cookies[CookieKey.ShowColumn].Value = null;
            }

            ListModel model = new ListModel { SetShownColoumns = setShownColoumns };
            model.TimeSpan = new _TimeSpanModel();
            projectId = getProjectInList(model, projectId);

            fillListModel(model, projectId.Value, TaskList.Sort_By_Created);

            return View(model);
        }

        [HttpPost]
        public ActionResult List(ListModel model, bool setShownColoumns = false)
        {
            if (setShownColoumns)
            {
                var showColumns = model.ShowColumns.Where(s => s.Selected).ToList();
                setShownColumnsIntoCookie(showColumns);

                return RedirectToAction("List");
            }

            int projectId = getProjectInList(model);
            ViewBag.SortType = getSortTypeInList();
            ViewBag.Direction = getDirectionInList();

            fillListModel(model, projectId, ViewBag.SortType, ViewBag.Direction);

            if (!ModelState.IsValid)
            {
                //TODO: useless since this is invoked
                //ModelState.Clear();
                return View(model);
            }

            //TODO: just a workaround
            ModelState.Clear();
            return View(model);
        }

        #endregion

        #region URL: /Task/New

        public ActionResult New(int? projectId, int? parentId)
        {
            NewModel model = new NewModel();
            fillNewModel(model, projectId, parentId);

            return View(model);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult New(NewModel model, IList<HttpPostedFileBase> file)
        {           
            if (model.IdOrName == SearchBy.Name)
            {
                string strSelectedParentId = Request.Form["selectedParentTaskId"];
                if (!string.IsNullOrEmpty(strSelectedParentId))
                {
                    model.TaskItem.Parent.Id =
                        Convert.ToInt32(Request.Form["selectedParentTaskId"]);                    
                }
                //TODO: should use a specific viewmodel for parent task search
                ModelState.Remove("TaskItem.Parent.Id");
            }

            int projectId = model.CurrentProject.TailSelectedProject.Id;
            int? parentId = model.TaskItem.Parent.Id;

            ModelState.Remove("TaskItem.Parent.Title");
            if (!ModelState.IsValid)
            {
                fillNewModel(model, projectId, parentId);
                return View(model);
            }
            if (model.TaskItem.LiteItem.Virtual && model.TaskItem.ExpectedWorkPeriod.HasValue)
            {
                fillNewModel(model, projectId, parentId);
                ModelState.AddModelError("TaskItem.LiteItem.Virtual", "* 虚任务不能包含工时 ");
                return View(model);
            }

            if (getPrivilegeInProject(projectId) != PrivilegeInProject.HasPublish)
            {
                //TODO;
                //throw new NotImplementedException();
            }

            if (parentId.HasValue &&
                string.IsNullOrEmpty(_taskService.GetTitle(parentId.Value)))
            {
                fillNewModel(model, projectId, parentId);
                ViewBag.ShowNotFound = true;
                return View(model);
            }

            int taskId = _taskService.Create(model, userHelper.CurrentUserId.Value);
            upload(file, taskId);

            HttpCookie cookie = new HttpCookie(CookieKey.PrevProjectInTaskNew, projectId.ToString());
            Response.Cookies.Add(cookie);

            return redirectAfterNew(model, taskId);
        }

        #endregion

        #region URL: /Task/Edit

        public ActionResult Edit(int taskId = 1)
        {
            EditModel model = fillEditModel(taskId);
            return View(model);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Edit(int taskId, EditModel model, IList<HttpPostedFileBase> file)
        {
            if (model.IdOrName == SearchBy.Name)
            {
                string strSelectedParentId = Request.Form["selectedParentTaskId"];
                if (!string.IsNullOrEmpty(strSelectedParentId))
                {
                    model.TaskItem.Parent.Id =
                        Convert.ToInt32(Request.Form["selectedParentTaskId"]);
                }
                //TODO: should use a specific viewmodel for parent task search
                ModelState.Remove("TaskItem.Parent.Id");
            }

            bool autoCompleteParent = model.AutoCompleteParent;

            ModelState.Remove("TaskItem.Parent.Title");
            //when only comment the taskItem should be null
            model.TaskItem = model.TaskItem ?? new FullItemModel();
            model.TaskItem.LiteItem = model.TaskItem.LiteItem ?? new LiteItemModel();
            model.TaskItem.LiteItem.Id = taskId;

            #region deal with task is virtual

            if (model.TaskItem.LiteItem.Virtual && model.TaskItem.ExpectedWorkPeriod.HasValue)
            {
                model = fillEditModel(taskId);
                ModelState.AddModelError("TaskItem.LiteItem.Virtual", "* 虚任务不能包含工时 ");
                return View(model);
            }

            #endregion

            #region deal with the attachments

            upload(file, taskId);

            #endregion

            #region deal with own

            if (model.Own)
            {
                model.TaskItem.Owner = userHelper.CurrentUser;
                _taskService.Assign(model);
                return redirectAfterEdit(model);
            }

            #endregion

            IList<TaskProcess> processess = getProcesses();

            #region deal with task process

            if (processess.Contains(TaskProcess.Publish))
            {
                if (model.TaskItem.Parent.Id.HasValue)
                {
                    if (string.IsNullOrEmpty(_taskService.GetTitle(model.TaskItem.Parent.Id.Value)))
                    {
                        ViewBag.ShowNotFound = true;
                        model = fillEditModel(taskId);
                        return View(model);
                    }
                    bool parentIsOffspring = _taskService.ParentIsOffspring(taskId, model.TaskItem.Parent.Id.Value);
                    if (parentIsOffspring ||
                        taskId == model.TaskItem.Parent.Id.Value)
                    {
                        ViewBag.ParentIsOwnOffspring = true;
                        model = fillEditModel(taskId);
                        return View(model);
                    }
                }
                if (!ModelState.IsValid)
                {
                    model = fillEditModel(taskId);
                    return View(model);
                }
                if (getPrivilegeInProject(model.CurrentProject.TailSelectedProject.Id) != PrivilegeInProject.HasPublish)
                {
                    //throw new NotImplementedException();
                }
                _taskService.UpdateTaskProperty(model);
            }
            if (processess.Contains(TaskProcess.Assign))
            {
                _taskService.Assign(model);
            }
            if (processess.Contains(TaskProcess.Remove))
            {
                _taskService.Remove(taskId, model.Comment);
                return redirectAfterEdit(model);
            }
            if (processess.Contains(TaskProcess.Resume))
            {
                _taskService.Resume(taskId, model.Comment);
            }
            if (processess.Contains(TaskProcess.InProcess))
            {
                _taskService.Handle(model);
            }
            if (processess.Contains(TaskProcess.Accept))
            {
                if (model.TaskItem.HasAccepted)
                {
                    _taskService.Accept(model);
                }
                else
                {
                    if (string.IsNullOrWhiteSpace(model.Comment))
                    {
                        ModelState.AddModelError("Comment", "* 拒绝验收时必须说明原因");
                        model = fillEditModel(taskId);
                        return View(model);
                    }
                    _taskService.Refuse(model);
                }
            }

            #endregion

            #region deal with comments

            if (model.Comment.RealEmpty())
            {
                if (processess.IsNullOrEmpty())
                {
                    ModelState.Remove("TaskItem.Title");
                    ModelState.AddModelError("Comment", "* 没有其他改动时，留言不能为空");
                    model = fillEditModel(taskId);
                    return View(model);
                }
            }
            else
            {
                model.Comment = model.Comment.FixTags();
                model.CurrentUser = userHelper.CurrentUser;
                _taskService.Comment(model);
            }

            #endregion

            return redirectAfterEdit(model);
        }

        #endregion

        #region URL: /Task/Download

        public void Download(string path)
        {
            Response.AddHeader("Content-Disposition", "attachment;filename=" + Path.GetFileName(path));
            string filename = Server.MapPath(path);
            Response.TransmitFile(filename);
        }

        #endregion

        #region URL: /Task/Search

        public ActionResult Search(int id = 1)
        {
            Task.SearchModel model = new Task.SearchModel();
            //walkaround: _projectService.Get(userId) has been removed
            //model.Projects = _projectService.Get(userHelper.CurrentUserId.Value);
            model.Publisher = _userService.GetPublishers(id);
            model.Owners = _userService.GetOwners(id);
            model.AllPriotity = _projectConfigService.GetPriorities(id);
            return View(model);
        }

        [HttpPost]
        public ActionResult Search(Task.SearchModel model)
        {
            StringBuilder url = new StringBuilder();
            url.Append("/Task/List?");

            bool firstParameter = true;

            if (model.SelectedProjectId != null)
            {
                string join = firstParameter ? "?" : "&";

                url.Append(join);
                url.Append("project=");
                url.Append(model.SelectedProjectId);
            }

            if (model.TaskNumber != null)
            {
                string join = firstParameter ? "?" : "&";

                url.Append(join);
                url.Append("task=");
                url.Append(model.TaskNumber.Value);
            }
            if (model.SelectedPublishId != null)
            {
                string join = firstParameter ? "?" : "&";

                url.Append(join);
                url.Append("assigner=");
                url.Append(model.SelectedPublishId.Value);
            }
            if (model.SelecteOwnerId != null)
            {
                string join = firstParameter ? "?" : "&";

                url.Append(join);
                url.Append("owner=");
                url.Append(model.SelecteOwnerId.Value);
            }
            if (model.Priority != null)
            {
                string join = firstParameter ? "?" : "&";

                url.Append(join);
                url.Append("priority =");
                url.Append(model.Priority.Value);

                firstParameter = false;
            }
            if (model.GreaterOverDue != null)
            {
                string join = firstParameter ? "?" : "&";
                url.Append(join);
                url.Append("GreatOverDue=gt");
                url.Append(model.GreaterOverDue.Value);
            }
            if (model.LessOverDue != null)
            {
                string join = firstParameter ? "?" : "&";
                url.Append(join);
                url.Append("LessOverDue=lt");
                url.Append(model.LessOverDue.Value);
            }
            if (model.StartPublishDate != null)
            {
                string join = firstParameter ? "?" : "&";
                url.Append(join);
                url.Append("create_from=");
                url.Append(model.StartPublishDate.Value.ToChinese());
            }
            if (model.EndPublishDate != null)
            {
                string join = firstParameter ? "?" : "&";
                url.Append(join);
                url.Append("create_to=");
                url.Append(model.EndPublishDate.Value.ToChinese());
            }
            if (model.StartAssignerDate != null)
            {
                string join = firstParameter ? "?" : "&";
                url.Append(join);
                url.Append("begin_from=");
                url.Append(model.StartAssignerDate.Value.ToChinese());
            }
            if (model.EndAssignerDate != null)
            {
                string join = firstParameter ? "?" : "&";
                url.Append(join);
                url.Append("begin_to=");
                url.Append(model.EndAssignerDate.Value.ToChinese());
            }
            if (model.StartExpectCompleteDate != null)
            {
                string join = firstParameter ? "?" : "&";
                url.Append(join);
                url.Append("plan_complete_from=");
                url.Append(model.StartExpectCompleteDate.Value.ToChinese());
            }

            if (model.EndExpectCompleteDate != null)
            {
                string join = firstParameter ? "?" : "&";
                url.Append(join);
                url.Append("plan_complete_to=");
                url.Append(model.EndExpectCompleteDate.Value.ToChinese());
            }
            if (model.StartActaulCompleteDate != null)
            {
                string join = firstParameter ? "?" : "&";
                url.Append(join);
                url.Append("completed_from=");
                url.Append(model.StartExpectCompleteDate.Value.ToChinese());
            }
            if (model.EndActualCompleteDate != null)
            {
                string join = firstParameter ? "?" : "&";
                url.Append(join);
                url.Append("completed_to=");
                url.Append(model.StartExpectCompleteDate.Value.ToChinese());
            }
            if (model.StartLastUpdateTime != null)
            {
                string join = firstParameter ? "?" : "&";
                url.Append(join);
                url.Append("LastUpdateTime_from=");
                url.Append(model.StartLastUpdateTime.Value.ToChinese());
            }
            if (model.EndLastUpdateTime != null)
            {
                string join = firstParameter ? "?" : "&";
                url.Append(join);
                url.Append("LastUpdateTime_to=");
                url.Append(model.EndLastUpdateTime.Value.ToChinese());
            }
            return Redirect(url.ToString());



        }

        #endregion

        #region URL: /Task/History

        public ActionResult History(int taskId)
        {
            IList<TaskHistoryItemModel> models = _taskService.GetHistory(taskId);
            return View(models);
        }

        #endregion

        #region URL: /Task/Relation

        public ActionResult Relation(int taskId)
        {
            TaskRelationModel model = _taskService.GetRelation(taskId);

            return View(model);
        }

        #endregion

        #region URL: /Task/Sequence

        public ActionResult Sequence(int taskId)
        {
            SequenceModel model = _taskService.GetSequence(taskId);

            return View(model);
        }

        [HttpPost]
        public void Sequence(SequenceModel model)
        {
            _taskService.UpdateTaskSequence(model.SelectedChildrenSequences);
        }

        #endregion

        #endregion

        #region Ajax

        #region URL: /Task/GetOwners

        public JsonResult GetOwners(int projectId)
        {
            var owners = _userService.GetOwners(projectId);
            return Json(owners);
        }

        #endregion

        #region URL: /Task/GetAccepters

        public JsonResult GetAccepters(int projectId)
        {
            var owners = _userService.GetAccepters(projectId);
            return Json(owners);
        }

        #endregion

        #region URL: /Task/GetDifficulties

        public JsonResult GetDifficulties(int projectId)
        {
            var difficulties = _projectConfigService.GetDifficulties(projectId)
                .GetSelectListItems<TaskDifficulty>();

            return Json(difficulties);
        }

        #endregion

        #region URL: /Task/GetPriorities

        public JsonResult GetPriorities(int projectId)
        {
            var priorities = _projectConfigService.GetPriorities(projectId)
                .GetSelectListItems<TaskPriority>();

            return Json(priorities);
        }

        #endregion

        #region URL: /Task/GetTask

        public JsonResult GetTask(int taskId)
        {
            string title = _taskService.GetTitle(taskId);
            return Json(new { Title = title });
        }

        #endregion

        #region URL: /Task/AjaxOwn

        public ActionResult AjaxOwn(int taskId)
        {
            if (taskId == 0)
            {
                //TODO: 
                throw new Exception("");
            }
            else
            {
                _taskService.Own(taskId, userHelper.CurrentUserId.Value);
            }
            return Json(userHelper.CurrentUser.Name, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region URL: /Task/CanPublish

        public ActionResult CanPublish(int projectId)
        {
            return Json(getPrivilegeInProject(projectId));
        }

        #endregion

        #region URL: /Task/_Sum

        [HttpPost]
        public ActionResult _Sum(ListModel model)
        {
            //get selected many status 
            setStatusWithCookie(model);
            _SumModel result = _taskService.GetSum(model);
            return PartialView("~/Views/Task/List/_Sum.cshtml", result);
        }

        #endregion

        #region URL: /Task/_SearchResult

        public ActionResult _SearchResult(string taskName)
        {
            IList < LiteItemModel > model = _taskService.GetStartWith(taskName);
            return View(model);
        }

        #endregion

        #endregion

        #region Private Methods

        private PrivilegeInProject getPrivilegeInProject(int projectId)
        {
            if (!_projectService.HasJoined(projectId, userHelper.CurrentUserId.Value))
            {
                return PrivilegeInProject.NotJoin;
            }

            bool canPublish = _authService
                .GetTokens(userHelper.CurrentUserId.Value, projectId)
                .Contains(Token.Assinger);

            if (canPublish)
            {
                return PrivilegeInProject.HasPublish;
            }
            else
            {
                return PrivilegeInProject.NotPublish;
            }
        }

        private ActionResult redirectAfterNew(NewModel model, int taskId)
        {
            switch (model.Redirect)
            {
                case RedirectPage.Current:
                    return RedirectToAction("Edit", new { taskId = taskId });
                case RedirectPage.Brother:
                    return RedirectToAction("New", new { parentId = model.TaskItem.Parent.Id });
                case RedirectPage.Child:
                    return RedirectToAction("New", new { parentId = taskId });
                case RedirectPage.List:
                    return RedirectToAction("List", new { projectId = model.CurrentProject.TailSelectedProject.Id });
                case RedirectPage.Other:
                    return RedirectToAction("New");
                //TODO:
                default: throw new Exception();
            }
        }

        private ActionResult redirectAfterEdit(EditModel model)
        {
            switch (model.Redirect)
            {
                case RedirectPage.Current:
                    return RedirectToAction("Edit", new { taskId = model.TaskItem.LiteItem.Id });
                case RedirectPage.Previous:
                    return RedirectToAction("Edit", new { taskId = model.PreviousTaskId });
                case RedirectPage.Next:
                    return RedirectToAction("Edit", new { taskId = model.NextTaskId });
                case RedirectPage.Parent:
                    return RedirectToAction("Edit", new { taskId = model.TaskItem.Parent.Id });
                case RedirectPage.Close:
                    return Content("<script type=\"text/javascript\">window.open('','_self').close()</script>");
                //TODO:
                default: throw new Exception();
            }
        }

        private IList<TaskProcess> getProcesses()
        {
            IList<TaskProcess> processess = new List<TaskProcess>();

            string strProcess = Request.Form[typeof(TaskProcess).Name];

            if (string.IsNullOrEmpty(strProcess))
            {
                return processess;
            }

            string[] arrProcesses = strProcess.Split(',');
            for (int i = 0; i < arrProcesses.Length; i++)
            {
                TaskProcess process = (TaskProcess)Enum.Parse(typeof(TaskProcess), arrProcesses[i]);
                processess.Add(process);
            }

            return processess;
        }

        private EditModel fillEditModel(int taskId)
        {
            EditModel model = _taskService.GetEdit(taskId, userHelper.CurrentUser);
            //TODO: don't know whether it's the best way, 
            //but always set the default option as "Accept" for convenience when rendering the page
            model.TaskItem.HasAccepted = true;

            return model;
        }

        private bool getDirectionInList()
        {
            if (!string.IsNullOrEmpty(Request.Form["Direction"]))
            {
                return Convert.ToBoolean(Request.Form["Direction"]);
            }
            return false;
        }

        private string getSortTypeInList()
        {
            if (string.IsNullOrEmpty(Request.Form["SortType"]))
            {
                return TaskList.Sort_By_Created;
            }
            return Request.Form["SortType"];
        }

        private int? getProjectInList(ListModel model, int? projectId)
        {
            if (projectId == null)
            {
                model.CurrentProject = _projectService.GetDropdownlistLink(userHelper.CurrentUserId.Value);
                projectId = model.CurrentProject.TailSelectedProject.Id;
            }
            else
            {
                model.CurrentProject = _projectService.GetDropdownlistLink(userHelper.CurrentUserId.Value, projectId.Value);
            }

            return projectId;
        }

        private int getProjectInList(ListModel model)
        {
            int projectId = model.CurrentProject.TailSelectedProject.Id;
            model.CurrentProject = _projectService.GetDropdownlistLink(userHelper.CurrentUserId.Value, projectId);

            return projectId;
        }

        private void fillListModel(ListModel model, int projectId, string sort, bool des = true)
        {
            getShownColumnsFromCookie(model);
            setStatusWithCookie(model);
            model.CanOwn = _authService.GetTokens(userHelper.CurrentUserId.Value, projectId)
                .Contains(Token.Owner);

            model.AllPriorities = _projectConfigService.GetPriorities(projectId);
            model.AllDifficulties = _projectConfigService.GetDifficulties(projectId);
            model.AllQualities = _projectConfigService.GetQualities(projectId);
            model.AllStatus = _projectConfigService.GetStatus(projectId);

            model.Owners = _userService.GetAllOwners(projectId);
            model.Accepters = _userService.GetAllAccepters(projectId);
            model.Publishers = _userService.GetAllPublishers(projectId);

            model.NodeTypes = getNodeTypes();

            int sumOfItems = _taskService.GetCount(model);
            int pageSize = 20;
            int rowSize = 20;
            model.PageIndex = model.PageIndex ?? 1;

            //to avoid customer provide bigger page index than the actual have
            if ((model.PageIndex - 1) * pageSize >= sumOfItems)
            {
                model.PageIndex = sumOfItems / (pageSize + 1) + 1;
            }

            model.Pager = new PagerModel { SumOfItems = sumOfItems, PageIndex = model.PageIndex.Value, PageSize = pageSize, RowSize = rowSize };

            model.Items = _taskService.Get(sort, des, model);
        }

        private void fillNewModel(NewModel model, int? projectId, int? parentId)
        {
            model.TaskItem = new FullItemModel();

            if (parentId.HasValue)
            {
                //TODO: need check first? or return null from service?
                model.TaskItem.Parent = _taskService.GetLite(parentId.Value);
            }

            if (projectId == null)
            {
                if (Request.Cookies[CookieKey.PrevProjectInTaskNew] != null)
                {
                    projectId = int.Parse(Request.Cookies[CookieKey.PrevProjectInTaskNew].Value);
                }
                else
                {
                    model.CurrentProject = _projectService.GetDropdownlistLink(userHelper.CurrentUserId.Value);
                    projectId = model.CurrentProject.TailSelectedProject.Id;
                }
            }
            else
            {
                model.CurrentProject = _projectService.GetDropdownlistLink(userHelper.CurrentUserId.Value, projectId.Value);
            }

            int intProjectId = projectId.Value;
            model.CurrentProject = _projectService.GetDropdownlistLink(userHelper.CurrentUserId.Value, intProjectId);
            model.AllDifficulties = _projectConfigService.GetDifficulties(intProjectId);
            model.Owners = _userService.GetOwners(intProjectId);
            model.Accepters = _userService.GetAccepters(projectId.Value);
            model.AllPriorities = _projectConfigService.GetPriorities(intProjectId);
            //TODO: duplicated with model.CurrentProject?
            model.TaskItem.Project = new FFLTask.SRV.ViewModel.Project.FullItemModel { LiteItem = new FFLTask.SRV.ViewModel.Project.LiteItemModel { Id = intProjectId } };
        }

        private void setStatusWithCookie(ListModel model)
        {
            if (Request.Cookies[CookieKey.PreferStatus] != null)
            {
                string PreferStatus = Server.UrlDecode(Request.Cookies[CookieKey.PreferStatus].Value);
                string[] statuses = PreferStatus.Split(",".ToCharArray());

                //there will always be one empty value after splitting, so need statuses.Length - 1
                //e.g: [1, 23,] => [1], [23], [""]
                for (int i = 0; i < statuses.Length - 1; i++)
                {
                    model.SelectedStages = model.SelectedStages ?? new List<int>();
                    model.SelectedStages.Add(int.Parse(statuses[i]));
                }
            }
        }

        private void getShownColumnsFromCookie(ListModel model)
        {
            if (Request.Cookies[CookieKey.ShowColumn] != null &&
                !string.IsNullOrEmpty(Request.Cookies[CookieKey.ShowColumn].Value))
            {
                string strShowColumns = Request.Cookies[CookieKey.ShowColumn].Value;
                model.ShowColumns = JsonConvert.DeserializeObject<IList<SelectListItem>>(strShowColumns);
            }
            else
            {
                model.ShowColumns = typeof(ListColumn).GetSelectListItems().ToList();
                model.ShowColumns.ToList().ForEach(s => s.Selected = true);
            }
        }

        private void setShownColumnsIntoCookie(IList<SelectListItem> columns)
        {
            string strShowColumns = JsonConvert.SerializeObject(columns);

            HttpCookie cookie = new HttpCookie(CookieKey.ShowColumn, strShowColumns);
            cookie.Expires = DateTime.Now.AddYears(CookieSpan.AlmostForever);

            Response.Cookies.Add(cookie);
        }

        private Dictionary<string, NodeType> getNodeTypes()
        {
            Dictionary<string, NodeType> nodeTypes = new Dictionary<string, NodeType>();

            nodeTypes.Add(NodeType.Root.GetEnumDescription(), NodeType.Root);
            nodeTypes.Add(NodeType.Branch.GetEnumDescription(), NodeType.Branch);
            nodeTypes.Add(NodeType.Leaf.GetEnumDescription(), NodeType.Leaf);

            return nodeTypes;
        }

        private void upload(IList<HttpPostedFileBase> files, int taskId)
        {
            if (files != null) /* the check is only for IE */
            {
                files = files.Where(x => x != null).ToList();

                if (files.Count > 0)
                {
                    string directory = Server.MapPath(string.Format(@"/Resource/Image/Attachment/{0}", taskId));
                    if (!Directory.Exists(directory))
                    {
                        Directory.CreateDirectory(directory);
                    }
                    for (int i = 0; i < files.Count; i++)
                    {
                        string fileName = Path.GetFileName(files[i].FileName);
                        string fileFullName = string.Format("{0}\\{1}", directory, fileName);

                        Request.Files[i].SaveAs(fileFullName);
                    }
                }

                IList<string> fileNames = files.Select(x => x.FileName).ToList();
                _taskService.UploadFiles(fileNames, taskId, userHelper.CurrentUserId.Value);
            }
        }

        #endregion
    }
}
