using NUnit.Framework;
using FFLTask.SRV.ViewModel.Shared;
using FFLTask.SRV.ViewModelMap;
using System;

namespace FFLTask.SRV.ViewModelMapTest
{
    [TestFixture]
    public class SplitDateTimeMapTest
    {
        [Test]
        public void Combine()
        {
            SplitDateTimeModel model = new SplitDateTimeModel
            {
                Date = "2014-3-12",
                Hour = 16,
                Minute = 31
            };

            Assert.That(model.Combine(), Is.EqualTo(new DateTime(2014, 3, 12, 16, 31, 0)));
        }

        [Test]
        public void Combine_Null()
        {
            SplitDateTimeModel model = new SplitDateTimeModel();
            Assert.That(model.Combine(), Is.Null);
        }

        [Test]
        public void Split()
        {
            DateTime? time = new DateTime(2014, 3, 12, 16, 31, 23);
            SplitDateTimeModel model = time.Split();

            Assert.That(model.Date, Is.EqualTo("2014-03-12"));
            Assert.That(model.Hour, Is.EqualTo(16));
            Assert.That(model.Minute, Is.EqualTo(31));
        }

        [Test]
        public void Split_Null()
        {
            DateTime? time = null;
            SplitDateTimeModel model = time.Split();

            Assert.That(model.Date, Is.Null.Or.Empty);
            Assert.That(model.Hour, Is.EqualTo(0));
            Assert.That(model.Minute, Is.EqualTo(0));
        }
    }
}
