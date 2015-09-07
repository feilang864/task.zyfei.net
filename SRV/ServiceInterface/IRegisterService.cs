using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FFLTask.SRV.ViewModel.Account;

namespace FFLTask.SRV.ServiceInterface
{
    public interface IRegisterService
    {
        /// <param name="name">user's name when registering</param>
        /// <returns>the user's id. if there is no such user, return 0</returns>
        int GetUserByName(string name);

        /// <summary>
        /// assume that the user with such name is exist.
        /// </summary>
        string GetPassword(string name);

        /// <returns>the registered user's id</returns>
        int Do(FFLTask.SRV.ViewModel.Account.RegisterModel model);
    }
}
