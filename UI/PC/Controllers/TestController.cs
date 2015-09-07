using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FFLTask.SRV.ViewModel.Account;
using FFLTask.SRV.ViewModel.Test;

namespace FFLTask.UI.PC.Controllers
{
    public class TestController : BaseController
    {
        public ActionResult UserLogon()
        {
            UserLogonModel model = new UserLogonModel();
            return View(model);
        }

        [HttpPost]
        public ActionResult UserLogon(UserLogonModel model)
        {
            string userId = ((int)model.SelectedUserId).ToString();
            if (model.Remember)
            {
                userHelper.SetUserId(userId, 14);
            }
            else
            {
                userHelper.SetUserId(userId);
            }

            return View(model);
        }
    }
}
