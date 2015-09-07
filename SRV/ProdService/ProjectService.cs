using System.Collections.Generic;
using System.Linq;
using FFLTask.BLL.Entity;
using FFLTask.SRV.Query;
using FFLTask.SRV.ServiceInterface;
using FFLTask.SRV.ViewModel.Project;
using FFLTask.SRV.ViewModelMap;
using Global.Core.ExtensionMethod;
using NHibernate.Linq;

namespace FFLTask.SRV.ProdService
{
    public class ProjectService : BaseService, IProjectService
    {
        public IList<FullItemModel> GetByParent(int parentId)
        {
            Project result = session.Load<Project>(parentId);
            IList<FullItemModel> models = new List<FullItemModel>();
            models.FilledBy(result.Children);

            return models;
        }

        public LinkedList<LiteItemModel> GetLinkedProject(int tailProjectId)
        {
            LinkedList<LiteItemModel> models = new LinkedList<LiteItemModel>();

            Project project = session.Load<Project>(tailProjectId);

            while (project != null)
            {
                LiteItemModel item = new LiteItemModel();
                item.FilledBy(project);
                models.AddFirst(item);

                project = project.Parent;
            }

            return models;
        }

        public int Create(CreateModel model, int? parentId, int userId)
        {
            User user = session.Load<User>(userId);

            Project project = new Project();
            if (parentId == null)
            {
                project.FilledBy(model, null);
            }
            else
            {
                Project parent = session.Load<Project>(parentId);
                project.FilledBy(model, parent);
            }

            user.Create(project);

            session.Save(project);

            return project.Id;
        }

        public int Join(int projectId, int userId)
        {
            User user = session.Load<User>(userId);
            Project project = session.Load<Project>(projectId);

            user.Join(project);

            session.Save(project);
            return project.Id;
        }

        public int GetByName(string name)
        {
            ProjectQuery query = new ProjectQuery(session.Query<Project>());
            Project project = query.GetByName(name).SingleOrDefault();
            return project == null ? 0 : project.Id;
        }

        public SummaryModel GetSummary(int projectId)
        {
            SummaryModel model = new SummaryModel();
            Project project = session.Load<Project>(projectId);
            model.FilledBy(project);
            return model;
        }

        public _DropdownlistLinkedModel GetDropdownlistLink(int userId)
        {
            _DropdownlistLinkedModel model = new _DropdownlistLinkedModel();

            IList<Project> projects = session.Load<User>(userId).RootProjects;

            if (projects != null && projects.Count > 0)
            {
                model.LinkedProject = new LinkedList<_DropdownlistLinkedNodeModel>();
                model.LinkedProject.AddFirst(getFirstNode(projects));
                model.LinkedProject.FilledByHead(projects.First());
                model.TailSelectedProject = model.LinkedProject.Last.Value.CurrentProject;
            }

            return model;
        }

        public _DropdownlistLinkedModel GetDropdownlistLink(int userId, int projectId)
        {
            _DropdownlistLinkedModel model = new _DropdownlistLinkedModel();

            Project project = session.Load<Project>(projectId);
            IList<Project> projects = session.Load<User>(userId).RootProjects;

            model.LinkedProject = new LinkedList<_DropdownlistLinkedNodeModel>();

            model.LinkedProject.FilledByTail(project);

            IList<LiteItemModel> headProjects = new List<LiteItemModel>();
            headProjects.FilledBy(projects);
            model.LinkedProject.First.Value.Projects = headProjects;

            model.TailSelectedProject = model.LinkedProject.Last.Value.CurrentProject;
            model.SelectedProjectHasChild = !project.Children.IsNullOrEmpty();

            return model;
        }

        private _DropdownlistLinkedNodeModel getFirstNode(IList<Project> rootProjects)
        {
            _DropdownlistLinkedNodeModel firstNode = new _DropdownlistLinkedNodeModel();
            firstNode.Projects = new List<LiteItemModel>();
            foreach (Project project in rootProjects)
            {
                LiteItemModel projectItemModel = new LiteItemModel();
                projectItemModel.FilledBy(project);
                firstNode.Projects.Add(projectItemModel);
            }
            firstNode.CurrentProject = firstNode.Projects.First();
            return firstNode;
        }

        public bool HasChild(int projectId)
        {
            Project project = session.Load<Project>(projectId);
            return !project.Children.IsNullOrEmpty();
        }

        public _SearchModel GetLinked(int projectId)
        {
            _SearchModel model = new _SearchModel();
            model.Projects = new List<_LiteralLinkedModel>();
            Project loadProject = session.Get<Project>(projectId);
            if (loadProject != null)
            {
                _LiteralLinkedModel item = new _LiteralLinkedModel();
                item.Filledby(loadProject);
                model.Projects.Add(item);
            }

            return model;
        }

        public _SearchModel GetByPartialName(string projectName)
        {
            _SearchModel model = new _SearchModel();
            model.Projects = new List<_LiteralLinkedModel>();

            ProjectQuery query = new ProjectQuery(session.Query<Project>());
            IList<Project> queryProjects = query.GetByPartialName(projectName).ToList();
            foreach (Project project in queryProjects)
            {
                _LiteralLinkedModel item = new _LiteralLinkedModel();
                item.Filledby(project);
                model.Projects.Add(item);
            }

            return model;
        }

        public _DropdownlistLinkedNodeModel GetNextNode(int projectId)
        {
            _DropdownlistLinkedNodeModel model = new _DropdownlistLinkedNodeModel();

            Project project = session.Load<Project>(projectId);
            if (!project.Children.IsNullOrEmpty())
            {
                model.Projects = new List<LiteItemModel>();
                model.Projects.FilledBy(project.Children);
                model.CurrentProject = model.Projects[0];
            }

            return model;
        }

        public JoinModel GetJoinTree(int projectId, int currentUserId)
        {
            JoinModel model = new JoinModel();
            Project project = session.Load<Project>(projectId);
            model.FilledBy(project, currentUserId);

            return model;
        }

        public EditModel GetEdit(int projectId)
        {
            Project project = session.Load<Project>(projectId);

            EditModel model = new EditModel();
            model.FilledBy(project);

            return model;
        }

        public bool HasJoined(int projectId, int userId)
        {
            User user = session.Load<User>(userId);
            Project project = session.Load<Project>(projectId);

            return user.Authorizations.Where(a => a.Project.Id == projectId).Count() > 0;
        }

        public bool ParentIsOffspring(int childId, int parentId)
        {
            Project child = session.Get<Project>(childId);
            Project parent = session.Get<Project>(parentId);

            return parent.IsOffspring(child);
        }

        public void Modify(EditModel model)
        {
            Project project = session.Load<Project>(model.Id);
            Project newParent = model.ParentId.HasValue ? 
                session.Load<Project>(model.ParentId) : 
                null;

            project.Name = model.Name;
            project.Description = model.Description;
            project.ChangeParent(newParent);
        }
    }
}
