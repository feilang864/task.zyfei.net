using System;
using System.Collections.Generic;
using System.Web.Mvc;
using FFLTask.GLB.Global.Enum;
using FFLTask.SRV.ServiceInterface;
using FFLTask.SRV.ViewModel.Message;
using FFLTask.SRV.ViewModel.Shared;
using FFLTask.UI.PC.Filter;

namespace FFLTask.UI.PC.Controllers
{
    [NeedAuthorized]
    public class MessageController : BaseController
    {
        private string _selectedProjectId = "SelectedProjectId";
        private string _selectedAddresseeId = "SelectedAddresseeId";
        private string _selectedAddresserId = "SelectedAddresserId";

        private IMessageService _messageService;
        public MessageController(IMessageService messageService)
        {
            _messageService = messageService;
        }

        public ActionResult FromMe(int pageIndex = 1, MessageSort sort = MessageSort.PublishTime, bool des = true)
        {
            int? projectId = (int?)TempData[_selectedProjectId];
            int? addresseeId = (int?)TempData[_selectedAddresseeId];

            PagerModel pager = new PagerModel
            {
                PageSize = 20,
                RowSize = 20,
                PageIndex = pageIndex
            };
            FromMeModel model = _messageService.GetFromMe(userHelper.CurrentUserId.Value, sort, des, pager, projectId, addresseeId);
            model.SelectedProjectId = projectId;
            model.SelectedAddresseeId = addresseeId;

            model.Pager = pager;
            model.Pager.SumOfItems = _messageService.GetFromMeCount(userHelper.CurrentUserId.Value, projectId, addresseeId);

            return View(model);
        }

        [HttpPost]
        public ActionResult FromMe(FromMeModel model)
        {
            model.Messages = model.Messages ?? new List<FromMeItemModel>();
            foreach (var item in model.Messages)
            {
                if (item.Checked)
                {
                    _messageService.DeleteForAddresser(item.Id);
                }
            }

            TempData[_selectedProjectId] = model.SelectedProjectId;
            TempData[_selectedAddresseeId] = model.SelectedAddresseeId;

            return RedirectToAction("FromMe", new { pageIndex = 1 });
        }

        public ActionResult ToMe(int pageIndex = 1, MessageSort sort = MessageSort.PublishTime, bool des = true)
        {
            int? projectId = (int?)TempData[_selectedProjectId];
            int? addresseeId = (int?)TempData[_selectedAddresserId];

            PagerModel pager = new PagerModel
            {
                PageSize = 20,
                RowSize = 20,
                PageIndex = pageIndex
            };
            ToMeModel model = _messageService.GetToMe(userHelper.CurrentUserId.Value, sort, des, pager, projectId, addresseeId);
            model.SelectedProjectId = projectId;
            model.SelectedAddresserId = addresseeId;

            model.Pager = pager;
            model.Pager.SumOfItems = _messageService.GetToMeCount(userHelper.CurrentUserId.Value, projectId, addresseeId);

            return View(model);
        }

        [HttpPost]
        public ActionResult ToMe(ToMeModel model)
        {
            model.Messages = model.Messages ?? new List<ToMeItemModel>();
            switch (model.MessageMark)
            {
                case MessageMark.Read:
                    markMessages(_messageService.Read, model.Messages);
                    break;
                case MessageMark.Delete:
                    markMessages(_messageService.DeleteForAddressee, model.Messages);
                    break;
            }

            TempData[_selectedProjectId] = model.SelectedProjectId;
            TempData[_selectedAddresserId] = model.SelectedAddresserId;

            return RedirectToAction("ToMe", new { pageIndex = 1 });
        }

        private void markMessages<T>(Action<int> action, T messages)
            where T : IList<ToMeItemModel>
        {
            foreach (var item in messages)
            {
                if (item.Checked)
                {
                    action(item.Id);
                }
            }
        }
    }
}
