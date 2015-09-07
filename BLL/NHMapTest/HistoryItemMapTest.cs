using System;
using FFLTask.BLL.Entity;
using FFLTask.GLB.Global.Enum;
using NUnit.Framework;

namespace FFLTask.BLL.NHMapTest
{
    [TestFixture]
    class HistoryItemMapTest : BaseMapTest<HistoryItem>
    {
        [Test]
        public void Normal()
        {
            string comment = "please do it ASAP";
            DateTime createTime = new DateTime(2014, 8, 12);
            string description = "Lisa has assign the task";
            Status status = Status.Own;
            HistoryItem item = new HistoryItem
            {
                Belong = new Task(),
                Comment = comment,
                Description = description,
                Executor = new User(),
                Status = status
            };
            item.MockCreateTime(createTime);

            HistoryItem loaded_item = Save(item);

            DBAssert.AreInserted(item.Belong);
            Assert.That(item.Comment, Is.EqualTo(loaded_item.Comment));
            Assert.That(item.Description, Is.EqualTo(loaded_item.Description));
            DBAssert.AreInserted(item.Executor);
            Assert.That(loaded_item.Status, Is.EqualTo(status));
        }
    }
}
