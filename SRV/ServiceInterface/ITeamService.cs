using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FFLTask.GLB.Global.Enum;

namespace FFLTask.SRV.ServiceInterface
{
    public interface ITeamService
    {
        FFLTask.SRV.ViewModel.Team.SearchModel GroupedByRole(int userId);

        IList<FFLTask.SRV.ViewModel.Team.DismissSearchResultItemModel> GroupedByProject(
            int userId);

        IList<ViewModel.Team.TransferItemModel> GetTasks(
            FFLTask.SRV.ViewModel.Team.TransferModel transferModel);
        
        IList<Status?> GetAllStatus(
            FFLTask.SRV.ViewModel.Team.TransferModel transferModel);

        void HandOver(ViewModel.Team.TransferItemModel model, 
            Role role, int succesorId, int operaterId);

    }
}
