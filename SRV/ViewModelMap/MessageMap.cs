using System.Collections.Generic;
using FFLTask.BLL.Entity;
using FFLTask.SRV.ViewModel.Account;
using FFLTask.SRV.ViewModel.Message;
using FFLTask.SRV.ViewModel.Project;

namespace FFLTask.SRV.ViewModelMap
{
    public static class MessageMap
    {
        public static void FilledBy(this FromMeItemModel model, Message message)
        {
            model.Id = message.Id;
            model.Content = message.Content;
            model.PublishTime = message.CreateTime;

            model.Task = new ViewModel.Task.LiteItemModel();
            model.Task.FilledBy(message.Task);
            model.Project = new _LiteralLinkedModel();
            model.Project.Filledby(message.Project);

            model.Addressee = new UserModel();
            model.Addressee.FilledBy(message.Addressee);

            model.ReadTime = message.ReadTime;
        }

        public static void FilledBy(this ToMeItemModel model, Message message)
        {
            model.Id = message.Id;
            model.Content = message.Content;
            model.PublishTime = message.CreateTime;

            model.Task = new ViewModel.Task.LiteItemModel();
            model.Task.FilledBy(message.Task);
            model.Project = new _LiteralLinkedModel();
            model.Project.Filledby(message.Project);

            model.Addresser = new UserModel();
            model.Addresser.FilledBy(message.Addresser);

            model.ReadTime = message.ReadTime;
        }

        public static void FilledBy(this IList<FromMeItemModel> models, IList<Message> messages)
        {
            foreach (var message in messages)
            {
                FromMeItemModel item = new FromMeItemModel();
                item.FilledBy(message);
                models.Add(item);
            }
        }

        public static void FilledBy(this IList<ToMeItemModel> models, IList<Message> messages)
        {
            foreach (var message in messages)
            {
                ToMeItemModel item = new ToMeItemModel();
                item.FilledBy(message);
                models.Add(item);
            }
        }
    }
}
