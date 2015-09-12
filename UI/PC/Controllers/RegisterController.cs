using System.Web.Mvc;
using FFLTask.SRV.ServiceInterface;
using FFLTask.UI.PC.Filter;
using FFLTask.SRV.ViewModel.Account;
using FFLTask.SRV.ViewModel.Shared;

namespace FFLTask.UI.PC.Controllers
{
    public class RegisterController : BaseController
    {
        private IRegisterService _registerService;
        public RegisterController(IRegisterService registerService)
        {
            _registerService = registerService;
        }

        #region URL: /Register

        [HttpGet]
        [CheckCookieEnabled]
        public ActionResult Index()
        {

            return View(new RegisterModel());
        }

        [HttpPost]
        public ActionResult Index(RegisterModel model)
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

            if (_registerService.GetUserByName(model.UserName) > 0)
            {
                ModelState.AddModelError("UserName", "*用户名已被使用");
                return View(model);
            }

            int userId = _registerService.Do(model);
            userHelper.SetUserId(userId.ToString());

            return RedirectToAction("Profile", "User");

        }

        #endregion

        #region Ajax
        public JsonResult IsUserNameExist(string name)
        {
            bool duplicated = _registerService.GetUserByName(name) > 0;
            return Json(duplicated);
        }
        #endregion
    }
}
