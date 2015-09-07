using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using FFLTask.BLL.Entity;
using FFLTask.GLB.Global.Enum;
using FFLTask.SRV.Query;
using FFLTask.SRV.ServiceInterface;
using FFLTask.SRV.ViewModel.Account;
using FFLTask.SRV.ViewModel.Task;
using FFLTask.SRV.ViewModelMap;
using Global.Core.ExtensionMethod;
using NHibernate.Linq;
using FFLTask.SRV.ViewModel.Team;
using AutoMapper;

namespace FFLTask.SRV.ProdService
{
    public class TaskService : BaseService, ITaskService
    {
        IProjectService _projectService;
        IUserService _userService;
        IAuthroizationService _authService;
        IProjectConfigService _projectConfigService;
        HistoryItemQuery _historyItemQuery;
        private IQueryable<Task> _querySource;
        public TaskService()
        {
            _projectService = new ProjectService();
            _userService = new UserService();
            _authService = new AuthorizationService();
            _projectConfigService = new ProjectConfigService();
            _historyItemQuery = new HistoryItemQuery(session.Query<HistoryItem>());
            _querySource = session.Query<Task>();
        }

        #region Implementations

        #region Get Methods

        public IList<FullItemModel> Get(string sort, bool des, ListModel model)
        {
            //should be in the controller for checking usage.
            if (model.TaskId != null)
            {
                Task task = session.Load<Task>(model.TaskId);
                FullItemModel item = new FullItemModel();
                item.FilledBy(task);
                return new List<FullItemModel> { item };
            }

            IList<FullItemModel> models = new List<FullItemModel>();

            Project current = session.Load<Project>(model.CurrentProject.TailSelectedProject.Id);
            IList<int> allProjectIds = current.GetDescendantIds();

            var tasks = _querySource
                .Get(model, allProjectIds)
                .Sort(sort, des)
                .Paged(model.Pager);

            foreach (var task in tasks)
            {
                FullItemModel item = new FullItemModel();
                item.FilledBy(task);
                models.Add(item);
            }

            return models;
        }

        public FullItemModel Get(int taskId)
        {
            Task task = session.Load<Task>(taskId);
            FullItemModel model = new FullItemModel();
            model.FilledBy(task);

            model.Project = new FFLTask.SRV.ViewModel.Project.FullItemModel();
            model.Project.FilledBy(task.Project);

            return model;
        }

        public int GetCount(ListModel model)
        {
            Project current = session.Load<Project>(model.CurrentProject.TailSelectedProject.Id);
            IList<int> allProjectIds = current.GetDescendantIds();

            return _querySource
                .Get(model, allProjectIds)
                .Count();
        }

        public IList<TaskHistoryItemModel> GetHistory(int taskId)
        {
            Task task = session.Load<Task>(taskId);

            IList<TaskHistoryItemModel> models = new List<TaskHistoryItemModel>();
            var sortedHistory = task.Histroy.OrderByDescending(x => x.Id);
            foreach (var item in sortedHistory)
            {
                TaskHistoryItemModel model = new TaskHistoryItemModel();
                models.Add(new TaskHistoryItemModel().FilledBy(item));
            }

            return models;
        }

        public EditModel GetEdit(int taskId, UserModel currentUser)
        {
            Task task = session.Load<Task>(taskId);

            EditModel model = new EditModel();
            model.FilledBy(task);

            model.CurrentUser = currentUser;
            model.QualifiedStatus = getQualifiedStatus(task.CurrentStatus, task);

            #region Depend on services and project id

            int projectId = task.Project.Id;
            int currentUserId = currentUser.Id.Value;

            model.CanOwn = _authService.GetTokens(currentUserId, projectId).Contains(Token.Owner);
            model.Owners = _userService.GetOwners(projectId);
            model.Accepters = _userService.GetAccepters(projectId);
            model.AllDifficulties = _projectConfigService.GetDifficulties(projectId);
            model.AllPriorities = _projectConfigService.GetPriorities(projectId);
            model.AllQualities = _projectConfigService.GetQualities(projectId);
            model.CurrentProject = _projectService.GetDropdownlistLink(currentUser.Id.Value, projectId);

            model.CanAutoCompleteParent = CanAutoComplete(taskId);
            model.CanAutoAccepterParent = CanAutoAccept(taskId);

            #endregion

            model.Attachments = new List<AttachmentModel>();
            foreach (Attachment attachment in task.Attachments)
            {
                AttachmentModel item = new AttachmentModel();
                item.FilledBy(attachment);
                model.Attachments.Add(item);
            }

            return model;
        }

        public string GetTitle(int taskId)
        {
            Task task = session.Get<Task>(taskId);
            return task == null ? string.Empty : task.Title;
        }

        public TaskRelationModel GetRelation(int taskId)
        {
            TaskRelationModel model = new TaskRelationModel();
            Task task = session.Load<Task>(taskId);
            model.FilledBy(task);

            return model;
        }

        public LiteItemModel GetLite(int taskId)
        {
            Task task = session.Get<Task>(taskId);
            LiteItemModel model = null;
            if (task != null)
            {
                model = new LiteItemModel();
                model.FilledBy(task);
            }
            return model;
        }

        public SequenceModel GetSequence(int taskId)
        {
            SequenceModel model = new SequenceModel();

            Task parent = session.Load<Task>(taskId);

            model.Parent = new LiteItemModel();
            model.Parent.FilledBy(parent);

            model.Children = new List<LiteItemModel>();
            foreach (Task task in parent.Children)
            {
                LiteItemModel item = new LiteItemModel();
                item.FilledBy(task);
                model.Children.Add(item);
            }

            return model;
        }

        #endregion

        public int Create(NewModel model, int creatorId)
        {
            Task task = new Task();

            task.Publisher = session.Load<User>(creatorId);
            task.Project = session.Load<Project>(model.CurrentProject.TailSelectedProject.Id);
            addParent(task, model.TaskItem.Parent);
            setAccepter(task, model.TaskItem.Accepter);

            model.TaskItem.Fill(task);

            task.Publish();

            session.Save(task);
            session.Flush();
            // important: to ensure the task is published first and then assigned later!

            if (model.TaskItem.Owner.Id.HasValue)
            {
                task.Owner = session.Load<User>(model.TaskItem.Owner.Id.Value);
                task.Assign();
            }

            return task.Id;
        }

        public void Assign(EditModel model)
        {
            Task task = session.Load<Task>(model.TaskItem.LiteItem.Id);

            if (model.TaskItem.Owner.Id != null)
            {
                task.Owner = session.Load<User>(model.TaskItem.Owner.Id);
                task.Assign();
            }
            else
            {
                task.CancelAssign();
            }

        }

        public void Own(int taskId, int ownerId)
        {
            Task task = session.Load<Task>(taskId);
            User owner = session.Load<User>(ownerId);
            task.Owner = owner;
            task.Own();
        }

        public void Handle(EditModel model)
        {
            int currentStatus = model.SelectedQualifiedStatus.Stage;
            Task task = session.Load<Task>(model.TaskItem.LiteItem.Id);

            string comment = model.Comment;
            if (currentStatus == (int)Status.Own)
            {
                task.Own();
            }
            else if (currentStatus == (int)Status.BeginWork)
            {
                task.BeginWork();
            }
            else if (currentStatus == (int)Status.Pause)
            {
                task.Pause();
            }
            else if (currentStatus == (int)Status.Doubt)
            {
                task.Doubt();
            }
            else if (currentStatus == (int)Status.Complete)
            {
                task.Complete();
                if (model.AutoCompleteParent)
                {
                    task.AutoCompleteAncestors();
                }
            }
            else if (currentStatus == (int)Status.Quit)
            {
                task.Quit();
            }
            else if (currentStatus == (int)Status.Dissent)
            {
                task.Dissent();
            }
        }

        public void Expire(int taskId)
        {
            throw new NotImplementedException();
        }

        public void UpdateTaskProperty(EditModel model)
        {
            Task task = session.Load<Task>(model.TaskItem.LiteItem.Id);

            task.Project = session.Load<Project>(model.CurrentProject.TailSelectedProject.Id);
            setParent(task, model.TaskItem.Parent);
            if (model.TaskItem != null)
            {
                setAccepter(task, model.TaskItem.Accepter);
            }
            model.TaskItem.Fill(task);
            task.UpdateProperty();
        }

        public void Accept(EditModel model)
        {
            Task task = session.Load<Task>(model.TaskItem.LiteItem.Id);

            task.Accept(model.TaskItem.Quality);
            if (model.AutoAcceptParent)
            {
                task.AutoAcceptAncestors();
            }
        }

        public void Refuse(EditModel model)
        {
            Task task = session.Load<Task>(model.TaskItem.LiteItem.Id);
            task.RefuseAccept();
        }

        public void Remove(int taskId, string comment)
        {
            Task task = session.Load<Task>(taskId);
            task.Remove();
        }

        public void Resume(int taskId, string comment)
        {
            Task task = session.Load<Task>(taskId);
            task.Resume();
        }

        public void Comment(EditModel model)
        {
            Task task = session.Load<Task>(model.TaskItem.LiteItem.Id);
            User current = session.Load<User>(model.CurrentUser.Id);
            if (model.AddresseeId.HasValue)
            {
                User addressee = session.Load<User>(model.AddresseeId);
                task.Comment(current, addressee, model.Comment);
            }
            else
            {
                task.AddHistory(current, model.Comment);
            }
        }

        public void UploadFiles(IList<string> files, int taskId, int uploaderId)
        {
            Task task = session.Load<Task>(taskId);
            User uploader = session.Load<User>(uploaderId);
            task.Attachments = task.Attachments ?? new List<Attachment>();

            foreach (string attachment in files)
            {
                Attachment att = new Attachment
                {
                    Task = task,
                    Uploader = uploader,
                    FileName = Path.GetFileName(attachment)
                };
                task.Attachments.Add(att);
            }
        }

        public void UpdateTaskSequence(IList<int> taskIds)
        {
            int count = taskIds.Count;
            for (int i = 0; i < count; i++)
            {
                Task tast = session.Load<Task>(taskIds[i]);
                tast.Sequence = i + 1;
            }
        }

        public _SumModel GetSum(ListModel model)
        {
            _SumModel sumModel = new _SumModel();

            Project current = session.Load<Project>(model.CurrentProject.TailSelectedProject.Id);
            IList<int> allProjectIds = current.GetDescendantIds();
            var allTasks = _querySource
                .Get(model, allProjectIds);

            sumModel.Types = new Dictionary<string, int>();
            sumModel.Types.Add("虚", allTasks.Count(t => t.IsVirtual));
            sumModel.Types.Add("实", allTasks.Count(t => !t.IsVirtual));

            sumModel.Publish = allTasks.GroupBy(t => t.Publisher).ToDictionary(t => t.Key.Name, t => t.Count());
            sumModel.Own = allTasks.GroupBy(t => t.Owner).ToDictionary(t => getName(t.Key), t => t.Count());
            sumModel.Accept = allTasks.GroupBy(t => t.Accepter).ToDictionary(t => t.Key.Name, t => t.Count());

            sumModel.Priorities = allTasks.GroupBy(t => t.Priority).ToDictionary(t => getDescription(t.Key), t => t.Count());
            sumModel.Difficulties = allTasks.GroupBy(t => t.Difficulty).ToDictionary(t => getDescription(t.Key), t => t.Count());

            sumModel.ConsumeTime = new Dictionary<string, int>();
            sumModel.ConsumeTime.Add("预计", allTasks.Sum(t => t.ExpectWorkPeriod) ?? 0);
            sumModel.ConsumeTime.Add("实际", allTasks.Sum(t => t.ActualWorkPeriod) ?? 0);

            sumModel.Status = allTasks.GroupBy(t => t.CurrentStatus).ToDictionary(t => t.Key.GetEnumDescription(), t => t.Count());

            sumModel.OverDue = new Dictionary<string, int>();
            sumModel.OverDue.Add("条数", allTasks.Where(t => t.OverDue > 0).Count());
            sumModel.OverDue.Add("时间", allTasks.Sum(t => t.OverDue) ?? 0);

            sumModel.Qualities = allTasks.GroupBy(t => t.Quality).ToDictionary(t => getDescription(t.Key), t => t.Count());

            sumModel.Doubt = new Dictionary<string, int>();
            sumModel.Doubt.Add("条数", allTasks.Count(t => t.Histroy.Count(h => h.Status == Status.Doubt) > 0));
            sumModel.Doubt.Add("次数", _historyItemQuery.Get(allTasks, Status.Doubt).Count());

            sumModel.RefuseAccept = new Dictionary<string, int>();
            sumModel.RefuseAccept.Add("条数", allTasks.Count(t => t.Histroy.Count(h => h.Status == Status.RefuseAccept) > 0));
            sumModel.RefuseAccept.Add("次数", _historyItemQuery.Get(allTasks, Status.RefuseAccept).Count());

            return sumModel;
        }

        public bool CanAutoAccept(int taskId)
        {
            return canAuto(taskId, Status.Accept);
        }

        public bool CanAutoComplete(int taskId)
        {
            return canAuto(taskId, Status.Complete);
        }

        public bool ParentIsOffspring(int childId, int parentId)
        {
            Task child = session.Get<Task>(childId);
            Task parent = session.Get<Task>(parentId);

            return parent.IsOffspring(child);
        }

        public IList<LiteItemModel> GetStartWith(string title)
        {
            IList<Task> tasks = _querySource.GetStartWith(title).ToList();
            IList<LiteItemModel> models = new List<LiteItemModel>();

            foreach (var item in tasks)
            {
                models.Add(Mapper.Map<LiteItemModel>(item));
            }

            return models;
        }

        #endregion

        #region Private Methods

        private void addDoubt(IList<StatusModel> qualified, Task task)
        {
            StatusModel doubtModel = new StatusModel();
            doubtModel.FilledBy(Status.Doubt);

            if (task.Publisher != task.Owner || task.Owner != task.Accepter)
            {
                qualified.Add(doubtModel);
            }
        }

        private IList<StatusModel> getQualifiedStatus(Status status, Task task)
        {
            IList<StatusModel> qualified = new List<StatusModel>();

            StatusModel beginWorkModel = new StatusModel();
            beginWorkModel.FilledBy(Status.BeginWork);

            StatusModel quitModel = new StatusModel();
            quitModel.FilledBy(Status.Quit);

            StatusModel completeModel = new StatusModel();
            completeModel.FilledBy(Status.Complete);

            StatusModel pauseModel = new StatusModel();
            pauseModel.FilledBy(Status.Pause);

            StatusModel dissentModel = new StatusModel();
            dissentModel.FilledBy(Status.Dissent);

            if (status == Status.Assign
                || status == Status.Own
                || status == Status.Dissent)
            {
                qualified.Add(beginWorkModel);
                qualified.Add(quitModel);
            }
            else if (status == Status.BeginWork)
            {
                qualified.Add(completeModel);
                qualified.Add(pauseModel);
                qualified.Add(quitModel);
                addDoubt(qualified, task);
            }
            else if (status == Status.Pause)
            {
                qualified.Add(beginWorkModel);
                qualified.Add(quitModel);
                addDoubt(qualified, task);
                qualified.Add(completeModel);
            }
            else if (status == Status.Doubt
                || status == Status.Update)
            {
                qualified.Add(beginWorkModel);
                qualified.Add(quitModel);
                qualified.Add(completeModel);
            }
            else if (status == Status.Complete)
            {
                qualified.Add(beginWorkModel);
            }
            else if (status == Status.RefuseAccept)
            {
                qualified.Add(beginWorkModel);
                qualified.Add(quitModel);
                qualified.Add(dissentModel);
            }
            return qualified;
        }

        private void setAccepter(Task task, UserModel accepter)
        {
            if (accepter.Id.HasValue)
            {
                task.Accepter = session.Load<User>(accepter.Id.Value);
            }
            else
            {
                task.Accepter = task.Publisher;
            }
        }

        private void addParent(Task task, LiteItemModel parent)
        {
            if (parent.Id.HasValue)
            {
                Task parentTask = session.Load<Task>(parent.Id);
                parentTask.AddChild(task);
            }
        }

        private void setParent(Task task, LiteItemModel parent)
        {
            if (parent.Id.HasValue)
            {
                Task newParent = session.Load<Task>(parent.Id);
                task.ChangeParent(newParent);
            }
            else
            {
                task.RemoveParent();
            }
        }

        private string getName(User user)
        {
            if (user == null)
            {
                return "未指定";
            }
            else
            {
                return user.Name;
            }
        }

        private string getDescription(Enum value)
        {
            if (value != null)
            {
                return value.GetEnumDescription();
            }
            else
            {
                return "未指定";
            }
        }

        private bool canAuto(int taskId, Status action)
        {
            Task task = session.Load<Task>(taskId);

            Task parent = task.Parent;
            if (parent == null || !parent.IsVirtual)
            {
                return false;
            }

            return task.IsLastInBrothers(action);
        }

        #endregion
    }
}
