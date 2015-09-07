using System;
using System.Linq;
using FFLTask.BLL.Entity;
using FFLTask.GLB.Global.Enum;
using FFLTask.SRV.Query;
using Global.Core.ExtensionMethod;
using NHibernate.Linq;
using NUnit.Framework;

namespace FFLTask.SRV.QueryTest
{
    [TestFixture]
    public class MessageQueryTest : BaseQueryTest
    {
        [Test]
        public void Project()
        {
            Project project_1 = new Project();
            Project project_2 = new Project();

            Message message_1 = create_message(project_1);
            Message message_2 = create_message(project_2);

            var result_project_id_not_value = session.Query<Message>().Project(null).ToList();
            Assert.That(result_project_id_not_value.Count, Is.EqualTo(2));
            Contains(result_project_id_not_value, message_1);
            Contains(result_project_id_not_value, message_2);

            var result_project_1 = session.Query<Message>().Project(project_1.Id).ToList();
            Assert.That(result_project_1.Count, Is.EqualTo(1));
            Assert.That(result_project_1.Single().Id, Is.EqualTo(message_1.Id));

            var result_project_2 = session.Query<Message>().Project(project_2.Id).ToList();
            Assert.That(result_project_2.Count, Is.EqualTo(1));
            Assert.That(result_project_2.Single().Id, Is.EqualTo(message_2.Id));
        }

        [Test]
        public void Sort()
        {
            Message publish_2015_01_01_000000_read_2015_01_01_235959 = create_message(new DateTime(2015, 1, 1), new DateTime(2015, 1, 1, 23, 59, 59));
            Message publish_2015_01_01_000001_read_2015_02_28_235959 = create_message(new DateTime(2015, 1, 1, 0, 0, 1), new DateTime(2015, 2, 28, 23, 59, 59));
            Message publish_2015_01_31_000000_not_read = create_message(new DateTime(2015, 1, 31), null);

            var result_sort_publish_time_false = session.Query<Message>().Sort(MessageSort.PublishTime, false).ToList();
            Assert.That(result_sort_publish_time_false.Count, Is.EqualTo(3));
            Assert.That(result_sort_publish_time_false[0].Id, Is.EqualTo(publish_2015_01_01_000000_read_2015_01_01_235959.Id));
            Assert.That(result_sort_publish_time_false[1].Id, Is.EqualTo(publish_2015_01_01_000001_read_2015_02_28_235959.Id));
            Assert.That(result_sort_publish_time_false[2].Id, Is.EqualTo(publish_2015_01_31_000000_not_read.Id));

            var result_sort_publish_time_true = session.Query<Message>().Sort(MessageSort.PublishTime, true).ToList();
            Assert.That(result_sort_publish_time_true.Count, Is.EqualTo(3));
            Assert.That(result_sort_publish_time_true[0].Id, Is.EqualTo(publish_2015_01_31_000000_not_read.Id));
            Assert.That(result_sort_publish_time_true[1].Id, Is.EqualTo(publish_2015_01_01_000001_read_2015_02_28_235959.Id));
            Assert.That(result_sort_publish_time_true[2].Id, Is.EqualTo(publish_2015_01_01_000000_read_2015_01_01_235959.Id));

            var result_sort_read_time_true = session.Query<Message>().Sort(MessageSort.ReadTime, false).ToList();
            Assert.That(result_sort_read_time_true.Count, Is.EqualTo(3));
            Assert.That(result_sort_read_time_true[0].Id, Is.EqualTo(publish_2015_01_31_000000_not_read.Id));
            Assert.That(result_sort_read_time_true[1].Id, Is.EqualTo(publish_2015_01_01_000000_read_2015_01_01_235959.Id));
            Assert.That(result_sort_read_time_true[2].Id, Is.EqualTo(publish_2015_01_01_000001_read_2015_02_28_235959.Id));

            var result_sort_read_time_false = session.Query<Message>().Sort(MessageSort.ReadTime, true).ToList();
            Assert.That(result_sort_read_time_false.Count, Is.EqualTo(3));
            Assert.That(result_sort_read_time_false[0].Id, Is.EqualTo(publish_2015_01_01_000001_read_2015_02_28_235959.Id));
            Assert.That(result_sort_read_time_false[1].Id, Is.EqualTo(publish_2015_01_01_000000_read_2015_01_01_235959.Id));
            Assert.That(result_sort_read_time_false[2].Id, Is.EqualTo(publish_2015_01_31_000000_not_read.Id));
        }

        [Test]
        public void CanShow()
        {
            Message no_hide = create_message(false, false);
            Message hide_for_addresser = create_message(true, false);
            Message hide_for_addressee = create_message(false, true);
            Message hide_for_addresser_and_addressee = create_message(true, true);

            var result_can_show_for_Addresser = session.Query<Message>().CanShow(MessageFor.Addresser).ToList();
            Assert.That(result_can_show_for_Addresser.Count, Is.EqualTo(2));
            Contains(result_can_show_for_Addresser, no_hide);
            Contains(result_can_show_for_Addresser, hide_for_addressee);

            var result_can_show_for_Addressee = session.Query<Message>().CanShow(MessageFor.Addressee).ToList();
            Assert.That(result_can_show_for_Addressee.Count, Is.EqualTo(2));
            Contains(result_can_show_for_Addressee, no_hide);
            Contains(result_can_show_for_Addressee, hide_for_addresser);
        }

        [Test]
        public void From()
        {
            User addresser_1 = new User();
            User addresser_2 = new User();

            Message message_1 = create_message(addresser_1, null);
            Message message_2 = create_message(addresser_2, null);

            var result_user_id_not_value = session.Query<Message>().From(null).ToList();
            Assert.That(result_user_id_not_value.Count, Is.EqualTo(2));
            Contains(result_user_id_not_value, message_1);
            Contains(result_user_id_not_value, message_2);

            var result_addresser_1 = session.Query<Message>().From(addresser_1.Id).ToList();
            Assert.That(result_addresser_1.Count, Is.EqualTo(1));
            Assert.That(result_addresser_1.Single().Id, Is.EqualTo(message_1.Id));

            var result_addresser_2 = session.Query<Message>().From(addresser_2.Id).ToList();
            Assert.That(result_addresser_2.Count, Is.EqualTo(1));
            Assert.That(result_addresser_2.Single().Id, Is.EqualTo(message_2.Id));
        }

        [Test]
        public void To()
        {
            User addresser = new User();
            User addressee_1 = new User();
            User addressee_2 = new User();

            Message message_1 = create_message(addresser, addressee_1);
            Message message_2 = create_message(addresser, addressee_2);

            var result_user_id_not_value = session.Query<Message>().To(null).ToList();
            Assert.That(result_user_id_not_value.Count, Is.EqualTo(2));
            Contains(result_user_id_not_value, message_1);
            Contains(result_user_id_not_value, message_2);

            var result_addressee_1 = session.Query<Message>().To(addressee_1.Id).ToList();
            Assert.That(result_addressee_1.Count, Is.EqualTo(1));
            Assert.That(result_addressee_1.Single().Id, Is.EqualTo(message_1.Id));

            var result_addressee_2 = session.Query<Message>().To(addressee_2.Id).ToList();
            Assert.That(result_addressee_2.Count, Is.EqualTo(1));
            Assert.That(result_addressee_2.Single().Id, Is.EqualTo(message_2.Id));
        }

        [Test]
        public void Read()
        {
            Message message_read_1 = new Message();
            message_read_1.SetPrivateField("_readTime", new DateTime(2014, 1, 1));
            Message message_read_2 = new Message();
            message_read_2.SetPrivateField("_readTime", new DateTime(2015, 11, 12));

            Message message_not_read_1 = new Message();
            Message message_not_read_2 = new Message();

            Save(message_read_1,
                message_read_2,
                message_not_read_1,
                message_not_read_2);

            var result_read = session.Query<Message>().HasRead().ToList();
            Assert.That(result_read.Count, Is.EqualTo(2));
            Contains(result_read, message_read_1);
            Contains(result_read, message_read_2);

            var result_not_read = session.Query<Message>().NotRead().ToList();
            Assert.That(result_not_read.Count, Is.EqualTo(2));
            Contains(result_not_read, message_not_read_1);
            Contains(result_not_read, message_not_read_2);
        }

        private Message create_message(bool hideForAddresser, bool hideForAddressee)
        {
            Message message = new Message();
            message.SetPrivateField("_hideForAddresser", hideForAddresser);
            message.SetPrivateField("_hideForAddressee", hideForAddressee);

            Save(message);

            return message;
        }

        private Message create_message(User addresser, User addressee)
        {
            Message message = new Message
            {
                Addresser = addresser,
                Addressee = addressee
            };
            Save(message);

            return message;
        }

        private Message create_message(Project project)
        {
            Message message = new Message
            {
                Project = project,
            };
            Save(message);

            return message;
        }

        private Message create_message(DateTime publishTime, DateTime? readTime)
        {
            Message message = new Message();
            message.SetPropertyInBase("CreateTime", publishTime);
            message.SetPrivateField("_readTime", readTime);
            Save(message);
            return message;
        }
    }
}
