using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FFLTask.BLL.Entity;
using FFLTask.SRV.ViewModel.Project;
using FFLTask.SRV.ViewModel.Account;
using FFLTask.SRV.ViewModel.Auth;

namespace FFLTask.SRV.ViewModelMap
{
    public static class AuthorizationMap
    {
        public static void FilledBy(this AuthorizationModel model, Authorization authorization)
        {
            model.Id = authorization.Id;
            model.CanAdmin = authorization.IsAdmin;
            model.CanOwn = authorization.IsOwner;
            model.CanPublish = authorization.IsPublisher;

            UserModel user = new UserModel();
            user.FilledBy(authorization.User);
            model.User = user;
        }

        internal static void FilledBy(this IList<AuthorizationModel> models, IList<Authorization> authorizations)
        {
            foreach (Authorization auth in authorizations)
            {
                AuthorizationModel model = new AuthorizationModel();
                model.FilledBy(auth);
                models.Add(model);
            }
        }

        public static void FilledBy(this IList<ProjectAuthorizationModel> models, IList<Authorization> authorizations)
        {
            foreach (Authorization authorization in authorizations)
            {
                ProjectAuthorizationModel model = new ProjectAuthorizationModel();
                model.FilledBy(authorization.Project);
                models.Add(model);
            }
        }

        public static void FilledBy(this ProjectAuthorizationModel model, Project project)
        {
            model.Id = project.Id;
            model.Name = project.Name;
            model.Authorizations = new List<AuthorizationModel>();
            model.Authorizations.FilledBy(project.Authorizations);
        }
    }
}
