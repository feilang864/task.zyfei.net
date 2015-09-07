using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FFLTask.UI.PC.Filter;
using FFLTask.SRV.ServiceInterface;
using FFLTask.SRV.ViewModel.Auth;

namespace FFLTask.UI.PC.Controllers
{
    public class AuthController : BaseController
    {
        #region AuthController

        private IAuthroizationService _authService;
        public AuthController(IAuthroizationService authService)
        {
            _authService = authService;
        }

        #endregion

        #region /Auth/Grant

        //[NeedAuthorized(Privilege.Admin)]
        public ActionResult Grant()
        {
            IList<ProjectAuthorizationModel> model = _authService.GetAdmined(userHelper.CurrentUserId.Value);
            return View(model);
        }

        [HttpPost]
        public ActionResult Grant(IList<ProjectAuthorizationModel> model)
        {
            foreach (var group in model)
            {
                foreach (var auth in group.Authorizations)
                {
                    if (auth.IsEdit)
                    {
                        _authService.Update(auth);
                    }
                }
            }
            return RedirectToAction("Grant");
        }

        #endregion
    }
}
