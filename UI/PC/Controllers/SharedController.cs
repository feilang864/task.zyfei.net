using System.Web.Mvc;
using FFLTask.SRV.ServiceInterface;
using FFLTask.SRV.ViewModel.Account;
using FFLTask.SRV.ViewModel.Shared;
using FFLTask.UI.PC.WebHelper;

namespace FFLTask.UI.PC.Controllers
{
    public class SharedController : BaseController
    {
        private IUserService _userService;

        public SharedController(IUserService userService)
        {
            _userService = userService;
        }

        #region Image Code

        public FileContentResult GetImageCode()
        {
            string code = ImageCodeHelper.CreateValidateCode(4);
            Session[SessionKey.SESSION_IMAGE_CODE] = code;
            byte[] bytes = ImageCodeHelper.CreateValidateGraphic(code);
            return File(bytes, @"image/jpeg");
        }

        [ChildActionOnly]
        /// <summary>
        /// This method is necessary, so the model is new, 
        /// then the imagecode's validation result will not show when the page is render the first time
        /// </summary>
        /// <returns></returns>
        public PartialViewResult _ImageCode()
        {
            return PartialView(new ImageCodeModel());
        }

        [HttpPost]
        [ChildActionOnly]
        public PartialViewResult _ImageCode(ImageCodeModel model)
        {
            if (!isCookieEnabled())
            {
                ModelState.AddModelError("InputImageCode", string.Format("* 检查不到{0}，请确保cookie已启用", "<a href='#'>cookie</a>"));
            }
            if (model.ImageCodeError == ImageCodeError.Expired)
            {
                ModelState.AddModelError("InputImageCode", "* 验证码已过期，请重新输入");
            }
            else if (model.ImageCodeError == ImageCodeError.Wrong)
            {
                ModelState.AddModelError("InputImageCode", "* 验证码错误，请重新输入");
            }

            imageCodeHelper.ClearImageCode();

            return PartialView(model);
        }

        private bool isCookieEnabled()
        {
            return Request.Cookies[CookieKey.CheckCookieEnable] != null;
        }

        #endregion

        [ChildActionOnly]
        public PartialViewResult _LoginStatus()
        {
            ViewBag.PrepageUrl = prepageUrlHelper.SetPrepageForUrl();
            return PartialView(userHelper.CurrentUser);
        }

        [ChildActionOnly]
        public ActionResult _Navigator()
        {
            _NavigatorModel model = new _NavigatorModel();
            model.CurrentUser = userHelper.CurrentUser;

            if (model.CurrentUser.Id.HasValue)
            {
                model.HasUnknownMessage = _userService.HasUnknownMessage(model.CurrentUser.Id.Value);
            }

            return View(model);
        }

        public PartialViewResult _Pager(PagerModel model)
        {
            #region format the url

            string path = Request.Url.LocalPath;
            int start = path.LastIndexOf("Page-");
            string pathPrefix = start > -1 ? path.Substring(0, start) : path;

            if (!pathPrefix.EndsWith("/"))
            {
                pathPrefix += "/";
            }

            model.FormatUrl = pathPrefix + "Page-{0}" + Request.Url.Query;

            #endregion

            return PartialView(model);
        }
    }
}
