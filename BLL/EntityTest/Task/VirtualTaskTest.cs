using System;
using System.Collections.Generic;
using FFLTask.BLL.Entity;
using FFLTask.GLB.Global.Enum;
using Global.Core.Helper;
using NUnit.Framework;

namespace FFLTask.BLL.EntityTest
{
    [TestFixture]
    public class VirtualTaskTest
    {
        DateTime now;

        [SetUp]
        public void SetUp()
        {
            SystemTime.SetDateTime(new DateTime(2014, 1, 1));
            now = SystemTime.Now();
        }

        [Test]
        public void BeginWork_With_Parent()
        {
            #region preparation

            Task task = get_task_with_parents();
            task.Owner = new User();

            Task parent = task.Parent;
            parent.IsVirtual = true;

            Task grand_parent = task.Parent.Parent;
            grand_parent.IsVirtual = true;
            User grand_parent_owner = new User();
            grand_parent.Owner = grand_parent_owner;

            #endregion

            #region the first time the task begin

            task.BeginWork();

            task.has_begin();

            parent.has_begin(Constants.CommentAutoBegin);
            Assert.That(parent.Owner, Is.EqualTo(task.Owner));
            has_auto_own(parent);

            grand_parent.has_begin(Constants.CommentAutoBegin);
            //the owner will not change because it had one before
            Assert.That(grand_parent.Owner, Is.EqualTo(grand_parent_owner));

            #endregion

            #region a second time the task's brother begin

            Task brother = new Task();
            parent.AddChild(brother);

            SystemTime.SetDateTime(now.AddMinutes(23));
            brother.BeginWork();

            Assert.That(parent.LatestUpdateTime, Is.EqualTo(now));

            #endregion
        }

        [Test]
        public void BeginWork_Not_Affect_Parent()
        {
            #region preparation

            Task task = get_task_with_parents();
            task.Owner = new User();

            Task parent = task.Parent;

            Task grand_parent = task.Parent.Parent;
            User grand_parent_owner = new User();
            grand_parent.Owner = grand_parent_owner;

            #endregion

            #region will not affect its parent

            task.BeginWork();

            task.has_begin();

            Assert.That(parent.CurrentStatus, Is.Not.EqualTo(Status.BeginWork));
            Assert.That(grand_parent.CurrentStatus, Is.Not.EqualTo(Status.BeginWork));

            #endregion
        }

        [Test]
        public void Complete_With_Parent()
        {
            Task task = get_task_with_brothers_and_parents();

            Task parent = task.Parent;
            parent.IsVirtual = true;

            Task grand_parent = parent.Parent;
            grand_parent.IsVirtual = true;

            SystemTime.SetDateTime(now.AddMinutes(53));
            task.Complete();
            task.AutoCompleteAncestors();

            task.has_complete();

            parent.has_complete(Constants.CommentAutoComplete);

            grand_parent.has_complete(Constants.CommentAutoComplete);
        }

        [Test]
        public void Complete_When_Parent_Is_Not_Virtual()
        {
            Task task = get_task_with_brothers_and_parents();

            Task parent = task.Parent;
            Task grand_parent = parent.Parent;

            task.Complete();
            task.AutoCompleteAncestors();

            task.has_complete();

            Assert.That(parent.CurrentStatus, Is.EqualTo(Status.BeginWork));
            Assert.That(grand_parent.CurrentStatus, Is.EqualTo(Status.BeginWork));
        }

        [Test]
        public void Complete_When_There_Is_Still_Incompleted_In_Brothers()
        {
            Task task = get_task_with_brothers_and_parents();

            Task parent = task.Parent;
            Task grand_parent = parent.Parent;

            //must set a different status
            parent.Children[1].CurrentStatus = Status.BeginWork;

            parent.IsVirtual = true;
            grand_parent.IsVirtual = true;

            task.Complete();
            task.AutoCompleteAncestors();

            task.has_complete();

            Assert.That(parent.CurrentStatus, Is.EqualTo(Status.BeginWork));
            Assert.That(grand_parent.CurrentStatus, Is.EqualTo(Status.BeginWork));
        }

        [Test]
        public void Accept_When_Owner_Is_Accepter()
        {
            User user = new User();
            Task task = new Task
            {
                Owner = user,
                Accepter = user
            };
            task.BeginWork();
            task.Complete();

            task.has_accept(Constants.CommentAutoAcceptForOwnerIsAccepter);
        }

        [Test]
        public void Accept_With_Parent()
        {
            #region preparation

            Task task = new Task
            {
                Project = get_project()
            };

            Task brother = new Task
            {
                Project = get_project()
            };

            Task parent = new Task
            {
                IsVirtual = true,
                Project = get_project()
            };
            parent.AddChild(task);
            parent.AddChild(brother);

            TaskQuality Quality = TaskQuality.Good;

            #endregion

            #region only the brother is accept, not affect parent

            brother.Accept(Quality);

            brother.has_accept(Quality);

            Assert.That(parent.CurrentStatus, Is.Not.EqualTo(Status.Accept));

            #endregion

            #region both the brother and task are accepted, affect their parent now

            task.Accept(Quality);
            task.AutoAcceptAncestors();

            task.has_accept(Quality);

            parent.has_accept(Constants.CommentAutoAcceptForChildren);
            Assert.That(parent.Quality, Is.Null);

            #endregion
        }

        [Test]
        public void Accept_When_Parent_Is_Not_Virtual()
        {
            Task task = get_task_with_brothers_and_parents();

            Task parent = task.Parent;
            Task grand_parent = parent.Parent;

            task.Accept(null);
            task.AutoCompleteAncestors();

            Assert.That(task.CurrentStatus, Is.EqualTo(Status.Accept));
            Assert.That(parent.CurrentStatus, Is.EqualTo(Status.BeginWork));
            Assert.That(grand_parent.CurrentStatus, Is.EqualTo(Status.BeginWork));
        }

        [Test]
        public void Accept_When_There_Is_Still_Incompleted_In_Brothers()
        {
            Task task = get_task_with_brothers_and_parents();

            Task parent = task.Parent;
            Task grand_parent = parent.Parent;

            //must set a different status
            parent.Children[1].CurrentStatus = Status.BeginWork;

            parent.IsVirtual = true;
            grand_parent.IsVirtual = true;

            task.Accept(null);
            task.AutoCompleteAncestors();

            Assert.That(task.CurrentStatus, Is.EqualTo(Status.Accept));
            Assert.That(parent.CurrentStatus, Is.EqualTo(Status.BeginWork));
            Assert.That(grand_parent.CurrentStatus, Is.EqualTo(Status.BeginWork));
        }

        private Task get_task_with_brothers_and_parents()
        {
            Task root = new Task { Accepter = new User() };
            root.BeginWork();

            Task branch_1 = new Task { Accepter = new User() };
            branch_1.BeginWork();
            root.AddChild(branch_1);

            Task branch_2 = new Task { Accepter = new User() };
            branch_2.CurrentStatus = Status.Complete;
            root.AddChild(branch_2);

            Task leaf_1 = new Task { Accepter = new User() };
            leaf_1.BeginWork();
            branch_1.AddChild(leaf_1);

            Task leaf_2 = new Task { Accepter = new User() };
            leaf_2.CurrentStatus = Status.Complete;
            branch_1.AddChild(leaf_2);

            return leaf_1;
        }

        [Obsolete()]
        private Project get_project()
        {
            Project project = new Project
            {
                Config = new ProjectConfig()
            };
            project.Config.SetQualities(new List<TaskQuality> { TaskQuality.Qualified, TaskQuality.Good });

            return project;
        }

        private Task get_task_with_parents()
        {
            Task task = new Task();
            task.AddChild(new Task());
            task.Children[0].AddChild(new Task());
            return task.Children[0].Children[0];
        }

        private void has_auto_own(Task parent)
        {
            HistoryItem own_history = parent.Histroy[0];

            Assert.That(own_history.Status, Is.EqualTo(Status.Own));
            Assert.That(own_history.Description, Is.EqualTo(Constants.CommentAutoOwn));
        }

        [TearDown]
        public void TearDown()
        {
            SystemTime.ResetDateTime();
        }
    }
}
