using System;
using FFLTask.BLL.Entity;
using NUnit.Framework;
using Global.Core.ExtensionMethod;

namespace FFLTask.BLL.NHMapTest
{
    [TestFixture]
    public class MessageMapTest : BaseMapTest<Message>
    {
        [Test]
        public void Normal()
        {
            string content = "Resource/Images/Attachment/MVC框架揭秘";
            DateTime read_time = new DateTime(2015, 1, 1);

            Message message = new Message
            {
                Content = content,
                Task = new Task(),
                Project = new Project(),
                Addresser = new User(),
                Addressee = new User()
            };
            message.SetPrivateField("_readTime", read_time);
            message.SetPrivateField("_hideForAddresser", true);
            message.SetPrivateField("_hideForAddressee", true);

            Message load_message = Save(message);

            DBAssert.AreInserted(load_message);
            Assert.That(load_message.Content, Is.EqualTo(content));
            DBAssert.AreInserted(load_message.Task);
            DBAssert.AreInserted(load_message.Project);
            DBAssert.AreInserted(load_message.Addresser);
            DBAssert.AreInserted(load_message.Addressee);
            Assert.That(load_message.ReadTime, Is.EqualTo(read_time));
            Assert.That(load_message.HideForAddresser, Is.True);
            Assert.That(load_message.HideForAddressee, Is.True);
        }
    }
}
