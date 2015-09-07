using FFLTask.BLL.Entity;
using NUnit.Framework;

namespace FFLTask.BLL.NHMapTest
{
    [TestFixture]
    public class AttachmentMapTest : BaseMapTest<Attachment>
    {
        [Test]
        public void Normal()
        {
            string filename = "Resource/Images/Attachment/MVC框架揭秘";

            Attachment attachment = new Attachment
            {
                FileName = filename,
                Task = new Task(),
                Uploader = new User()
            };

            Attachment load_attachment = Save(attachment);

            DBAssert.AreInserted(load_attachment);
            DBAssert.AreInserted(load_attachment.Task);
            DBAssert.AreInserted(load_attachment.Uploader);
            Assert.That(load_attachment.FileName, Is.EqualTo(filename));
        }
    }
}
