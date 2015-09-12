using System.Web.Mvc;
using FFLTask.SRV.ServiceInterface;
using FFLTask.UI.PC.Filter;
using FFLTask.SRV.ViewModel.User;
using FFLTask.GLB.Global;
using System.Collections.Generic;

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

        public ActionResult Summary(int? userId)
        {
            SummaryModel model = new SummaryModel();

            model.IsCurrentUser = (userId == userHelper.CurrentUserId);

            model.Profile = _userService.GetProfile(userId.Value);
            model.JoinedProjects = _userService.GetJoinedProjects(userId.Value);

            return View(model);
        }

        #endregion

        #region URL: /User/Profile

        [NeedAuthorized]
        public new ActionResult Profile()
        {
            ProfileModel model = _userService.GetProfile(userHelper.CurrentUserId.Value);
            return View(model);
        }

        [HttpPost]
        [NeedAuthorized]
        public new ActionResult Profile(string button, ProfileModel model)
        {
            if (button != "跳过")
            {
                if (!ModelState.IsValid)
                {
                    return View(model);
                }

                model = nullProvinceAndCity(model);

                _userService.SaveProfile(model, userHelper.CurrentUserId.Value);
            }

            if (model.BuildProject)
            {
                return RedirectToAction("Create", "Project");
            }
            else
            {
                return RedirectToAction("Search", "Project");
            }
        }

        private ProfileModel nullProvinceAndCity(ProfileModel model)
        {
            model.Province = model.Province == "------" ?
                null :
                AddressHelper.GetLiteral(model.Province);

            if (model.City == "--------")
            {
                model.City = null;
            }

            return model;
        }

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
