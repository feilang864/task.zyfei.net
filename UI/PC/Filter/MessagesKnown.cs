//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Web;
//using System.Web.Mvc;
//using FFLTask.SRV.ViewModel.Message;
//using PC;

//namespace FFLTask.UI.PC.Filter
//{
//    public class MessagesKnown : ActionFilterAttribute
//    {
//        public override void OnResultExecuting(ResultExecutingContext filterContext)
//        {
//            HistoryModel model = filterContext.Controller.ViewData.Model as HistoryModel;
//            if (model.Messages != null)
//            {
//                //TODO: decouple UI from BLL
//                //var _messageService = new MessageService();
//                //_messageService.Load(model.Messages[0].Id).Subscriber.ReadAllMessages();
//            }

//            base.OnResultExecuting(filterContext);
//        }
//    }
//}