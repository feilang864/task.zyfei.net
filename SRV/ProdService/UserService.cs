using System.Collections.Generic;
using System.Linq;
using FFLTask.BLL.Entity;
using FFLTask.GLB.Global.Enum;
using FFLTask.SRV.Query;
using FFLTask.SRV.ServiceInterface;
using FFLTask.SRV.ViewModel.Account;
using FFLTask.SRV.ViewModelMap;
using NHibernate.Linq;
using FFLTask.SRV.ViewModel.User;
using AutoMapper;

namespace FFLTask.SRV.ProdService
{
    public class UserService : BaseService, IUserService
    {
        public ProfileModel GetProfile(int userId)
        {
            User user = session.Load<User>(userId);

            ProfileModel model = new ProfileModel();
            return Mapper.Map<ProfileModel>(user.Profile);
        }

        public UserModel GetUser(int userId)
        {
            UserModel model = new UserModel();

            User user = session.Load<User>(userId);
            return model.FilledBy(user);
        }

        public IList<UserModel> GetOwners(int projectId)
        {
            Project project = session.Load<Project>(projectId);

            return fill(project.Owners);
        }

        public IList<UserModel> GetAllOwners(int projectId)
        {
            Project current = session.Load<Project>(projectId);
            IList<int> allProjectIds = current.GetDescendantIds();

            var owners = session.Query<Authorization>()
                .InProjects(allProjectIds)
                .Where(a => a.IsOwner)
                .Select(a => a.User)
                .Distinct();

            return fill(owners);
        }

        public IList<UserModel> GetPublishers(int projectId)
        {
            Project project = session.Load<Project>(projectId);

            return fill(project.Publisher);
        }

        public IList<UserModel> GetAllPublishers(int projectId)
        {
            Project current = session.Load<Project>(projectId);
            IList<int> allProjectIds = current.GetDescendantIds();

            var publishers = session.Query<Authorization>()
                .InProjects(allProjectIds)
                .Where(a => a.IsPublisher)
                .Select(a => a.User)
                .Distinct();

            return fill(publishers); ;
        }

        public IList<UserModel> GetAccepters(int projectId)
        {
            Project project = session.Load<Project>(projectId);

            return fill(project.AllUsers);
        }

        public IList<UserModel> GetAllAccepters(int projectId)
        {
            Project current = session.Load<Project>(projectId);
            IList<int> allProjectIds = current.GetDescendantIds();

            var accepters = session.Query<Authorization>()
                .InProjects(allProjectIds)
                .Select(a => a.User)
                .Distinct();

            return fill(accepters);
        }

        private IList<UserModel> fill(IEnumerable<User> users)
        {
            IList<UserModel> models = new List<UserModel>();
            models.FilledBy(users.ToList());

            return models;
        }

        public bool HasUnknownMessage(int userId)
        {
            return session.Query<Message>()
                .CanShow(MessageFor.Addressee)
                .To(userId)
                .NotRead()
                .FirstOrDefault() != null;
        }

        public bool HasJoinedProject(int userId)
        {
            throw new System.NotImplementedException();
        }

        public void SaveProfile(ProfileModel model, int userId)
        {
            User user = session.Load<User>(userId);
            user.Profile = Mapper.Map<FFLTask.BLL.Entity.Profile>(model);
        }
    }
}
