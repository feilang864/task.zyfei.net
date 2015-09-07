using System.Web.Mvc;
using FFLTask.SRV.ServiceInterface;
using FFLTask.UI.PC.Filter;
using FFLTask.SRV.ViewModel.Account;
using FFLTask.SRV.ViewModel.Shared;
using Global.Core.ExtensionMethod;

namespace FFLTask.UI.PC.Controllers
{
    public class LogController : BaseController
    {
        private IRegisterService _registerService;
        private IUserService _userService;
        public LogController(IRegisterService registerService, IUserService userService)
        {
            _registerService = registerService;
            _userService = userService;
        }

        #region URL: /Log/On

        [CheckCookieEnabled]
        public ActionResult On()
        {
            return View(new LogonModel());
        }

        [HttpPost]
        public ActionResult On(LogonModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            model.ImageCode = imageCodeHelper.CheckResult();
            if (model.ImageCode.ImageCodeError != ImageCodeError.NoError)
            {
                return View(model);
            }

            int userId = _registerService.GetUserByName(model.UserName);
            if (userId == 0)
            {
                ModelState.AddModelError("UserName", "* 用户名不存在");
                return View(model);
            }

            string password = _registerService.GetPassword(model.UserName);
            if (model.Password.Md5Encypt() != password)
            {
                ModelState.AddModelError("Password", "* 密码（或用户名）错误");
                return View(model);
            }

            if (model.RememberMe)
            {
                userHelper.SetUserId(userId.ToString(), CookieSpan.DayForRememberMe);
            }
            else
            {
                userHelper.SetUserId(userId.ToString());
            }

            return prepageUrlHelper.ReturnPrePage("/Task/List");
        }


        #endregion

        #region URL: /Log/Off

        public ActionResult Off()
        {
            userHelper.LogOff();
            return prepageUrlHelper.ReturnPrePage();
        }

        #endregion
    }
}
