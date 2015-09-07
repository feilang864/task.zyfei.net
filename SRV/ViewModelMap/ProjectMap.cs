using System.Collections.Generic;
using System.Linq;
using FFLTask.BLL.Entity;
using FFLTask.SRV.ViewModel.Account;
using FFLTask.SRV.ViewModel.Project;
using FFLTask.SRV.ViewModel.Shared;

namespace FFLTask.SRV.ViewModelMap
{
    public static class ProjectMap
    {
        public static void FilledBy(this LiteItemModel model, Project project)
        {
            model.Id = project.Id;
            model.Name = project.Name;
        }

        public static void FilledBy(this IList<LiteItemModel> model, IList<Project> projects)
        {
            foreach (Project project in projects)
            {
                LiteItemModel projectitemmodel = new LiteItemModel();
                projectitemmodel.FilledBy(project);
                model.Add(projectitemmodel);
            }
        }

        public static FullItemModel FilledBy(this FullItemModel model, Project project)
        {
            model.LiteItem = new LiteItemModel();
            model.LiteItem.FilledBy(project);

            model.Description = project.Description;
            if (project.Children != null)
            {
                model.Children = new List<FullItemModel>();
                model.Children.FilledBy(project.Children);
            }

            return model;
        }

        public static void FilledBy(this Project project, CreateModel model, Project parent)
        {
            project.Parent = parent;
            project.Name = model.Name;
            project.Description = model.Introduction;
        }

        public static void FilledBy(this JoinModel model, Project project, int userId)
        {
            model.Item = new FullItemModel();
            model.Children = new List<JoinModel>();

            model.Item.FilledBy(project);
            model.HasJoined = project.Authorizations.Count(x => x.User.Id == userId) > 0;
            foreach (Project childProject in project.Children)
            {
                JoinModel childModel = new JoinModel();
                childModel.FilledBy(childProject, userId);
                model.Children.Add(childModel);
            }
        }

        public static void FilledBy(this SummaryModel model, Project project)
        {
            if (project.Authorizations != null)
            {
                model.Authorizations = new List<AuthorizationModel>();
                model.Authorizations.FilledBy(project.Authorizations);
            }

            if (project.Children != null)
            {
                model.Projects = new List<FullItemModel>();
                model.Projects.FilledBy(project.Children);
            }

            model.Abstract = new AbstractModel();
            model.Abstract.Filledby(project);
        }


        private static void Filledby(this AbstractModel model, Project project)
        {
            model.Name = project.Name;
            model.Description = project.Description;
            model.Founder = new UserModel();
            model.Founder.FilledBy(project.Founder);
        }

        public static void FilledBy(this IList<FullItemModel> model, IList<Project> projects)
        {
            foreach (Project project in projects)
            {
                FullItemModel projectitemmodel = new FullItemModel();
                projectitemmodel.FilledBy(project);
                model.Add(projectitemmodel);
            }
        }

        public static void FilledByTail(this LinkedList<_DropdownlistLinkedNodeModel> models, Project tail)
        {
            while (tail.Parent != null)
            {
                _DropdownlistLinkedNodeModel nodeModel = new _DropdownlistLinkedNodeModel();
                nodeModel.filledBy(tail);
                models.AddFirst(nodeModel);
                tail = tail.Parent;
            }

            _DropdownlistLinkedNodeModel firstNode = new _DropdownlistLinkedNodeModel
            {
                CurrentProject = new LiteItemModel()
            };
            firstNode.CurrentProject.FilledBy(tail);
            models.AddFirst(firstNode);
        }

        public static void FilledByHead(this LinkedList<_DropdownlistLinkedNodeModel> models, Project head)
        {
            while (head.Children != null && head.Children.Count > 0)
            {
                _DropdownlistLinkedNodeModel lastModel = new _DropdownlistLinkedNodeModel();
                lastModel.Projects = new List<LiteItemModel>();
                lastModel.Projects.FilledBy(head.Children);
                lastModel.CurrentProject = lastModel.Projects.First();
                models.AddLast(lastModel);
                head = head.Children.First();
            }
        }

        private static void filledBy(this _DropdownlistLinkedNodeModel model, Project project)
        {
            model.CurrentProject = new LiteItemModel();
            model.CurrentProject.FilledBy(project);
            model.Projects = new List<LiteItemModel>();
            model.Projects.FilledBy(project.Parent.Children);
        }

        public static void Filledby(this _LiteralLinkedModel model, Project tail)
        {
            model.LinkedList = new LinkedList<LiteItemModel>();
            model.TailedId = tail.Id;
            while (tail != null)
            {
                LiteItemModel item = new LiteItemModel();
                item.FilledBy(tail);
                model.LinkedList.AddFirst(item);

                tail = tail.Parent;
            }
        }

        public static void FilledBy(this IList<_LiteralLinkedModel> model, IList<Project> projects)
        {
            foreach (Project project in projects)
            {
                _LiteralLinkedModel item = new _LiteralLinkedModel();
                item.Filledby(project);
                model.Add(item);
            }
        }

        public static void FilledBy(this EditModel model, Project project)
        {
            model.Id = project.Id;
            model.Name = project.Name;
            model.Description = project.Description;
            if (project.Parent != null)
            {
                model.ParentId = project.Parent.Id;
            }
        }
    }
}
