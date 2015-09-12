using FFLTask.BLL.Entity;
using FFLTask.GLB.Global.Enum;
using Global.Core.ExtensionMethod;
using Global.Core.Helper;
using NUnit.Framework;

namespace FFLTask.BLL.EntityTest
{
    public static class TaskTestHelper
    {
        internal static HistoryItem get_latest_history(this Task task)
        {
            return task.Histroy[task.Histroy.Count - 1];
        }

        internal static void has_update_latest(this Task task)
        {
            Assert.That(task.LatestUpdateTime, Is.EqualTo(SystemTime.Now()));
        }

        internal static void has_begin(this Task task, string description = Constants.DescriptionBeginWork)
        {
            Assert.That(task.CurrentStatus, Is.EqualTo(Status.BeginWork));
            task.has_update_latest();
            Assert.That(task.get_latest_history().Description, Is.EqualTo(description));
        }

        internal static void has_accept(this Task task, string description = Constants.DescriptionAccept)
        {
            Assert.That(task.HasAccepted, Is.EqualTo(true));
            Assert.That(task.CurrentStatus, Is.EqualTo(Status.Accept));
            task.has_update_latest();
            Assert.That(task.get_latest_history().Description, Is.EqualTo(description));
        }

        internal static void has_accept(this Task task, TaskQuality? quality)
        {
            Assert.That(task.HasAccepted, Is.EqualTo(true));
            Assert.That(task.CurrentStatus, Is.EqualTo(Status.Accept));
            Assert.That(task.Quality, Is.EqualTo(quality));

            task.has_update_latest();

            HistoryItem latest = task.get_latest_history();
            Assert.That(latest.Description, Is.EqualTo("验收任务"));
        }

        internal static void has_complete(this Task task, string description = Constants.DescriptionComplete)
        {
            Assert.That(task.CurrentStatus, Is.EqualTo(Status.Complete));
            task.has_update_latest();
            Assert.That(task.get_latest_history().Description, Is.EqualTo(description));
        }
    }
}
