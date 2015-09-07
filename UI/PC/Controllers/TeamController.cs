using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FFLTask.SRV.ViewModel.Team;
using FFLTask.SRV.ServiceInterface;
using FFLTask.GLB.Global.Enum;

namespace FFLTask.UI.PC.Controllers
{
    public class TeamController : BaseController
    {
        private IRegisterService _registerService;
        private ITeamService _teamService;
        private ITaskService _taskService;
        private IUserService _userService;
        private IAuthroizationService _authService;
        public TeamController(IRegisterService registerService,
            ITeamService teamService,
            ITaskService taskService,
            IUserService userService,
            IAuthroizationService authService)
        {
            _registerService = registerService;
            _teamService = teamService;
            _taskService = taskService;
            _userService = userService;
            _authService = authService;
        }

        #region URL: /Team/Search

        public ActionResult Search()
        {
            if (TempData["SearchResult"] == null)
            {
                return View(new SearchModel());
            }

            return View((SearchModel)TempData["SearchResult"]);
        }

        [HttpPost]
        public ActionResult Search(SearchModel model)
        {
            if (model.TransferResult != null)
            {

            }
            else if (model.DismissResult != null)
            {
                IList<int> projectIds = model.DismissResult.Items
                    .Where(i => i.Selected)
                    .Select(i => i.ProjectId)
                    .ToList();
                _authService.Dismiss(model.UserId, projectIds);
            }
            else
            {
                if (!ModelState.IsValid)
                {
                    return View(model);
                }

                int userId = _registerService.GetUserByName(model.UserName);

                if (userId == 0)
                {
                    ModelState.AddModelError("UserName", "* 用户名不存在");
                    return View(model);
                }

                if (model.ResultFor == ResultFor.Transfer)
                {
                    model = _teamService.GroupedByRole(userId);
                }
                else if (model.ResultFor == ResultFor.Dismiss)
                {
                    model.DismissResult = new DismissSearchResultModel
                    {
                        Items = _teamService.GroupedByProject(userId)
                    };
                }

                model.UserId = userId;
                TempData["SearchResult"] = model;
            }

            return RedirectToAction("Search");
        }

        #endregion

        #region URL: /Team/Transfer

        public ActionResult Transfer(int userId, Role role, int projectId, Status? status)
        {
            TransferModel model = new TransferModel
            {
                LeaverId = userId,
                ProjectId = projectId,
                CurrentRole = role,
                SelectedStatus = status
            };

            model.Predecessor = _userService.GetUser(userId);

            model.Tasks = _teamService.GetTasks(model);
            model.AllStatus = _teamService.GetAllStatus(model);

            return View(model);
        }

        [HttpPost]
        public ActionResult Transfer(TransferModel model, Role role)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            int succesorId = _registerService.GetUserByName(model.SuccessorName);

            if (succesorId == 0)
            {
                ModelState.AddModelError("SuccessorName", "* 用户名不存在");
                return View(model);
            }

            foreach (var task in model.Tasks)
            {
                if (task.Selected)
                {
                    _teamService.HandOver(task, role, succesorId, userHelper.CurrentUserId.Value);
                }
            }

            return RedirectToAction("Transfer");
        }

        #endregion
    }
}
