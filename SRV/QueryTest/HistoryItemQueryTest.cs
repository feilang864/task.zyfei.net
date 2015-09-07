using System.Collections.Generic;
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
    public class HistoryItemQueryTest : BaseQueryTest
    {
        [Test]
        public void Get()
        {
            #region initialization

            Task task_1 = new Task();
            Task task_2 = new Task();
            Task task_3 = new Task();

            HistoryItem task_1_assign = create_item(task_1, Status.Assign);
            HistoryItem task_1_doubt_1 = create_item(task_1, Status.Doubt);
            HistoryItem task_1_doubt_2 = create_item(task_1, Status.Doubt);

            HistoryItem task_2_assign = create_item(task_2, Status.Assign);
            HistoryItem task_2_doubt = create_item(task_2, Status.Doubt);

            HistoryItem task_3_publish = create_item(task_3, Status.Publish);
            HistoryItem task_3_doubt = create_item(task_3, Status.Doubt);

            #endregion

            HistoryItemQuery query = new HistoryItemQuery(session.Query<HistoryItem>());

            IQueryable<Task> tasks;

            #region task_1

            tasks = session.Query<Task>().Where(t => t.Id == task_1.Id);

            var result_task_1_assign = query.Get(tasks, Status.Assign).ToList();
            Assert.That(result_task_1_assign.Count(), Is.EqualTo(1));
            contains(result_task_1_assign, task_1_assign);

            var result_task_1_doubt = query.Get(tasks, Status.Doubt).ToList();
            Assert.That(result_task_1_doubt.Count, Is.EqualTo(2));
            contains(result_task_1_doubt, task_1_doubt_1);
            contains(result_task_1_doubt, task_1_doubt_2);

            #endregion

            #region task_2

            tasks = session.Query<Task>().Where(t => t.Id == task_2.Id);

            var result_task_2_assign = query.Get(tasks, Status.Assign).ToList();
            Assert.That(result_task_2_assign.Count, Is.EqualTo(1));
            contains(result_task_2_assign, task_2_assign);

            var result_task_2_doubt = query.Get(tasks, Status.Doubt).ToList();
            Assert.That(result_task_2_doubt.Count, Is.EqualTo(1));
            contains(result_task_2_doubt, task_2_doubt);

            #endregion

            #region task_3

            tasks = session.Query<Task>().Where(t => t.Id == task_3.Id);

            var result_task_3_assign = query.Get(tasks, Status.Publish).ToList();
            Assert.That(result_task_3_assign.Count, Is.EqualTo(1));
            contains(result_task_3_assign, task_3_publish);

            var result_task_3_doubt = query.Get(tasks, Status.Doubt).ToList();
            Assert.That(result_task_3_doubt.Count, Is.EqualTo(1));
            contains(result_task_3_doubt, task_3_doubt);

            #endregion

            #region task_1 and task_2

            tasks = session.Query<Task>().Where(t => t.Id == task_1.Id || t.Id == task_2.Id);

            var result_task_1_and_task_2_assign = query.Get(tasks, Status.Assign).ToList();
            Assert.That(result_task_1_and_task_2_assign.Count, Is.EqualTo(2));
            contains(result_task_1_and_task_2_assign, task_1_assign);
            contains(result_task_1_and_task_2_assign, task_2_assign);

            var result_task_1_and_task_2_doubt = query.Get(tasks, Status.Doubt).ToList();
            Assert.That(result_task_1_and_task_2_doubt.Count, Is.EqualTo(3));
            contains(result_task_1_and_task_2_doubt, task_1_doubt_1);
            contains(result_task_1_and_task_2_doubt, task_1_doubt_2);
            contains(result_task_1_and_task_2_doubt, task_2_doubt);

            #endregion

            #region task_1 and task_3

            tasks = session.Query<Task>().Where(t => t.Id == task_1.Id || t.Id == task_3.Id);

            var result_task_1_and_task_3_assign = query.Get(tasks, Status.Assign).ToList();
            Assert.That(result_task_1_and_task_3_assign.Count, Is.EqualTo(1));
            contains(result_task_1_and_task_3_assign, task_1_assign);

            var result_task_1_and_task_3_Publish = query.Get(tasks, Status.Publish).ToList();
            Assert.That(result_task_1_and_task_3_Publish.Count, Is.EqualTo(1));
            contains(result_task_1_and_task_3_Publish, task_3_publish);

            var result_task_1_and_task_3_doubt = query.Get(tasks, Status.Doubt).ToList();
            Assert.That(result_task_1_and_task_3_doubt.Count, Is.EqualTo(3));
            contains(result_task_1_and_task_3_doubt, task_1_doubt_1);
            contains(result_task_1_and_task_3_doubt, task_1_doubt_2);
            contains(result_task_1_and_task_3_doubt, task_3_doubt);

            #endregion

            #region task_2 and task_3

            tasks = session.Query<Task>().Where(t => t.Id == task_2.Id || t.Id == task_3.Id);

            var result_task_2_and_task_3_assign = query.Get(tasks, Status.Assign).ToList();
            Assert.That(result_task_2_and_task_3_assign.Count, Is.EqualTo(1));
            contains(result_task_2_and_task_3_assign, task_2_assign);

            var result_task_2_and_task_3_Publish = query.Get(tasks, Status.Publish).ToList();
            Assert.That(result_task_2_and_task_3_Publish.Count, Is.EqualTo(1));
            contains(result_task_2_and_task_3_Publish, task_3_publish);

            var result_task_2_and_task_3_doubt = query.Get(tasks, Status.Doubt).ToList();
            Assert.That(result_task_2_and_task_3_doubt.Count, Is.EqualTo(2));
            contains(result_task_2_and_task_3_doubt, task_2_doubt);
            contains(result_task_2_and_task_3_doubt, task_3_doubt);

            #endregion

            #region task_1 and task_2 and task_3

            tasks = session.Query<Task>().Where(t => t.Id == task_1.Id || t.Id == task_2.Id || t.Id == task_3.Id);

            var result_task_1_and_task_2_and_task_3_assign = query.Get(tasks, Status.Assign).ToList();
            Assert.That(result_task_1_and_task_2_and_task_3_assign.Count, Is.EqualTo(2));
            contains(result_task_1_and_task_2_and_task_3_assign, task_1_assign);
            contains(result_task_1_and_task_2_and_task_3_assign, task_2_assign);

            var result_task_1_and_task_2_and_task_3_Publish = query.Get(tasks, Status.Publish).ToList();
            Assert.That(result_task_1_and_task_2_and_task_3_Publish.Count, Is.EqualTo(1));
            contains(result_task_1_and_task_2_and_task_3_Publish, task_3_publish);

            var result_task_1_and_task_2_and_task_3_doubt = query.Get(tasks, Status.Doubt).ToList();
            Assert.That(result_task_1_and_task_2_and_task_3_doubt.Count, Is.EqualTo(4));
            contains(result_task_1_and_task_2_and_task_3_doubt, task_1_doubt_1);
            contains(result_task_1_and_task_2_and_task_3_doubt, task_1_doubt_2);
            contains(result_task_1_and_task_2_and_task_3_doubt, task_2_doubt);
            contains(result_task_1_and_task_2_and_task_3_doubt, task_3_doubt);

            #endregion
        }

        private void contains(List<HistoryItem> items, HistoryItem item)
        {
            Assert.That(items.Exists(h => h.Id == item.Id));
        }

        private HistoryItem create_item(Task task, Status status)
        {
            HistoryItem item = new HistoryItem();
            item.SetProperty("Belong", task);
            item.SetProperty("Status", status);
            Save(item);

            return item;
        }
    }
}
