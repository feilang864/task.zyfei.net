using System.Linq;
using FFLTask.BLL.Entity;
using FFLTask.SRV.Query;
using FFLTask.SRV.ServiceInterface;
using FFLTask.SRV.ViewModel.Account;
using Global.Core.ExtensionMethod;
using Global.Core.Helper;
using NHibernate.Linq;

namespace FFLTask.SRV.ProdService
{
    public class RegisterService : BaseService, IRegisterService
    {
        private UserQuery _userQuery;
        public RegisterService()
        {
            _userQuery = new UserQuery(session.Query<User>());
        }

        public int GetUserByName(string name)
        {
            User user = _userQuery.getByName(name).SingleOrDefault();
            return user == null ? 0 : user.Id;
        }

        public string GetPassword(string name)
        {
            User user = _userQuery.getByName(name).SingleOrDefault();
            return user.Password;
        }

        public int Do(RegisterModel model)
        {
            User user = new User
            {
                Name = model.UserName,
                Password = model.Password.Md5Encypt(),
                AuthenticationCode = RandomGenerator.GetNumbers(6)
            };
            session.Save(user);

            return user.Id;
        }
    }
}
