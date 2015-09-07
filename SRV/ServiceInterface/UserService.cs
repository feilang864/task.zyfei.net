using System.Collections.Generic;
using System.Linq;
using BLL.Entity;
using NHibernate.Linq;
using ViewModelMap;
using ServiceInterface;
using UI.ViewModel.Account;
using UI.ViewModel.Project;

namespace ProdService
{
    public class UserService : BaseService, IUserService
    {
        public UserModel GetUser(int userId)
        {
            UserModel model = new UserModel();

            User user = session.Load<User>(userId);
            return model.FilledBy(user);
        }

        public IList<UserModel> GetOwners(int projectId)
        {
            IList<UserModel> models = new List<UserModel>();
            Project project = session.Load<Project>(projectId);
            foreach (var owner in project.Owners)
            {
                UserModel model = new UserModel();
                model.FilledBy(owner);
                models.Add(model);
            }
            return models;
        }

        public IList<UserModel> GetPublishers(int projectId)
        {
            IList<UserModel> models = new List<UserModel>();
            Project project = session.Load<Project>(projectId);
            foreach (var publisher in project.Publisher)
            {
                UserModel model = new UserModel();
                model.FilledBy(publisher);
                models.Add(model);
            }
            return models;
        }

        //TODO: comment first
        //public IList<GroupModel> GetGroupsWithProjects(int userId)
        //{
        //    IList<GroupModel> models = new List<GroupModel>();
        //    User user = session.Load<User>(userId);
        //    foreach (var group in user.Groups)
        //    {
        //        GroupModel groupModel = new GroupModel();
        //        groupModel.Item = new GroupItemModel().FilledBy(group);
        //        groupModel.Projects = new List<ProjectModel>();
        //        foreach (var project in group.Projects)
        //        {
        //            ProjectModel projectModel = new ProjectModel();
        //            projectModel.Item = new ProjectItemModel().FilledBy(project);
        //            projectModel.HasJoined = user.Authorizations.Where(a => a.Project == project).Count() > 0;
        //            groupModel.Projects.Add(projectModel);
        //        }
        //        models.Add(groupModel);
        //    }
        //    return models;
        //}

        //public IList<GroupItemModel> GetGroups(int userId)
        //{
        //    IList<GroupItemModel> models = new List<GroupItemModel>();
        //    User user = session.Load<User>(userId);
        //    foreach (var group in user.Groups)
        //    {
        //        models.Add(new GroupItemModel().FilledBy(group));
        //    }
        //    return models;
        //}
    }
}
