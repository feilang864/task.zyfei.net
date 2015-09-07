using Global.Core.ExtensionMethod;
using FFLTask.SRV.ServiceInterface;
using FFLTask.SRV.ViewModel.Account;
using FFLTask.SRV.ViewModel.Test;

namespace FFLTask.SRV.UIDevService
{
    public class RegisterService : IRegisterService
    {
        public int GetUserByName(string name)
        {
            int userId = 0;
            switch (name)
            {
                case "自由飞":
                    userId = (int)FakeUsers.自由飞;
                    break;
            }
            return userId;
        }

        public string GetPassword(string name)
        {
            return "1234".Md5Encypt();
        }


        public int Do(RegisterModel model)
        {
            return -1;
        }
    }
}
