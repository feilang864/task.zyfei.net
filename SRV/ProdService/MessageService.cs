using System.Collections.Generic;
using System.Linq;
using FFLTask.BLL.Entity;
using FFLTask.GLB.Global.Enum;
using FFLTask.SRV.Query;
using FFLTask.SRV.ServiceInterface;
using FFLTask.SRV.ViewModel.Account;
using FFLTask.SRV.ViewModel.Message;
using FFLTask.SRV.ViewModel.Project;
using FFLTask.SRV.ViewModel.Shared;
using FFLTask.SRV.ViewModelMap;
using NHibernate.Linq;

namespace FFLTask.SRV.ProdService
{
    public class MessageService : BaseService, IMessageService
    {
        private IQueryable<Message> _querySource;
        public MessageService()
        {
            _querySource = session.Query<Message>();
        }

        public ToMeModel GetToMe(int userId,
            MessageSort sortedBy, bool des,
            PagerModel pager,
            int? projectId = null, int? addresserId = null)
        {
            ToMeModel model = new ToMeModel();

            model.Addressers = getCommunicators(userId, MessageFor.Addressee);
            model.Projects = getProjects(userId, MessageFor.Addressee);

            var messages = _querySource.Get(addresserId, userId, projectId, MessageFor.Addressee)
                .Sort(sortedBy, des)
                .Paged(pager)
                .ToList();

            model.Messages = new List<ToMeItemModel>();
            model.Messages.FilledBy(messages);

            return model;
        }

        public FromMeModel GetFromMe(int userId,
            MessageSort sortedBy, bool des,
            PagerModel pager,
            int? projectId = null, int? addresseeId = null)
        {
            FromMeModel model = new FromMeModel();

            model.Addressees = getCommunicators(userId, MessageFor.Addresser);
            model.Projects = getProjects(userId, MessageFor.Addresser);

            var messages = _querySource.Get(userId, addresseeId, projectId, MessageFor.Addresser)
                .Sort(sortedBy, des)
                .Paged(pager)
                .ToList();

            model.Messages = new List<FromMeItemModel>();
            model.Messages.FilledBy(messages);

            return model;
        }

        public int GetToMeCount(int userId, int? projectId, int? addresserId)
        {
            return session.Query<Message>()
                .Get(addresserId, userId, projectId, MessageFor.Addressee)
                .Count();
        }

        public int GetFromMeCount(int userId, int? projectId, int? addresseeId)
        {
            return session.Query<Message>()
                .Get(userId, addresseeId, projectId, MessageFor.Addresser)
                .Count();
        }

        public void DeleteForAddresser(int messageId)
        {
            Message message = session.Load<Message>(messageId);
            message.Hide(MessageFor.Addresser);
        }

        public void DeleteForAddressee(int messageId)
        {
            Message message = session.Load<Message>(messageId);
            if (!message.ReadTime.HasValue)
            {
                message.Read();
            }
            message.Hide(MessageFor.Addressee);
        }

        public void Read(int messageId)
        {
            Message message = session.Load<Message>(messageId);
            if (!message.ReadTime.HasValue)
            {
                message.Read();
            }
        }

        private IList<_LiteralLinkedModel> getProjects(int? userId, MessageFor direction)
        {
            IList<_LiteralLinkedModel> models = new List<_LiteralLinkedModel>();

            var messages = _querySource.CanShow(direction); ;
            if (direction == MessageFor.Addresser)
            {
                messages = messages.From(userId);
            }
            else if (direction == MessageFor.Addressee)
            {
                messages = messages.To(userId);
            }
            var projects = messages
                .Select(m => m.Project)
                .Distinct()
                .ToList();

            models.FilledBy(projects);

            return models;
        }

        private IList<UserModel> getCommunicators(int? userId, MessageFor direction)
        {
            IList<UserModel> models = new List<UserModel>();

            IList<User> users = getRawCommunicators(userId, direction);
            models.FilledBy(users);

            return models;
        }

        private IList<User> getRawCommunicators(int? userId, MessageFor direction)
        {
            var result = _querySource.CanShow(direction);
            if (direction == MessageFor.Addresser)
            {
                return result
                    .From(userId)
                    .Select(m => m.Addressee)
                    .Distinct().ToList();
            }
            else
            {
                return result
                    .To(userId)
                    .Select(m => m.Addresser)
                    .Distinct().ToList();
            }
        }
    }
}
