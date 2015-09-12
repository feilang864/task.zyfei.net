using System.Collections.Generic;

namespace FFLTask.SRV.ServiceInterface
{
    public interface IUserService
    {
        ViewModel.User.ProfileModel GetProfile(int userId);
        FFLTask.SRV.ViewModel.Account.UserModel GetUser(int userId);
        IList<FFLTask.SRV.ViewModel.Account.UserModel> GetOwners(int projectId);
        IList<FFLTask.SRV.ViewModel.Account.UserModel> GetAllOwners(int projectId);
        IList<FFLTask.SRV.ViewModel.Account.UserModel> GetPublishers(int projectId);
        IList<FFLTask.SRV.ViewModel.Account.UserModel> GetAllPublishers(int projectId);
        IList<FFLTask.SRV.ViewModel.Account.UserModel> GetAccepters(int projectId);
        IList<FFLTask.SRV.ViewModel.Account.UserModel> GetAllAccepters(int projectId);
        IEnumerable<ViewModel.User.JoinedProjectItemModel> GetJoinedProjects(int userId);

        bool HasUnknownMessage(int userId);
        bool HasJoinedProject(int userId);

        //TODO: comment first
        //IList<UI.ViewModel.Project.GroupModel> GetGroupsWithProjects(int userId);
        //IList<UI.ViewModel.Group.GroupItemModel> GetGroups(int userId);

        void SaveProfile(ViewModel.User.ProfileModel model, int userId);


    }
}
