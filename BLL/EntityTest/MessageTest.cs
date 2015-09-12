using System;
using FFLTask.BLL.Entity;
using FFLTask.GLB.Global.Enum;
using Global.Core.ExtensionMethod;
using Global.Core.Helper;
using NUnit.Framework;

namespace FFLTask.BLL.EntityTest
{
    [TestFixture]
    public class MessageTest
    {
        [Test]
        public void Not_Send()
        {
            Message message_empty = new Message
            {
                Addresser = null,
                Addressee = null
            };

            message_empty.Send();             //no exception is threw out

            User addresser = new User();            
            Message message_no_addressee = new Message
            {
                Addresser = addresser,
                Addressee = null
            };

            message_no_addressee.Send();

            //need not record into MessageFromMe since no addressee
            Assert.That(addresser.MessagesFromMe.Count, Is.EqualTo(1));

            
            User addressee = new User();
            Message message_no_addresser = new Message
            {
                Addresser = null,
                Addressee = addressee
            };
            Assert.That(addressee.MessagesToMe.IsNullOrEmpty(), Is.True);

            message_no_addresser.Send();

            //only record in MessagesToMe
            Assert.That(addressee.MessagesToMe.Count, Is.EqualTo(1));              
        }

        [Test]
        public void Send()
        {
            User addresser = new User();
            User addressee = new User();

            Message message = new Message
            {
                Addresser = addresser,
                Addressee = addressee
            };

            Assert.That(addresser.MessagesFromMe.IsNullOrEmpty(), Is.True);
            Assert.That(addressee.MessagesToMe.IsNullOrEmpty(), Is.True);

            message.Send();

            Assert.That(addresser.MessagesFromMe.Count, Is.EqualTo(1));
            Assert.That(addressee.MessagesToMe.Count, Is.EqualTo(1));
        }

        [Test]
        public void Read()
        {
            Message message = new Message();

            Assert.That(message.ReadTime, Is.Null);

            DateTime read_time = new DateTime(2015, 2, 14, 12, 12, 12);
            SystemTime.SetDateTime(read_time);
            message.Read();
            SystemTime.ResetDateTime();

            Assert.That(message.ReadTime, Is.EqualTo(read_time));
        }

        [Test]
        public void Hide()
        {
            User addresser = new User();
            User addressee = new User();
            Message message = new Message
            {
                Addresser = addresser,
                Addressee = addressee
            };
            message.Send();

            Assert.That(message.HideForAddresser, Is.False);
            Assert.That(message.HideForAddressee, Is.False);

            message.Hide(MessageFor.Addresser);

            Assert.That(message.HideForAddresser, Is.True);
            Assert.That(message.HideForAddressee, Is.False);

            message.Hide(MessageFor.Addressee);

            Assert.That(message.HideForAddresser, Is.True);
            Assert.That(message.HideForAddressee, Is.True);
        }
    }
}
