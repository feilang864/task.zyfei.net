using System;
using System.Collections.Generic;
using System.Linq;
using FFLTask.BLL.Entity;
using FFLTask.GLB.Global.Enum;
using NUnit.Framework;

namespace FFLTask.BLL.NHMapTest
{
    [TestFixture]
    class TaskMapTest : BaseMapTest<Task>
    {
        [Test]
        public void Normal()
        {
            #region Initialize

            DateTime task_accept_time = new DateTime(1, 1, 1);
            DateTime task_actualcomplete_time = new DateTime(2, 2, 2);
            DateTime task_assign_time = new DateTime(3, 3, 3);
            DateTime task_expectcomplete_time = new DateTime(4, 4, 4);
            DateTime task_latestupdate_time = new DateTime(5, 5, 5);
            DateTime task_own_time = new DateTime(6, 6, 6);

            string task_title = "task_title";
            string task_body = "this is a task";
            Status task_current_status = Status.Accept;
            int task_sequence = 18;
            TaskDifficulty task_difficulty = TaskDifficulty.Easy;
            int task_expect_workperiod = 2;
            int task_overdue = 3;
            int task_delay = 4;
            TaskPriority task_priority = TaskPriority.Low;
            TaskQuality task_quality = TaskQuality.Good;
            HistoryItem history_item_1 = new HistoryItem();
            WorkPeriod work_period_1 = new WorkPeriod();
            WorkPeriod work_period_2 = new WorkPeriod();
            string editing_name = "yezi";
            User editing_user = new User { Name = editing_name };
            Task parent_task = new Task();
            Task task_child_1 = new Task();
            Task task_child_2 = new Task();
            Attachment attachment_1 = new Attachment();
            Attachment attachment_2 = new Attachment();

            #region Initialize task

            Task task = new Task
            {
                AcceptTime = task_accept_time,
                ActualCompleteTime = task_actualcomplete_time,
                AssignTime = task_assign_time,
                Body = task_body,
                ExpectCompleteTime = task_expectcomplete_time,
                LatestUpdateTime = task_latestupdate_time,
                OwnTime = task_own_time,
                CurrentStatus = task_current_status,
                Sequence = task_sequence,
                Difficulty = task_difficulty,
                IsVirtual = true,
                Title = task_title,
                ExpectWorkPeriod = task_expect_workperiod,
                HasAccepted = true,
                OverDue = task_overdue,
                Delay = task_delay,
                Priority = task_priority,
                Quality = task_quality,
                Histroy = new List<HistoryItem> { history_item_1 },
                Owner = new User(),
                Project = new Project(),
                Publisher = new User(),
                Accepter = new User(),
                WorkPeriods = new List<WorkPeriod> { work_period_1, work_period_2 },
                EditingBy = editing_user,
                Parent = parent_task,
                Children = new List<Task> { task_child_1, task_child_2 },
                Attachments = new List<Attachment> { attachment_1, attachment_2 }
            };
            history_item_1.Belong = task;
            work_period_1.Task = task;
            work_period_2.Task = task;
            task_child_1.Parent = task;
            task_child_2.Parent = task;
            attachment_1.Task = task;
            attachment_2.Task = task;

            #endregion

            #endregion

            Task load_task = Save(task);

            #region Assert

            Assert.That(load_task.AcceptTime, Is.EqualTo(task_accept_time));
            Assert.That(load_task.ActualCompleteTime, Is.EqualTo(task_actualcomplete_time));
            Assert.That(load_task.AssignTime, Is.EqualTo(task_assign_time));
            Assert.That(load_task.Body, Is.EqualTo(task_body));
            Assert.That(load_task.ExpectCompleteTime, Is.EqualTo(task_expectcomplete_time));
            Assert.That(load_task.LatestUpdateTime, Is.EqualTo(task_latestupdate_time));
            Assert.That(load_task.OwnTime, Is.EqualTo(task_own_time));

            Assert.That(load_task.CurrentStatus, Is.EqualTo(task_current_status));
            Assert.That(load_task.Sequence, Is.EqualTo(task_sequence));
            Assert.That(load_task.Difficulty, Is.EqualTo(task_difficulty));
            Assert.That(load_task.IsVirtual, Is.EqualTo(true));
            Assert.That(load_task.Title, Is.EqualTo(task_title));
            Assert.That(load_task.ExpectWorkPeriod, Is.EqualTo(task_expect_workperiod));
            Assert.That(load_task.HasAccepted, Is.EqualTo(true));
            Assert.That(load_task.OverDue, Is.EqualTo(task_overdue));
            Assert.That(load_task.Delay, Is.EqualTo(task_delay));
            Assert.That(load_task.Priority, Is.EqualTo(task_priority));
            Assert.That(load_task.Quality, Is.EqualTo(task_quality));

            Assert.That(load_task.Histroy.Count, Is.EqualTo(1));
            DBAssert.AreInserted(load_task.Owner);
            DBAssert.AreInserted(load_task.Project);
            DBAssert.AreInserted(load_task.Publisher);
            DBAssert.AreInserted(load_task.Accepter);
            Assert.That(load_task.WorkPeriods.Count, Is.EqualTo(2));
            Assert.That(load_task.EditingBy.Name, Is.EqualTo(editing_user.Name));

            Assert.That(load_task.HasAccepted, Is.EqualTo(true));
            Assert.That(load_task.Parent.Id, Is.EqualTo(parent_task.Id));
            Assert.That(load_task.Children.Count, Is.EqualTo(2));
            //TODO: need these? or need encapulate?
            Assert.That(load_task.Children.Count(x => x.Id == task_child_1.Id), Is.EqualTo(1));
            Assert.That(load_task.Children.Count(x => x.Id == task_child_2.Id), Is.EqualTo(1));
            Assert.That(load_task.Attachments.Count, Is.EqualTo(2));

            #endregion
        }
    }
}
