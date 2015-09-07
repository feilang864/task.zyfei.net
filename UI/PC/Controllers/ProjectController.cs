using System;
using System.Collections.Generic;
using System.Web.Mvc;
using FFLTask.GLB.Global.Enum;
using FFLTask.SRV.ServiceInterface;
using FFLTask.SRV.ViewModel.Project;
using FFLTask.UI.PC.Filter;

namespace FFLTask.UI.PC.Controllers
{
    public class ProjectController : BaseController
    {
        private IProjectService _projectService;
        private IUserService _userService;
        private IAuthroizationService _authService;
        public ProjectController(IProjectService profectService,
            IUserService userService,
            IAuthroizationService authService)
        {
            _projectService = profectService;
            _userService = userService;
            _authService = authService;
        }

        #region Common Actions

        #region URL: /Project/Create

        [NeedAuthorized]
        public ActionResult Create()
        {
            CreateModel model = new CreateModel();
            model.Parents = 
                _authService.GetAdminedProjectIds(userHelper.CurrentUserId.Value);

            return View(model);
        }

        [HttpPost]
        [NeedAuthorized]
        public ActionResult Create(CreateModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            int projectId = getProjectId(model);

            if (model.Continue)
            {
                return RedirectToAction("Create");
            }
            else
            {
                return RedirectToAction("New", "Task", new { id = projectId });
            }

        }

        private int getProjectId(CreateModel model)
        {
            if (model.SelectedParent != null)
            {
                return _projectService.Create(model, model.SelectedParent.Value,
                   userHelper.CurrentUserId.Value);
            }
            else
            {
                return _projectService.Create(model, null, userHelper.CurrentUserId.Value);
            }
        }

        #endregion

        #region URL: /Project/Edit/{projectId}

        [NeedAuthorized]
        public ActionResult Edit(int projectId)
        {
            if (!_authService.GetTokens(userHelper.CurrentUserId.Value, projectId).Contains(Token.Founder))
            {
                ViewBag.IsFounder = false;
                return View();
            }

            EditModel model = _projectService.GetEdit(projectId);

            return View(model);
        }

        [HttpPost]
        [NeedAuthorized]
        public ActionResult Edit(EditModel model, int projectId, bool hasParent)
        {
            model.Id = projectId;
            if (!hasParent)
            {
                model.ParentId = null;
            }

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            if (!_authService.GetTokens(userHelper.CurrentUserId.Value, model.Id).Contains(Token.Founder))
            {
                ViewBag.IsFounder = false;
                return View();
            }

            _projectService.Modify(model);

            return RedirectToAction("Edit", new { projectId = projectId });
        }

        #endregion

        #region URL: /Project/Join

        [NeedAuthorized]
        public ActionResult Join(int projectId = 0)
        {
            if (projectId == 0)
            {
                return RedirectToAction("Search");
            }

            JoinModel model = _projectService.GetJoinTree(projectId, userHelper.CurrentUserId.Value);

            return View(model);
        }

        [HttpPost]
        [NeedAuthorized]
        public ActionResult Join(JoinModel model)
        {
            string[] items = Request.Form["Item.LiteItem.Id"].Split(',');
            string[] selected = Request.Form["Selected"].Split(',');

            //offset used here because asp.net mvc add hidden input for every checkbox,
            //and only checked will post back with true value
            //so selected'length = item's lenght + offset(selected number)
            int offset = 0;
            for (int i = 0; i < selected.Length; i++)
            {
                if (Convert.ToBoolean(selected[i]))
                {
                    int projectId = Convert.ToInt32(items[i - offset]);

                    if (!_projectService.HasJoined(projectId, userHelper.CurrentUserId.Value))
                    {
                        _projectService.Join(projectId, userHelper.CurrentUserId.Value);
                    }
                    else
                    {
                        //TODO: is it necessary?
                        throw new Exception("trying to join project twice with project id {0} and user id {1}");
                    }

                    offset++;
                }
            }

            return RedirectToAction("Join");
        }

        #endregion

        #region URL: /Project/{projectId}

        public ActionResult Summary(int projectId)
        {
            SummaryModel model = _projectService.GetSummary(projectId);
            return View(model);
        }

        #endregion

        #region URL: /Project/Search

        public ActionResult Search()
        {
            return View();
        }

        #endregion

        #region URL: /Project/SearchPopup

        public ActionResult SearchPopup()
        {
            return View(new _SearchModel());
        }

        #endregion

        #endregion

        #region Child Actions

        [ChildActionOnly]
        public ActionResult _LiteralLinkedProject(int projectId)
        {
            LinkedList<LiteItemModel> model = _projectService.GetLinkedProject(projectId);
            return View(model);
        }

        #endregion

        #region Ajax Actions

        #region URL: /Project/HasChild/{projectId}

        public JsonResult HasChild(int projectId)
        {
            bool hasChild = _projectService.HasChild(projectId);
            return Json(hasChild, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region URL: /Project/_NectProject/{projectId}

        public ActionResult _NextProjectNode(int projectId)
        {
            _DropdownlistLinkedNodeModel model = _projectService.GetNextNode(projectId);

            return PartialView(model);
        }

        #endregion

        #region URL: /Project/HasExist/{projectName}

        public JsonResult HasExist(string projectName)
        {
            bool hasExist = _projectService.GetByName(projectName) > 0;
            return Json(hasExist);
        }

        #endregion

        #region URL: /Project/_SearchResult

        [HttpPost]
        public ActionResult _SearchResult(_SearchModel model)
        {
            if (model.IdOrName)
            {
                model.Projects = _projectService.GetLinked(int.Parse(model.Input)).Projects;
            }
            else
            {
                model.Projects = _projectService.GetByPartialName(model.Input).Projects;
            }
            return PartialView(model);
        }

        #endregion

        #endregion
    }
}
