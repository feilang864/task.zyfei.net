using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FFLTask.SRV.ServiceInterface
{
    public interface IAuthroizationService
    {
        IList<FFLTask.GLB.Global.Enum.Token> GetTokens(int userId, int projectId);

        IList<FFLTask.SRV.ViewModel.Project.ProjectMemberModel> Get(int projectId);

        IList<FFLTask.SRV.ViewModel.Auth.ProjectAuthorizationModel> GetAdmined(int userId);

        //TODO: merge/reuse with GetAdmined(userId)?
        IList<int> GetAdminedProjectIds(int userId);

        void Update(FFLTask.SRV.ViewModel.Project.AuthorizationModel model);

        void Dismiss(int userId, IList<int> projectIds);
    }
}
