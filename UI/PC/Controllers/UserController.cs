using System.Web.Mvc;
using FFLTask.SRV.ServiceInterface;
using FFLTask.UI.PC.Filter;
using FFLTask.SRV.ViewModel.User;

namespace FFLTask.UI.PC.Controllers
{
    public class UserController : BaseController
    {
        private IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        #region URL: /User/Summary/{id}

        public ActionResult Summary(int? id)
        {
            return View();
        }

        #endregion

        #region URL: /User/Profile

        [NeedAuthorized]
        public new ActionResult Profile()
        {
            ProfileModel model = new ProfileModel();
            return View(model);
        }

        //[HttpPost]
        //[NeedAuthorized]
        //public ActionResult Profile()
        //{
        //    return View();
        //}

        #endregion


        #region Ajax

        public JsonResult HasUnknownMessage()
        {
            if (!userHelper.CurrentUserId.HasValue)
            {
                return Json(false);
            }

            return Json(_userService.HasUnknownMessage(userHelper.CurrentUserId.Value));
        }

        #endregion
    }
}
