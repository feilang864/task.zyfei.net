using System.Web.Mvc;
using FFLTask.SRV.ServiceInterface;

namespace FFLTask.UI.PC.Controllers
{
    public class UserController : BaseController
    {
        private IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        public ActionResult Summary(int? id)
        {
            return View();
        }

        public JsonResult HasUnknownMessage()
        {
            if (!userHelper.CurrentUserId.HasValue)
            {
                return Json(false);
            }

            return Json(_userService.HasUnknownMessage(userHelper.CurrentUserId.Value));
        }
    }
}
