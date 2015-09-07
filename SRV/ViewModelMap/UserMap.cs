using System.Collections.Generic;
using System.Linq;
using FFLTask.BLL.Entity;
using FFLTask.SRV.ViewModel.Account;
using Global.Core.ExtensionMethod;

namespace FFLTask.SRV.ViewModelMap
{
    public static class UserMap
    {
        public static UserModel FilledBy(this UserModel model, User user)
        {
            model.Id = user.Id;
            model.Name = user.Name;
            model.AuthCode = user.AuthenticationCode;
            model.HasLogon = true;
            model.IsAdmin = user.IsAdmin;
            model.HasJoinedProject = !user.Authorizations.IsNullOrEmpty();

            return model;
        }

        public static void FilledBy(this IList<UserModel> model, IList<User> users)
        {
            foreach (User user in users)
            {
                UserModel item = new UserModel();
                item.FilledBy(user);
                model.Add(item);
            }
        }
    }
}
