using System;
using FFLTask.BLL.Entity;
using FFLTask.GLB.Global.Enum;
using Global.Core.ExtensionMethod;
using Global.Core.Helper;
using NUnit.Framework;
namespace FFLTask.BLL.EntityTest
{
    [TestFixture]
    public class TaskTest
    {
        DateTime now;

        [SetUp]
        public void SetUp()
        {
            now = new DateTime(2015, 1, 19, 15, 54, 12);
            SystemTime.SetDateTime(now);
        }

        [Test]
        public void BeginWork()
        {
            Task task = new Task();

            task.BeginWork();

            task.has_begin();
        }

        [Test]
        public void Update_Property()
        {
            Task task = new Task { Publisher = new User() };
            task.Publish();

            task.UpdateProperty();
            Assert.That(task.CurrentStatus, Is.EqualTo(Status.Publish));

            task.Owner = new User();
            task.Own();
            task.BeginWork();
            task.Doubt();

            task.UpdateProperty();
            Assert.That(task.CurrentStatus, Is.EqualTo(Status.Update));
            Assert.That(task.get_latest_history().Status, Is.EqualTo(Status.Update));
        }

        [Test]
        public void Doubt()
        {
            Task task = new Task
            {
                Publisher = new User(),
                Owner = new User()
            };
            task.Publish();
            task.Assign();
            task.Own();
            task.BeginWork();
            task.Doubt();

            Assert.That(task.CurrentStatus, Is.EqualTo(Status.Doubt));
        }

        [Test]
        public void Complete()
        {
            Task task = new Task { Accepter = new User(), ExpectCompleteTime = SystemTime.Now() };
            task.BeginWork();

            SystemTime.SetDateTime(now.AddHours(3));
            task.Complete();

            task.has_complete();
            Assert.That(task.Delay, Is.EqualTo(3));
        }

        [Test]
        public void Refused()
        {
            Task task = new Task
            {
                Project = new Project()
            };
            string comment = "accept comment";
            task.RefuseAccept();

            Assert.That(task.HasAccepted, Is.EqualTo(false));
            Assert.That(task.CurrentStatus, Is.EqualTo(Status.RefuseAccept));

            //TODO: HistoryItems
        }

        [Test]
        public void Dissent()
        {
            Task task = new Task
            {
                Publisher = new User(),
                Owner = new User(),
                Accepter = new User()
            };
            task.Publish();
            task.Assign();
            task.BeginWork();
            task.Complete();
            task.RefuseAccept();
            task.Dissent();

            task.has_update_latest();
            Assert.That(task.CurrentStatus, Is.EqualTo(Status.Dissent));
        }

        [Test]
        public void Accept()
        {
            Task task = new Task
            {
                Owner = new User(),
                Accepter = new User()
            };
            task.BeginWork();
            task.Complete();
            task.Accept(TaskQuality.Good);

            task.has_accept(TaskQuality.Good);
        }

        [Test]
        public void Remove()
        {
            #region Publish Status

            Task task_when_publish = new Task { Publisher = new User() };
            task_when_publish.Publish();

            task_when_publish.Remove();
            Assert.That(task_when_publish.CurrentStatus, Is.EqualTo(Status.Remove));

            #endregion

            #region Assign Status

            Task task_when_assign = new Task { Publisher = new User() };
            task_when_assign.Owner = new User();
            task_when_assign.Publish();
            task_when_assign.Assign();

            task_when_assign.Remove();
            Assert.That(task_when_assign.CurrentStatus, Is.EqualTo(Status.Remove));

            #endregion

            #region Quit Status

            Task task_when_quit = new Task { Publisher = new User() };
            task_when_quit.Owner = new User();
            task_when_quit.Publish();
            task_when_quit.Assign();
            task_when_quit.BeginWork();
            task_when_quit.Quit();

            task_when_quit.Remove();
            Assert.That(task_when_quit.CurrentStatus, Is.EqualTo(Status.Remove));

            #endregion
        }

        [Test]
        public void Resume_When_Publish()
        {
            Task task = new Task { Publisher = new User() };

            SystemTime.SetDateTime(new DateTime(2014, 11, 1));
            task.Publish();
            SystemTime.SetDateTime(new DateTime(2014, 11, 2));
            task.Remove();
            SystemTime.SetDateTime(new DateTime(2014, 11, 3));
            task.Resume();

            Assert.That(task.CurrentStatus, Is.EqualTo(Status.Publish));
        }

        [Test]
        public void Resume_When_Assign()
        {
            Task task = new Task { Publisher = new User() };
            task.Owner = new User();
            SystemTime.SetDateTime(new DateTime(2014, 11, 1));
            task.Publish();
            SystemTime.SetDateTime(new DateTime(2014, 11, 2));
            task.Assign();
            SystemTime.SetDateTime(new DateTime(2014, 11, 3));
            task.Remove();
            SystemTime.SetDateTime(new DateTime(2014, 11, 4));
            task.Resume();

            Assert.That(task.CurrentStatus, Is.EqualTo(Status.Assign));
        }

        [Test]
        public void Resume_When_Quit()
        {
            Task task = new Task { Publisher = new User() };
            task.Owner = new User();
            SystemTime.SetDateTime(new DateTime(2014, 11, 1));
            task.Publish();
            SystemTime.SetDateTime(new DateTime(2014, 11, 2));
            task.Assign();
            SystemTime.SetDateTime(new DateTime(2014, 11, 3));
            task.BeginWork();
            SystemTime.SetDateTime(new DateTime(2014, 11, 4));
            task.Quit();
            SystemTime.SetDateTime(new DateTime(2014, 11, 5));
            task.Remove();
            SystemTime.SetDateTime(new DateTime(2014, 11, 6));
            task.Resume();

            Assert.That(task.CurrentStatus, Is.EqualTo(Status.Quit));
        }

        [Test]
        public void Comment()
        {
            User publisher = new User();
            User owner = new User();
            Project project = new Project();

            Task task = new Task
            {
                Project = project,
                Publisher = publisher,
                Owner = owner
            };
            task.Publish();
            task.Assign();

            Assert.That(task.Histroy.Count, Is.EqualTo(2));
            Assert.That(publisher.MessagesToMe.IsNullOrEmpty(), Is.True);
            Assert.That(owner.MessagesFromMe.IsNullOrEmpty(), Is.True);

            string comment = "工时太少";
            task.Comment(owner, publisher, comment);

            Assert.That(task.Histroy.Count, Is.EqualTo(3));
            Assert.That(publisher.MessagesToMe[0].Task, Is.EqualTo(task));
            Assert.That(publisher.MessagesToMe[0].Project, Is.EqualTo(project));
            Assert.That(task.get_latest_history().Comment, Is.EqualTo(comment));
            Assert.That(task.get_latest_history().Description, Is.EqualTo(Constants.DescriptionComment));

            Assert.That(publisher.MessagesToMe.Count, Is.EqualTo(1));
            Assert.That(publisher.MessagesToMe[0].Content, Is.EqualTo(comment));
            Assert.That(owner.MessagesFromMe.Count, Is.EqualTo(1));
            Assert.That(owner.MessagesFromMe[0].Content, Is.EqualTo(comment));
        }

        [Test]
        public void GetPrevious()
        {
            Task parent = new Task();
            Task first_task = new Task();
            Task middle_task = new Task();
            Task last_task = new Task();

            parent.AddChild(first_task);
            parent.AddChild(middle_task);
            parent.AddChild(last_task);

            Assert.That(parent.GetPrevious(), Is.Null);
            Assert.That(first_task.GetPrevious(), Is.Null);
            Assert.That(middle_task.GetPrevious(), Is.EqualTo(first_task));
            Assert.That(last_task.GetPrevious(), Is.EqualTo(middle_task));
        }

        [Test]
        public void GetNext()
        {
            Task parent = new Task();
            Task first_task = new Task();
            Task middle_task = new Task();
            Task last_task = new Task();

            parent.AddChild(first_task);
            parent.AddChild(middle_task);
            parent.AddChild(last_task);

            Assert.That(parent.GetNext(), Is.Null);
            Assert.That(first_task.GetNext(), Is.EqualTo(middle_task));
            Assert.That(middle_task.GetNext(), Is.EqualTo(last_task));
            Assert.That(last_task.GetNext(), Is.Null);
        }

        [Test]
        public void IsLastInBrothers()
        {
            Task parent = new Task();
            parent.SetPropertyInBase("Id", 1);
            Task first_task = new Task { CurrentStatus = Status.Assign };
            first_task.SetPropertyInBase("Id", 2);
            Task middle_task = new Task { CurrentStatus = Status.Assign };
            middle_task.SetPropertyInBase("Id", 3);
            Task last_task = new Task { CurrentStatus = Status.Accept };
            last_task.SetPropertyInBase("Id", 4);

            parent.AddChild(first_task);
            parent.AddChild(middle_task);
            parent.AddChild(last_task);

            Assert.That(first_task.IsLastInBrothers(Status.Assign), Is.False);
            Assert.That(middle_task.IsLastInBrothers(Status.Assign), Is.False);

            //all other brothers are Assign status except himselft, so he's the last one with Assign status.
            Assert.That(last_task.IsLastInBrothers(Status.Assign), Is.True);
        }

        [TearDown]
        public void TearDown()
        {
            SystemTime.ResetDateTime();
        }
    }
}
