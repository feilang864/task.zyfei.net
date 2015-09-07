using System.Collections.Generic;

namespace FFLTask.SRV.ServiceInterface
{
    public interface IUserService
    {
        FFLTask.SRV.ViewModel.Account.UserModel GetUser(int userId);
        IList<FFLTask.SRV.ViewModel.Account.UserModel> GetOwners(int projectId);
        IList<FFLTask.SRV.ViewModel.Account.UserModel> GetAllOwners(int projectId);
        IList<FFLTask.SRV.ViewModel.Account.UserModel> GetPublishers(int projectId);
        IList<FFLTask.SRV.ViewModel.Account.UserModel> GetAllPublishers(int projectId);
        IList<FFLTask.SRV.ViewModel.Account.UserModel> GetAccepters(int projectId);
        IList<FFLTask.SRV.ViewModel.Account.UserModel> GetAllAccepters(int projectId);

        bool HasUnknownMessage(int userId);
        bool HasJoinedProject(int userId);

        //TODO: comment first
        //IList<UI.ViewModel.Project.GroupModel> GetGroupsWithProjects(int userId);
        //IList<UI.ViewModel.Group.GroupItemModel> GetGroups(int userId);
    }
}
