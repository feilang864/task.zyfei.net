using System;
using System.Collections.Generic;
using System.Linq;
using FFLTask.BLL.Entity;
using FFLTask.GLB.Global.Enum;
using FFLTask.GLB.Global.UrlParameter;
using FFLTask.SRV.Query;
using FFLTask.SRV.ViewModel.Task;
using Global.Core.ExtensionMethod;
using NHibernate.Linq;
using NUnit.Framework;

namespace FFLTask.SRV.QueryTest
{
    [TestFixture]
    public class TaskQueryTest : BaseQueryTest
    {
        ListModel model;
        IList<Task> tasks;

        Project root, project_1, project_2, project_1_1, project_1_2;

        /// <summary>
        /// these tasks are in root
        /// </summary>
        Task task_root_1, task_root_2, task_root_3;

        /// <summary>
        /// these tasks are in project_1
        /// </summary>
        Task task_11, task_12, task_13, task_14, task_15, task_16;

        /// <summary>
        /// these tasks are in project_2
        /// </summary>
        Task task_21, task_22;

        /// <summary>
        /// these tasks are in project_1_1
        /// </summary>
        Task task_1_11, task_1_12, task_1_13;

        [SetUp]
        public void SetUp()
        {
            set_Projects_Hierarchy();
            set_Tasks_With_Project();

            model = new ListModel();
            model.TimeSpan = new _TimeSpanModel();
        }

        private void set_Projects_Hierarchy()
        {
            root = new Project();

            project_1 = new Project { Parent = root };

            project_2 = new Project { Parent = root };

            project_1_1 = new Project { Parent = project_1 };

            project_1_2 = new Project { Parent = project_1 };

            root.Children = new List<Project> { project_1, project_2 };
            project_1.Children = new List<Project> { project_1_1, project_1_2 };
        }

        private void set_Tasks_With_Project()
        {
            task_root_1 = new Task { Project = root };
            task_root_2 = new Task { Project = root };
            task_root_3 = new Task { Project = root };

            task_11 = new Task { Project = project_1 };
            task_12 = new Task { Project = project_1 };
            task_13 = new Task { Project = project_1 };
            task_14 = new Task { Project = project_1 };
            task_15 = new Task { Project = project_1 };
            task_16 = new Task { Project = project_1 };

            task_21 = new Task { Project = project_2 };
            task_22 = new Task { Project = project_2 };

            task_1_11 = new Task { Project = project_1_1 };
            task_1_12 = new Task { Project = project_1_1 };
            task_1_13 = new Task { Project = project_1_1 };        
        }

        [Test]
        public void Get_In_Project()
        {
            Project project_1 = new Project();

            Project project_2 = new Project();

            Task task_1 = new Task { Project = project_1 };
            Task task_2 = new Task { Project = project_2 };
            Task task_3 = new Task { Project = project_1 };

            Save(task_1, task_2, task_3);

            IList<Task> tasks = session.Query<Task>().GetInProject(project_2.Id).ToList();

            Assert.That(tasks.Count, Is.EqualTo(1));
            Contains(tasks, task_2);
        }

        [Test]
        public void Get_With_ProjectIds()
        {
            save();
            set_Current_Project();

            IList<int> ids = new List<int> { project_2.Id };
            tasks = session.Query<Task>().Get(model, ids).ToList();

            Assert.That(tasks.Count, Is.EqualTo(2));
            Contains(tasks, task_21);
            Contains(tasks, task_22);

            ids = new List<int>
            { 
                project_1.Id, 
                project_1_1.Id, 
                project_1_2.Id 
            };
            tasks = session.Query<Task>().Get(model, ids).ToList();

            Assert.That(tasks.Count, Is.EqualTo(9));
            Contains(tasks, task_1_11);
            Contains(tasks, task_12);
            Contains(tasks, task_13);
            Contains(tasks, task_14);
            Contains(tasks, task_15);
            Contains(tasks, task_16);
            Contains(tasks, task_1_11);
            Contains(tasks, task_1_12);
            Contains(tasks, task_1_13);

            ids = new List<int> 
            { 
                root.Id, 
                project_1.Id,
                project_2.Id,
                project_1_1.Id, 
                project_1_2.Id
            };
            tasks = session.Query<Task>().Get(model, ids).ToList();

            Assert.That(tasks.Count, Is.EqualTo(14));
            Contains(tasks, task_root_1);
            Contains(tasks, task_root_2);
            Contains(tasks, task_root_3);
            Contains(tasks, task_11);
            Contains(tasks, task_12);
            Contains(tasks, task_13);
            Contains(tasks, task_14);
            Contains(tasks, task_15);
            Contains(tasks, task_16);
            Contains(tasks, task_21);
            Contains(tasks, task_22);
            Contains(tasks, task_1_11);
            Contains(tasks, task_1_12);
            Contains(tasks, task_1_13);
        }

        #region Separate Test on Filter

        [Test]
        public void Get_By_OverDue()
        {
            task_11.OverDue = 0;
            task_12.OverDue = 5;
            task_13.OverDue = 19;
            task_14.OverDue = null;
            task_15.OverDue = -8;
            task_16.OverDue = 20;

            task_21.OverDue = 13;

            save();
            set_Current_Project();

            //both greater and less are null
            model.GreaterOverDue = null;
            model.LessOverDue = null;
            tasks = session.Query<Task>().Get(model).ToList();
            Assert.That(tasks.Count, Is.EqualTo(6));
            Contains(tasks, task_11);
            Contains(tasks, task_12);
            Contains(tasks, task_13);
            Contains(tasks, task_14);
            Contains(tasks, task_15);
            Contains(tasks, task_16);

            //only greater is null
            model.GreaterOverDue = null;
            model.LessOverDue = 15;
            tasks = session.Query<Task>().Get(model).ToList();
            Assert.That(tasks.Count, Is.EqualTo(3));
            Contains(tasks, task_11);
            Contains(tasks, task_12);
            Contains(tasks, task_15);

            //only less is null
            model.GreaterOverDue = 7;
            model.LessOverDue = null;
            tasks = session.Query<Task>().Get(model).ToList();
            Assert.That(tasks.Count, Is.EqualTo(2));
            Contains(tasks, task_13);
            Contains(tasks, task_16);

            //both greater and less are not null
            model.GreaterOverDue = -9;
            model.LessOverDue = 19;
            tasks = session.Query<Task>().Get(model).ToList();
            Assert.That(tasks.Count, Is.EqualTo(3));
            Contains(tasks, task_11);
            Contains(tasks, task_12);
            Contains(tasks, task_15);
        }

        [Test]
        public void Get_By_WorkPeriod()
        {
            task_11.MockActualWorkPeriod(0);
            task_12.MockActualWorkPeriod(5);
            task_13.MockActualWorkPeriod(19);
            // task_4.MockActualWorkPeriod(null);
            task_15.MockActualWorkPeriod(-8);
            task_16.MockActualWorkPeriod(20);
            task_21.MockActualWorkPeriod(13);

            save();
            set_Current_Project();

            //both greater and less are null
            model.GreaterWorkPeriod = null;
            model.LessWorkPeriod = null;
            tasks = session.Query<Task>().Get(model).ToList();
            Assert.That(tasks.Count, Is.EqualTo(6));
            Contains(tasks, task_11);
            Contains(tasks, task_12);
            Contains(tasks, task_13);
            Contains(tasks, task_14);
            Contains(tasks, task_15);
            Contains(tasks, task_16);

            //only greater is null
            model.GreaterWorkPeriod = null;
            model.LessWorkPeriod = 15;
            tasks = session.Query<Task>().Get(model).ToList();
            Assert.That(tasks.Count, Is.EqualTo(3));
            Contains(tasks, task_11);
            Contains(tasks, task_12);
            Contains(tasks, task_15);

            //only less is null
            model.GreaterWorkPeriod = 5;
            model.LessWorkPeriod = null;
            tasks = session.Query<Task>().Get(model).ToList();
            Assert.That(tasks.Count, Is.EqualTo(2));
            Contains(tasks, task_13);
            Contains(tasks, task_16);

            //both greater and less are not null
            model.GreaterWorkPeriod = -9;
            model.LessWorkPeriod = 19;
            tasks = session.Query<Task>().Get(model).ToList();
            Assert.That(tasks.Count, Is.EqualTo(3));
            Contains(tasks, task_11);
            Contains(tasks, task_12);
            Contains(tasks, task_15);
        }

        [Test]
        public void Get_By_ExpectComplete_DateTime()
        {
            task_11.ExpectCompleteTime = null;
            task_12.ExpectCompleteTime = new DateTime(2013, 12, 31);
            task_13.ExpectCompleteTime = new DateTime(2014, 1, 1);
            task_14.ExpectCompleteTime = new DateTime(2014, 1, 1, 6, 37, 42);
            task_15.ExpectCompleteTime = new DateTime(2014, 1, 1, 9, 42, 0);
            task_16.ExpectCompleteTime = new DateTime(2014, 5, 7);

            task_21.ExpectCompleteTime = new DateTime(2014, 1, 1); ;

            save();
            set_Current_Project();

            //both from and to are null
            model.TimeSpan.FromExpectComplete = null;
            model.TimeSpan.ToExpectComplete = null;
            tasks = session.Query<Task>().Get(model).ToList();
            Assert.That(tasks.Count, Is.EqualTo(6));
            Contains(tasks, task_11);
            Contains(tasks, task_12);
            Contains(tasks, task_13);
            Contains(tasks, task_14);
            Contains(tasks, task_15);
            Contains(tasks, task_16);

            //only from is null
            model.TimeSpan.FromExpectComplete = null;
            model.TimeSpan.ToExpectComplete = new DateTime(2014, 1, 2);
            tasks = session.Query<Task>().Get(model).ToList();
            Assert.That(tasks.Count, Is.EqualTo(4));
            Contains(tasks, task_12);
            Contains(tasks, task_13);
            Contains(tasks, task_14);
            Contains(tasks, task_15);

            //only to is null
            model.TimeSpan.FromExpectComplete = new DateTime(2014, 1, 1);
            model.TimeSpan.ToExpectComplete = null;
            tasks = session.Query<Task>().Get(model).ToList();
            Assert.That(tasks.Count, Is.EqualTo(3));
            Contains(tasks, task_14);
            Contains(tasks, task_15);
            Contains(tasks, task_16);

            //both greater and less are not null
            model.TimeSpan.FromExpectComplete = new DateTime(2013, 12, 31);
            model.TimeSpan.ToExpectComplete = new DateTime(2014, 5, 1);
            tasks = session.Query<Task>().Get(model).ToList();
            Assert.That(tasks.Count, Is.EqualTo(3));
            Contains(tasks, task_13);
            Contains(tasks, task_14);
            Contains(tasks, task_15);
        }

        [Test]
        public void Get_By_Publish_DateTime()
        {
            task_11.MockCreateTime(new DateTime(2013, 12, 1));
            task_12.MockCreateTime(new DateTime(2013, 12, 31, 23, 59, 59));
            task_13.MockCreateTime(new DateTime(2014, 1, 1, 0, 0, 1));
            task_14.MockCreateTime(new DateTime(2014, 1, 1, 6, 37, 42));
            task_15.MockCreateTime(new DateTime(2014, 1, 1, 9, 42, 0));
            task_16.MockCreateTime(new DateTime(2014, 5, 7));

            task_21.MockCreateTime(new DateTime(2014, 1, 1));

            save();
            set_Current_Project();

            //both from and to are null
            model.TimeSpan.FromPublish = null;
            model.TimeSpan.ToPublish = null;
            tasks = session.Query<Task>().Get(model).ToList();
            Assert.That(tasks.Count, Is.EqualTo(6));
            Contains(tasks, task_11);
            Contains(tasks, task_12);
            Contains(tasks, task_13);
            Contains(tasks, task_14);
            Contains(tasks, task_15);
            Contains(tasks, task_16);

            //only from is null
            model.TimeSpan.FromPublish = null;
            model.TimeSpan.ToPublish = new DateTime(2014, 1, 2);
            tasks = session.Query<Task>().Get(model).ToList();
            Assert.That(tasks.Count, Is.EqualTo(5));
            Contains(tasks, task_11);
            Contains(tasks, task_12);
            Contains(tasks, task_13);
            Contains(tasks, task_14);
            Contains(tasks, task_15);

            //only to is null
            model.TimeSpan.FromPublish = new DateTime(2014, 1, 1);
            model.TimeSpan.ToPublish = null;
            tasks = session.Query<Task>().Get(model).ToList();
            Assert.That(tasks.Count, Is.EqualTo(4));
            Contains(tasks, task_13);
            Contains(tasks, task_14);
            Contains(tasks, task_15);
            Contains(tasks, task_16);

            //both greater and less are not null
            model.TimeSpan.FromPublish = new DateTime(2013, 12, 31);
            model.TimeSpan.ToPublish = new DateTime(2014, 5, 1);
            tasks = session.Query<Task>().Get(model).ToList();
            Assert.That(tasks.Count, Is.EqualTo(4));
            Contains(tasks, task_12);
            Contains(tasks, task_13);
            Contains(tasks, task_14);
            Contains(tasks, task_15);
        }

        [Test]
        public void Get_By_Assign_DateTime()
        {
            task_11.AssignTime = null;
            task_12.AssignTime = new DateTime(2013, 12, 31, 23, 59, 59);
            task_13.AssignTime = new DateTime(2014, 1, 1, 0, 0, 1);
            task_14.AssignTime = new DateTime(2014, 1, 1, 6, 37, 42);
            task_15.AssignTime = new DateTime(2014, 1, 1, 9, 42, 0);
            task_16.AssignTime = new DateTime(2014, 5, 7);

            task_21.AssignTime = new DateTime(2014, 1, 1); ;

            save();
            set_Current_Project();

            //both from and to are null
            model.TimeSpan.FromAssign = null;
            model.TimeSpan.ToAssign = null;
            tasks = session.Query<Task>().Get(model).ToList();
            Assert.That(tasks.Count, Is.EqualTo(6));
            Contains(tasks, task_11);
            Contains(tasks, task_12);
            Contains(tasks, task_13);
            Contains(tasks, task_14);
            Contains(tasks, task_15);
            Contains(tasks, task_16);

            //only from is null
            model.TimeSpan.FromAssign = null;
            model.TimeSpan.ToAssign = new DateTime(2014, 1, 2);
            tasks = session.Query<Task>().Get(model).ToList();
            Assert.That(tasks.Count, Is.EqualTo(4));
            Contains(tasks, task_12);
            Contains(tasks, task_13);
            Contains(tasks, task_14);
            Contains(tasks, task_15);


            //only to is null
            model.TimeSpan.FromAssign = new DateTime(2014, 1, 1);
            model.TimeSpan.ToAssign = null;
            tasks = session.Query<Task>().Get(model).ToList();
            Assert.That(tasks.Count, Is.EqualTo(4));
            Contains(tasks, task_13);
            Contains(tasks, task_14);
            Contains(tasks, task_15);
            Contains(tasks, task_16);

            //both greater and less are not null
            model.TimeSpan.FromAssign = new DateTime(2013, 12, 31);
            model.TimeSpan.ToAssign = new DateTime(2014, 5, 1);
            tasks = session.Query<Task>().Get(model).ToList();
            Assert.That(tasks.Count, Is.EqualTo(4));
            Contains(tasks, task_12);
            Contains(tasks, task_13);
            Contains(tasks, task_14);
            Contains(tasks, task_15);
        }

        [Test]
        public void Get_By_Own_DateTime()
        {
            task_11.OwnTime = null;
            task_12.OwnTime = new DateTime(2013, 12, 31, 23, 59, 59);
            task_13.OwnTime = new DateTime(2014, 1, 1, 0, 0, 1);
            task_14.OwnTime = new DateTime(2014, 1, 1, 6, 37, 42);
            task_15.OwnTime = new DateTime(2014, 1, 1, 9, 42, 0);
            task_16.OwnTime = new DateTime(2014, 5, 7);

            task_21.OwnTime = new DateTime(2014, 1, 1);

            save();
            set_Current_Project();

            //both from and to are null
            model.TimeSpan.FromOwn = null;
            model.TimeSpan.ToOwn = null;
            tasks = session.Query<Task>().Get(model).ToList();
            Assert.That(tasks.Count, Is.EqualTo(6));
            Contains(tasks, task_11);
            Contains(tasks, task_12);
            Contains(tasks, task_13);
            Contains(tasks, task_14);
            Contains(tasks, task_15);
            Contains(tasks, task_16);

            //only from is null
            model.TimeSpan.FromOwn = null;
            model.TimeSpan.ToOwn = new DateTime(2014, 1, 1);
            tasks = session.Query<Task>().Get(model).ToList();
            Assert.That(tasks.Count, Is.EqualTo(1));
            Contains(tasks, task_12);

            //only to is null
            model.TimeSpan.FromOwn = new DateTime(2014, 1, 1);
            model.TimeSpan.ToOwn = null;
            tasks = session.Query<Task>().Get(model).ToList();
            Assert.That(tasks.Count, Is.EqualTo(4));
            Contains(tasks, task_13);
            Contains(tasks, task_14);
            Contains(tasks, task_15);
            Contains(tasks, task_16);

            //both greater and less are not null
            model.TimeSpan.FromOwn = new DateTime(2013, 12, 31);
            model.TimeSpan.ToOwn = new DateTime(2014, 5, 1);
            tasks = session.Query<Task>().Get(model).ToList();
            Assert.That(tasks.Count, Is.EqualTo(4));
            Contains(tasks, task_12);
            Contains(tasks, task_13);
            Contains(tasks, task_14);
            Contains(tasks, task_15);
        }

        [Test]
        public void Get_By_ActualComplete_DateTime()
        {
            task_11.ActualCompleteTime = null;
            task_12.ActualCompleteTime = new DateTime(2013, 12, 31);
            task_13.ActualCompleteTime = new DateTime(2014, 1, 1);
            task_14.ActualCompleteTime = new DateTime(2014, 1, 1, 6, 37, 42);
            task_15.ActualCompleteTime = new DateTime(2014, 1, 1, 9, 42, 0);
            task_16.ActualCompleteTime = new DateTime(2014, 5, 7);

            task_21.ActualCompleteTime = new DateTime(2014, 1, 1);

            save();
            set_Current_Project();

            //both from and to are null
            model.TimeSpan.FromActualComplete = null;
            model.TimeSpan.ToActualComplete = null;
            tasks = session.Query<Task>().Get(model).ToList();
            Assert.That(tasks.Count, Is.EqualTo(6));
            Contains(tasks, task_11);
            Contains(tasks, task_12);
            Contains(tasks, task_13);
            Contains(tasks, task_14);
            Contains(tasks, task_15);
            Contains(tasks, task_16);

            //only from is null
            model.TimeSpan.FromActualComplete = null;
            model.TimeSpan.ToActualComplete = new DateTime(2014, 1, 2);
            tasks = session.Query<Task>().Get(model).ToList();
            Assert.That(tasks.Count, Is.EqualTo(4));
            Contains(tasks, task_12);
            Contains(tasks, task_13);
            Contains(tasks, task_14);
            Contains(tasks, task_15);


            //only to is null
            model.TimeSpan.FromActualComplete = new DateTime(2014, 1, 1);
            model.TimeSpan.ToActualComplete = null;
            tasks = session.Query<Task>().Get(model).ToList();
            Assert.That(tasks.Count, Is.EqualTo(3));
            Contains(tasks, task_14);
            Contains(tasks, task_15);
            Contains(tasks, task_16);

            //both greater and less are not null
            model.TimeSpan.FromActualComplete = new DateTime(2013, 12, 31);
            model.TimeSpan.ToActualComplete = new DateTime(2014, 5, 1);
            tasks = session.Query<Task>().Get(model).ToList();
            Assert.That(tasks.Count, Is.EqualTo(3));
            Contains(tasks, task_13);
            Contains(tasks, task_14);
            Contains(tasks, task_15);
        }

        [Test]
        public void Get_By_LastestUpdateTime()
        {
            task_11.LatestUpdateTime = new DateTime(2013, 12, 1); ;
            task_12.LatestUpdateTime = new DateTime(2013, 12, 31);
            task_13.LatestUpdateTime = new DateTime(2014, 1, 1);
            task_14.LatestUpdateTime = new DateTime(2014, 1, 1, 6, 37, 42);
            task_15.LatestUpdateTime = new DateTime(2014, 1, 1, 9, 42, 0);
            task_16.LatestUpdateTime = new DateTime(2014, 5, 7);

            task_21.LatestUpdateTime = new DateTime(2014, 1, 1);

            save();
            set_Current_Project();

            //both from and to are null
            model.TimeSpan.FromLastestUpdateTime = null;
            model.TimeSpan.ToLastestUpdateTime = null;
            tasks = session.Query<Task>().Get(model).ToList();
            Assert.That(tasks.Count, Is.EqualTo(6));
            Contains(tasks, task_11);
            Contains(tasks, task_12);
            Contains(tasks, task_13);
            Contains(tasks, task_14);
            Contains(tasks, task_15);
            Contains(tasks, task_16);

            //only from is null
            model.TimeSpan.FromLastestUpdateTime = null;
            model.TimeSpan.ToLastestUpdateTime = new DateTime(2014, 1, 2);
            tasks = session.Query<Task>().Get(model).ToList();
            Assert.That(tasks.Count, Is.EqualTo(5));
            Contains(tasks, task_11);
            Contains(tasks, task_12);
            Contains(tasks, task_13);
            Contains(tasks, task_14);
            Contains(tasks, task_15);

            //only to is null
            model.TimeSpan.FromLastestUpdateTime = new DateTime(2014, 1, 1);
            model.TimeSpan.ToLastestUpdateTime = null;
            tasks = session.Query<Task>().Get(model).ToList();
            Assert.That(tasks.Count, Is.EqualTo(3));
            Contains(tasks, task_14);
            Contains(tasks, task_15);
            Contains(tasks, task_16);

            //both greater and less are not null
            model.TimeSpan.FromLastestUpdateTime = new DateTime(2013, 12, 31);
            model.TimeSpan.ToLastestUpdateTime = new DateTime(2014, 5, 1);
            tasks = session.Query<Task>().Get(model).ToList();
            Assert.That(tasks.Count, Is.EqualTo(3));
            Contains(tasks, task_13);
            Contains(tasks, task_14);
            Contains(tasks, task_15);
        }

        [Test]
        public void Get_By_SelectedPriorityLevel()
        {
            task_11.Priority = TaskPriority.Common;
            task_12.Priority = TaskPriority.High;
            task_13.Priority = TaskPriority.High;
            task_14.Priority = null;
            task_15.Priority = TaskPriority.Highest;
            task_16.Priority = TaskPriority.Low;

            task_21.Priority = TaskPriority.Lowest;

            save();
            set_Current_Project();

            model.SelectedPriority = null;
            tasks = session.Query<Task>().Get(model).ToList();
            Assert.That(tasks.Count, Is.EqualTo(6));
            Contains(tasks, task_11);
            Contains(tasks, task_12);
            Contains(tasks, task_13);
            Contains(tasks, task_14);
            Contains(tasks, task_15);
            Contains(tasks, task_16);

            model.SelectedPriority = TaskPriority.High;
            tasks = session.Query<Task>().Get(model).ToList();
            Assert.That(tasks.Count, Is.EqualTo(2));
            Contains(tasks, task_12);
            Contains(tasks, task_13);

            model.SelectedPriority = TaskPriority.Highest;
            tasks = session.Query<Task>().Get(model).ToList();
            Assert.That(tasks.Count, Is.EqualTo(1));
            Contains(tasks, task_15);
        }

        [Test]
        public void Get_By_SelectedDifficulty()
        {
            task_11.Difficulty = TaskDifficulty.Common;
            task_12.Difficulty = TaskDifficulty.Easy;
            task_13.Difficulty = TaskDifficulty.Easy;
            task_14.Difficulty = null;
            task_15.Difficulty = TaskDifficulty.Easiest;
            task_16.Difficulty = TaskDifficulty.Hard;

            task_21.Difficulty = TaskDifficulty.Hardest;

            save();
            set_Current_Project();

            model.SelectedDifficulty = null;
            tasks = session.Query<Task>().Get(model).ToList();
            Assert.That(tasks.Count, Is.EqualTo(6));
            Contains(tasks, task_11);
            Contains(tasks, task_12);
            Contains(tasks, task_13);
            Contains(tasks, task_14);
            Contains(tasks, task_15);
            Contains(tasks, task_16);

            model.SelectedDifficulty = TaskDifficulty.Easy;
            tasks = session.Query<Task>().Get(model).ToList();
            Assert.That(tasks.Count, Is.EqualTo(2));
            Contains(tasks, task_12);
            Contains(tasks, task_13);

            model.SelectedDifficulty = TaskDifficulty.Easiest;
            tasks = session.Query<Task>().Get(model).ToList();
            Assert.That(tasks.Count, Is.EqualTo(1));
            Contains(tasks, task_15);
        }

        [Test]
        public void Get_By_SelectedOwnerId()
        {
            User user_1 = new User();
            User user_2 = new User();
            User user_3 = new User();

            task_11.Owner = null;
            task_12.Owner = user_1;
            task_13.Owner = user_1;
            task_14.Owner = user_1;
            task_15.Owner = user_2;
            task_16.Owner = user_3;

            task_21.Owner = user_2;

            save();
            set_Current_Project();

            model.SelectedOwnerId = null;
            tasks = session.Query<Task>().Get(model).ToList();
            Assert.That(tasks.Count, Is.EqualTo(6));
            Contains(tasks, task_11);
            Contains(tasks, task_12);
            Contains(tasks, task_13);
            Contains(tasks, task_14);
            Contains(tasks, task_15);
            Contains(tasks, task_16);

            model.SelectedOwnerId = user_1.Id;
            tasks = session.Query<Task>().Get(model).ToList();
            Assert.That(tasks.Count, Is.EqualTo(3));
            Contains(tasks, task_12);
            Contains(tasks, task_13);
            Contains(tasks, task_14);

            model.SelectedOwnerId = user_2.Id;
            tasks = session.Query<Task>().Get(model).ToList();
            Assert.That(tasks.Count, Is.EqualTo(1));
            Contains(tasks, task_15);

            model.SelectedOwnerId = user_3.Id;
            tasks = session.Query<Task>().Get(model).ToList();
            Assert.That(tasks.Count, Is.EqualTo(1));
            Contains(tasks, task_16);
        }

        [Test]
        public void Get_By_SelectedAccepterId()
        {
            User user_1 = new User();
            User user_2 = new User();
            User user_3 = new User();

            task_11.Accepter = user_1;
            task_12.Accepter = user_1;
            task_13.Accepter = user_1;
            task_14.Accepter = user_1;
            task_15.Accepter = user_2;
            task_16.Accepter = user_3;

            task_21.Accepter = user_2;

            save();
            set_Current_Project();

            model.SelectedAccepterId = null;
            tasks = session.Query<Task>().Get(model).ToList();
            Assert.That(tasks.Count, Is.EqualTo(6));
            Contains(tasks, task_11);
            Contains(tasks, task_12);
            Contains(tasks, task_13);
            Contains(tasks, task_14);
            Contains(tasks, task_15);
            Contains(tasks, task_16);

            model.SelectedAccepterId = user_1.Id;
            tasks = session.Query<Task>().Get(model).ToList();
            Assert.That(tasks.Count, Is.EqualTo(4));
            Contains(tasks, task_11);
            Contains(tasks, task_12);
            Contains(tasks, task_13);
            Contains(tasks, task_14);

            model.SelectedAccepterId = user_2.Id;
            tasks = session.Query<Task>().Get(model).ToList();
            Assert.That(tasks.Count, Is.EqualTo(1));
            Contains(tasks, task_15);

            model.SelectedAccepterId = user_3.Id;
            tasks = session.Query<Task>().Get(model).ToList();
            Assert.That(tasks.Count, Is.EqualTo(1));
            Contains(tasks, task_16);
        }

        [Test]
        public void Get_By_SelectedPublisherId()
        {
            User user_1 = new User();
            User user_2 = new User();
            User user_3 = new User();

            task_11.Publisher = null;
            task_12.Publisher = user_1;
            task_13.Publisher = user_1;
            task_14.Publisher = user_1;
            task_15.Publisher = user_2;
            task_16.Publisher = user_3;

            task_21.Publisher = user_2;

            save();
            set_Current_Project();

            model.SelectedPublisherId = null;
            tasks = session.Query<Task>().Get(model).ToList();
            Assert.That(tasks.Count, Is.EqualTo(6));
            Contains(tasks, task_11);
            Contains(tasks, task_12);
            Contains(tasks, task_13);
            Contains(tasks, task_14);
            Contains(tasks, task_15);
            Contains(tasks, task_16);

            model.SelectedPublisherId = user_1.Id;
            tasks = session.Query<Task>().Get(model).ToList();
            Assert.That(tasks.Count, Is.EqualTo(3));
            Contains(tasks, task_12);
            Contains(tasks, task_13);
            Contains(tasks, task_14);

            model.SelectedPublisherId = user_2.Id;
            tasks = session.Query<Task>().Get(model).ToList();
            Assert.That(tasks.Count, Is.EqualTo(1));
            Contains(tasks, task_15);

            model.SelectedPublisherId = user_3.Id;
            tasks = session.Query<Task>().Get(model).ToList();
            Assert.That(tasks.Count, Is.EqualTo(1));
            Contains(tasks, task_16);
        }

        [Test]
        public void Get_By_SelectedStage()
        {
            task_11.CurrentStatus = Status.Pause;
            task_12.CurrentStatus = Status.Quit;
            task_13.CurrentStatus = Status.Quit;
            task_14.CurrentStatus = Status.Accept;
            task_15.CurrentStatus = Status.Accept;
            task_16.CurrentStatus = Status.Accept;

            task_21.CurrentStatus = Status.Quit;

            save();
            set_Current_Project();

            IList<Task> tasks;

            model.SelectedOwnerId = null;
            tasks = session.Query<Task>().Get(model).ToList();
            Assert.That(tasks.Count, Is.EqualTo(6));
            Contains(tasks, task_11);
            Contains(tasks, task_12);
            Contains(tasks, task_13);
            Contains(tasks, task_14);
            Contains(tasks, task_15);
            Contains(tasks, task_16);

            model.SelectedStage = (int)Status.Pause;
            tasks = session.Query<Task>().Get(model).ToList();
            Assert.That(tasks.Count, Is.EqualTo(1));
            Contains(tasks, task_11);

            model.SelectedStage = (int)Status.Quit;
            tasks = session.Query<Task>().Get(model).ToList();
            Assert.That(tasks.Count, Is.EqualTo(2));
            Contains(tasks, task_12);
            Contains(tasks, task_13);

            model.SelectedStage = (int)Status.Accept;
            tasks = session.Query<Task>().Get(model).ToList();
            Assert.That(tasks.Count, Is.EqualTo(3));
            Contains(tasks, task_14);
            Contains(tasks, task_15);
            Contains(tasks, task_16);

        }

        [Test]
        public void Get_By_SelectedStages()
        {
            task_11.CurrentStatus = Status.Pause;
            task_12.CurrentStatus = Status.Quit;
            task_13.CurrentStatus = Status.Quit;
            task_14.CurrentStatus = Status.Accept;
            task_15.CurrentStatus = Status.Accept;
            task_16.CurrentStatus = Status.Accept;

            task_21.CurrentStatus = Status.Quit;

            save();
            set_Current_Project();

            model.SelectedStages = new List<int>
            {
                (int)Status.Accept,
                (int)Status.Quit
            };
            tasks = session.Query<Task>().Get(model).ToList();
            Assert.That(tasks.Count, Is.EqualTo(5));
            Contains(tasks, task_12);
            Contains(tasks, task_13);
            Contains(tasks, task_14);
            Contains(tasks, task_15);
            Contains(tasks, task_16);

        }

        [Test]
        public void Get_By_SelectedQuality()
        {
            task_11.Quality = null;
            task_12.Quality = TaskQuality.Qualified;
            task_13.Quality = TaskQuality.Perfect;
            task_14.Quality = TaskQuality.Good;
            task_15.Quality = TaskQuality.Good;
            task_16.Quality = TaskQuality.Perfect;

            task_21.Quality = TaskQuality.Perfect;

            save();
            set_Current_Project();

            model.SelectedOwnerId = null;
            tasks = session.Query<Task>().Get(model).ToList();
            Assert.That(tasks.Count, Is.EqualTo(6));
            Contains(tasks, task_11);
            Contains(tasks, task_12);
            Contains(tasks, task_13);
            Contains(tasks, task_14);
            Contains(tasks, task_15);
            Contains(tasks, task_16);

            model.SelectedQuality = TaskQuality.Qualified;
            tasks = session.Query<Task>().Get(model).ToList();
            Assert.That(tasks.Count, Is.EqualTo(1));
            Contains(tasks, task_12);

            model.SelectedQuality = TaskQuality.Perfect;
            tasks = session.Query<Task>().Get(model).ToList();
            Assert.That(tasks.Count, Is.EqualTo(2));
            Contains(tasks, task_13);
            Contains(tasks, task_16);
        }

        [Test]
        public void Get_By_SelectedNodeType()
        {
            set_tasks_parent_and_children();

            save();
            set_Current_Project();

            model.SelectedNodeType = NodeType.Root;
            tasks = session.Query<Task>().Get(model).ToList();
            Assert.That(tasks.Count, Is.EqualTo(2));
            Contains(tasks, task_11);
            Contains(tasks, task_12);

            model.SelectedNodeType = NodeType.Branch;
            tasks = session.Query<Task>().Get(model).ToList();
            Assert.That(tasks.Count, Is.EqualTo(2));
            Contains(tasks, task_13);
            Contains(tasks, task_14);

            model.SelectedNodeType = NodeType.Leaf;
            tasks = session.Query<Task>().Get(model).ToList();
            Assert.That(tasks.Count, Is.EqualTo(2));
            Contains(tasks, task_15);
            Contains(tasks, task_16);
        }

        private void set_tasks_parent_and_children()
        {
            //task_11.NodeType = NodeType.Root;
            task_11.Parent = null;

            //task_12.NodeType = NodeType.Root;
            task_12.Parent = null;
            task_12.Children = new List<Task> { task_13, task_14 };

            //task_13.NodeType = NodeType.Branch;
            task_13.Parent = task_12;
            task_13.Children = new List<Task> { task_15 };
            //task_14.NodeType = NodeType.Branch;
            task_14.Parent = task_12;
            task_14.Children = new List<Task> { task_16 };

            //task_15.NodeType = NodeType.Leaf;
            task_15.Parent = task_13;
            //task_16.NodeType = NodeType.Leaf;
            task_16.Parent = task_14;
        }

        #endregion

        #region Combined Tests

        [Test]
        public void Get_By_Some_Conditions()
        {
            #region initialization task

            task_11.AssignTime = null;
            task_11.LatestUpdateTime = new DateTime(2014, 10, 1, 23, 59, 59);
            task_11.OverDue = null;
            task_11.Difficulty = null;

            task_12.AssignTime = new DateTime(2014, 10, 1, 0, 0, 1);
            task_12.LatestUpdateTime = new DateTime(2014, 10, 1, 23, 59, 59);
            task_12.OverDue = -5;
            task_12.Difficulty = TaskDifficulty.Hard;

            task_13.AssignTime = new DateTime(2014, 10, 1, 23, 59, 59);
            task_13.LatestUpdateTime = new DateTime(2014, 10, 5, 23, 59, 59);
            task_13.OverDue = 0;
            task_13.Difficulty = TaskDifficulty.Hard;

            task_14.AssignTime = new DateTime(2014, 10, 1, 0, 0, 1);
            task_14.LatestUpdateTime = new DateTime(2014, 10, 31, 23, 59, 59);
            task_14.OverDue = 5;
            task_14.Difficulty = TaskDifficulty.Hardest;

            task_15.AssignTime = new DateTime(2014, 10, 1, 23, 59, 59);
            task_15.LatestUpdateTime = new DateTime(2014, 11, 30, 0, 0, 1);
            task_15.OverDue = 5;
            task_15.Difficulty = TaskDifficulty.Hardest;

            task_16.AssignTime = new DateTime(2014, 10, 30, 23, 59, 59);
            task_16.LatestUpdateTime = new DateTime(2014, 11, 30, 23, 59, 59);
            task_16.OverDue = 10;
            task_16.Difficulty = TaskDifficulty.Hardest;

            #region interference task

            task_21.AssignTime = new DateTime(2014, 10, 1, 0, 0, 1);
            task_21.LatestUpdateTime = new DateTime(2014, 10, 30, 23, 59, 59);
            task_21.OverDue = 5;
            task_21.Difficulty = TaskDifficulty.Easiest;

            task_22.AssignTime = new DateTime(2014, 11, 1, 0, 0, 1);
            task_22.LatestUpdateTime = new DateTime(2014, 11, 30, 23, 59, 59);
            task_22.OverDue = 0;
            task_22.Difficulty = TaskDifficulty.Hardest;

            #endregion

            #endregion

            save();
            set_Current_Project();

            #region start test

            #region model attribute all is null

            model.TimeSpan.FromAssign = null;
            model.TimeSpan.ToAssign = null;
            model.TimeSpan.FromLastestUpdateTime = null;
            model.TimeSpan.ToLastestUpdateTime = null;
            model.LessOverDue = null;
            model.GreaterOverDue = null;
            model.SelectedDifficulty = null;
            tasks = session.Query<Task>().Get(model).ToList();
            Assert.That(tasks.Count, Is.EqualTo(6));
            Contains(tasks, task_11);
            Contains(tasks, task_12);
            Contains(tasks, task_13);
            Contains(tasks, task_14);
            Contains(tasks, task_15);
            Contains(tasks, task_16);

            #endregion

            #region set FormTime not null,set ToTime is null

            model.TimeSpan.FromAssign = new DateTime(2014, 10, 1);
            model.TimeSpan.ToAssign = null;
            model.TimeSpan.FromLastestUpdateTime = new DateTime(2014, 10, 31);
            model.TimeSpan.ToLastestUpdateTime = null;
            model.GreaterOverDue = 0;
            model.LessOverDue = 20;
            model.SelectedDifficulty = TaskDifficulty.Hardest;
            tasks = session.Query<Task>().Get(model).ToList();
            Assert.That(tasks.Count, Is.EqualTo(3));
            Contains(tasks, task_14);
            Contains(tasks, task_15);
            Contains(tasks, task_16);

            #endregion

            #region set FormTime null,set ToTime not null

            model.TimeSpan.FromAssign = null;
            model.TimeSpan.ToAssign = new DateTime(2014, 10, 30);
            model.TimeSpan.FromLastestUpdateTime = null;
            model.TimeSpan.ToLastestUpdateTime = new DateTime(2014, 11, 1);
            model.GreaterOverDue = -10;
            model.LessOverDue = 1;
            model.SelectedDifficulty = TaskDifficulty.Hard;
            tasks = session.Query<Task>().Get(model).ToList();
            Assert.That(tasks.Count, Is.EqualTo(2));
            Contains(tasks, task_12);
            Contains(tasks, task_13);

            #endregion

            #region set attribute all not null

            model.TimeSpan.FromAssign = new DateTime(2014, 10, 1);
            model.TimeSpan.ToAssign = new DateTime(2014, 12, 1);
            model.TimeSpan.FromLastestUpdateTime = new DateTime(2014, 10, 31);
            model.TimeSpan.ToLastestUpdateTime = new DateTime(2014, 11, 30, 23, 59, 59);
            model.GreaterOverDue = 1;
            model.LessOverDue = 20;
            model.SelectedDifficulty = TaskDifficulty.Hardest;
            tasks = session.Query<Task>().Get(model).ToList();
            Assert.That(tasks.Count, Is.EqualTo(2));
            Contains(tasks, task_14);
            Contains(tasks, task_15);

            #endregion

            #endregion
        }

        [Test]
        public void Get_By_All_Conditions()
        {
            #region initialization task

            User user_1 = new User();
            User user_2 = new User();
            //qualified
            Task task_1 = new Task
            {
                OverDue = 8,
                AssignTime = new DateTime(2014, 8, 15),
                OwnTime = new DateTime(2014, 8, 15),
                ExpectCompleteTime = new DateTime(2014, 8, 14),
                ActualCompleteTime = new DateTime(2014, 5, 3),
                LatestUpdateTime = new DateTime(2014, 6, 11),
                Difficulty = TaskDifficulty.Hardest,
                Owner = user_1,
                Accepter = user_1,
                Publisher = user_1,
                CurrentStatus = Status.Pause,
                Quality = TaskQuality.Good,
                Project = project_1,
                Priority = TaskPriority.Highest
            };
            task_1.MockActualWorkPeriod(12);
            task_1.MockCreateTime(new DateTime(2014, 3, 16));

            Task task_Greater_OverDue = new Task
            {
                OverDue = 0,
                AssignTime = new DateTime(2014, 8, 15),
                OwnTime = new DateTime(2014, 8, 15),
                ExpectCompleteTime = new DateTime(2014, 8, 14),
                ActualCompleteTime = new DateTime(2014, 5, 3),
                LatestUpdateTime = new DateTime(2014, 6, 11),
                Difficulty = TaskDifficulty.Hardest,
                Owner = user_1,
                Accepter = user_1,
                Publisher = user_1,
                CurrentStatus = Status.Pause,
                Quality = TaskQuality.Good,
                Project = project_1,
                Priority = TaskPriority.Highest
            };
            task_Greater_OverDue.MockActualWorkPeriod(12);
            task_Greater_OverDue.MockCreateTime(new DateTime(2014, 3, 16));

            Task task_Less_OverDue = new Task
            {
                OverDue = 20,
                AssignTime = new DateTime(2014, 8, 15),
                OwnTime = new DateTime(2014, 8, 15),
                ExpectCompleteTime = new DateTime(2014, 8, 14),
                ActualCompleteTime = new DateTime(2014, 5, 3),
                LatestUpdateTime = new DateTime(2014, 6, 11),
                Difficulty = TaskDifficulty.Hardest,
                Owner = user_1,
                Accepter = user_1,
                Publisher = user_1,
                CurrentStatus = Status.Pause,
                Quality = TaskQuality.Good,
                Project = project_1,
                Priority = TaskPriority.Highest
            };
            task_Less_OverDue.MockActualWorkPeriod(12);
            task_Less_OverDue.MockCreateTime(new DateTime(2014, 3, 16));

            Task task_Greater_WorkPeriod = new Task
            {
                OverDue = 8,
                AssignTime = new DateTime(2014, 8, 15),
                OwnTime = new DateTime(2014, 8, 15),
                ExpectCompleteTime = new DateTime(2014, 8, 14),
                ActualCompleteTime = new DateTime(2014, 5, 3),
                LatestUpdateTime = new DateTime(2014, 6, 11),
                Difficulty = TaskDifficulty.Hardest,
                Owner = user_1,
                Accepter = user_1,
                Publisher = user_1,
                CurrentStatus = Status.Pause,
                Quality = TaskQuality.Good,
                Project = project_1,
                Priority = TaskPriority.Highest
            };
            task_Greater_WorkPeriod.MockActualWorkPeriod(1);
            task_Greater_WorkPeriod.MockCreateTime(new DateTime(2014, 3, 16));
            Task task_Less_WorkPeriod = new Task
            {
                OverDue = 8,
                AssignTime = new DateTime(2014, 8, 15),
                OwnTime = new DateTime(2014, 8, 15),
                ExpectCompleteTime = new DateTime(2014, 8, 14),
                ActualCompleteTime = new DateTime(2014, 5, 3),
                LatestUpdateTime = new DateTime(2014, 6, 11),
                Difficulty = TaskDifficulty.Hardest,
                Owner = user_1,
                Accepter = user_1,
                Publisher = user_1,
                CurrentStatus = Status.Pause,
                Quality = TaskQuality.Good,
                Project = project_1,
                Priority = TaskPriority.Highest
            };
            task_Less_WorkPeriod.MockActualWorkPeriod(20);
            task_Less_WorkPeriod.MockCreateTime(new DateTime(2014, 3, 16));

            Task task_Greater_Publish = new Task
            {
                OverDue = 8,
                AssignTime = new DateTime(2014, 8, 15),
                OwnTime = new DateTime(2014, 8, 15),
                ExpectCompleteTime = new DateTime(2014, 8, 14),
                ActualCompleteTime = new DateTime(2014, 5, 3),
                LatestUpdateTime = new DateTime(2014, 6, 11),
                Difficulty = TaskDifficulty.Hardest,
                Owner = user_1,
                Accepter = user_1,
                Publisher = user_1,
                CurrentStatus = Status.Pause,
                Quality = TaskQuality.Good,
                Project = project_1,
                Priority = TaskPriority.Highest
            };
            task_Greater_Publish.MockCreateTime(new DateTime(2014, 2, 15));
            task_Greater_Publish.MockActualWorkPeriod(12);
            Task task_Less_Publish = new Task
            {
                OverDue = 8,
                AssignTime = new DateTime(2014, 8, 15),
                OwnTime = new DateTime(2014, 8, 15),
                ExpectCompleteTime = new DateTime(2014, 8, 14),
                ActualCompleteTime = new DateTime(2014, 5, 3),
                LatestUpdateTime = new DateTime(2014, 6, 11),
                Difficulty = TaskDifficulty.Hardest,
                Owner = user_1,
                Accepter = user_1,
                Publisher = user_1,
                CurrentStatus = Status.Pause,
                Quality = TaskQuality.Good,
                Project = project_1,
                Priority = TaskPriority.Highest
            };
            task_Less_Publish.MockCreateTime(new DateTime(2014, 5, 15));
            task_Less_Publish.MockCreateTime(new DateTime(2014, 3, 16));

            Task task_Greater_AssignTime = new Task
            {
                AssignTime = new DateTime(2014, 6, 16),
                OverDue = 8,
                OwnTime = new DateTime(2014, 8, 15),
                ExpectCompleteTime = new DateTime(2014, 8, 14),
                ActualCompleteTime = new DateTime(2014, 5, 3),
                LatestUpdateTime = new DateTime(2014, 6, 11),
                Difficulty = TaskDifficulty.Hardest,
                Owner = user_1,
                Accepter = user_1,
                Publisher = user_1,
                CurrentStatus = Status.Pause,
                Quality = TaskQuality.Good,
                Project = project_1,
                Priority = TaskPriority.Highest
            };
            task_Greater_AssignTime.MockActualWorkPeriod(12);
            task_Greater_AssignTime.MockCreateTime(new DateTime(2014, 3, 16));
            Task task_Less_AssignTime = new Task
            {
                AssignTime = new DateTime(2014, 10, 16),
                OverDue = 8,
                OwnTime = new DateTime(2014, 8, 15),
                ExpectCompleteTime = new DateTime(2014, 8, 14),
                ActualCompleteTime = new DateTime(2014, 5, 3),
                LatestUpdateTime = new DateTime(2014, 6, 11),
                Difficulty = TaskDifficulty.Hardest,
                Owner = user_1,
                Accepter = user_1,
                Publisher = user_1,
                CurrentStatus = Status.Pause,
                Quality = TaskQuality.Good,
                Project = project_1,
                Priority = TaskPriority.Highest
            };
            task_Less_AssignTime.MockActualWorkPeriod(12);
            task_Less_AssignTime.MockCreateTime(new DateTime(2014, 3, 16));

            Task task_From_OwnTime = new Task
            {
                OwnTime = new DateTime(2014, 6, 15),
                OverDue = 8,
                AssignTime = new DateTime(2014, 8, 15),
                ExpectCompleteTime = new DateTime(2014, 8, 14),
                ActualCompleteTime = new DateTime(2014, 5, 3),
                LatestUpdateTime = new DateTime(2014, 6, 11),
                Difficulty = TaskDifficulty.Hardest,
                Owner = user_1,
                Accepter = user_1,
                Publisher = user_1,
                CurrentStatus = Status.Pause,
                Quality = TaskQuality.Good,
                Project = project_1,
                Priority = TaskPriority.Highest
            };
            task_From_OwnTime.MockActualWorkPeriod(12);
            task_From_OwnTime.MockCreateTime(new DateTime(2014, 3, 16));
            Task task_To_OwnTime = new Task
            {
                OwnTime = new DateTime(2014, 11, 16),
                OverDue = 8,
                AssignTime = new DateTime(2014, 8, 15),
                ExpectCompleteTime = new DateTime(2014, 8, 14),
                ActualCompleteTime = new DateTime(2014, 5, 3),
                LatestUpdateTime = new DateTime(2014, 6, 11),
                Difficulty = TaskDifficulty.Hardest,
                Owner = user_1,
                Accepter = user_1,
                Publisher = user_1,
                CurrentStatus = Status.Pause,
                Quality = TaskQuality.Good,
                Project = project_1,
                Priority = TaskPriority.Highest
            };
            task_To_OwnTime.MockActualWorkPeriod(12);
            task_To_OwnTime.MockCreateTime(new DateTime(2014, 3, 16));

            Task task_From_ExpectCompleteTime = new Task
            {
                ExpectCompleteTime = new DateTime(2014, 6, 14),
                OverDue = 8,
                AssignTime = new DateTime(2014, 8, 15),
                OwnTime = new DateTime(2014, 8, 15),
                ActualCompleteTime = new DateTime(2014, 5, 3),
                LatestUpdateTime = new DateTime(2014, 6, 11),
                Difficulty = TaskDifficulty.Hardest,
                Owner = user_1,
                Accepter = user_1,
                Publisher = user_1,
                CurrentStatus = Status.Pause,
                Quality = TaskQuality.Good,
                Project = project_1,
                Priority = TaskPriority.Highest
            };
            task_From_ExpectCompleteTime.MockActualWorkPeriod(12);
            task_From_ExpectCompleteTime.MockCreateTime(new DateTime(2014, 3, 16));
            Task task_To_ExpectCompleteTime = new Task
            {
                ExpectCompleteTime = new DateTime(2014, 10, 6),
                OverDue = 8,
                AssignTime = new DateTime(2014, 8, 15),
                OwnTime = new DateTime(2014, 8, 15),
                ActualCompleteTime = new DateTime(2014, 5, 3),
                LatestUpdateTime = new DateTime(2014, 6, 11),
                Difficulty = TaskDifficulty.Hardest,
                Owner = user_1,
                Accepter = user_1,
                Publisher = user_1,
                CurrentStatus = Status.Pause,
                Quality = TaskQuality.Good,
                Project = project_1,
                Priority = TaskPriority.Highest
            };
            task_To_ExpectCompleteTime.MockActualWorkPeriod(12);
            task_To_ExpectCompleteTime.MockCreateTime(new DateTime(2014, 3, 16));

            Task task_From_ActualComplete = new Task
            {
                ActualCompleteTime = new DateTime(2014, 4, 20),
                OverDue = 8,
                AssignTime = new DateTime(2014, 8, 15),
                OwnTime = new DateTime(2014, 8, 15),
                ExpectCompleteTime = new DateTime(2014, 8, 14),
                LatestUpdateTime = new DateTime(2014, 6, 11),
                Difficulty = TaskDifficulty.Hardest,
                Owner = user_1,
                Accepter = user_1,
                Publisher = user_1,
                CurrentStatus = Status.Pause,
                Quality = TaskQuality.Good,
                Project = project_1,
                Priority = TaskPriority.Highest
            };
            task_From_ActualComplete.MockActualWorkPeriod(12);
            task_From_ActualComplete.MockCreateTime(new DateTime(2014, 3, 16));
            Task task_To_ActualComplete = new Task
            {
                ActualCompleteTime = new DateTime(2014, 6, 6),
                OverDue = 8,
                AssignTime = new DateTime(2014, 8, 15),
                OwnTime = new DateTime(2014, 8, 15),
                ExpectCompleteTime = new DateTime(2014, 8, 14),
                LatestUpdateTime = new DateTime(2014, 6, 11),
                Difficulty = TaskDifficulty.Hardest,
                Owner = user_1,
                Accepter = user_1,
                Publisher = user_1,
                CurrentStatus = Status.Pause,
                Quality = TaskQuality.Good,
                Project = project_1,
                Priority = TaskPriority.Highest
            };
            task_To_ActualComplete.MockActualWorkPeriod(12);
            task_To_ActualComplete.MockCreateTime(new DateTime(2014, 3, 16));

            Task task_From_LastestUpdateTime = new Task
            {
                LatestUpdateTime = new DateTime(2014, 5, 20),
                OverDue = 8,
                AssignTime = new DateTime(2014, 8, 15),
                OwnTime = new DateTime(2014, 8, 15),
                ExpectCompleteTime = new DateTime(2014, 8, 14),
                ActualCompleteTime = new DateTime(2014, 5, 3),
                Difficulty = TaskDifficulty.Hardest,
                Owner = user_1,
                Accepter = user_1,
                Publisher = user_1,
                CurrentStatus = Status.Pause,
                Quality = TaskQuality.Good,
                Project = project_1,
                Priority = TaskPriority.Highest
            };
            task_From_LastestUpdateTime.MockActualWorkPeriod(12);
            task_From_LastestUpdateTime.MockCreateTime(new DateTime(2014, 3, 16));

            Task task_To_LastestUpdateTime = new Task
            {
                LatestUpdateTime = new DateTime(2014, 7, 26),
                OverDue = 8,
                AssignTime = new DateTime(2014, 8, 15),
                OwnTime = new DateTime(2014, 8, 15),
                ExpectCompleteTime = new DateTime(2014, 8, 14),
                ActualCompleteTime = new DateTime(2014, 5, 3),
                Difficulty = TaskDifficulty.Hardest,
                Owner = user_1,
                Accepter = user_1,
                Publisher = user_1,
                CurrentStatus = Status.Pause,
                Quality = TaskQuality.Good,
                Project = project_1,
                Priority = TaskPriority.Highest
            };
            task_To_LastestUpdateTime.MockActualWorkPeriod(12);
            task_To_LastestUpdateTime.MockCreateTime(new DateTime(2014, 3, 16));

            Task task_not_SelectedPriorityLevel = new Task
            {
                Priority = TaskPriority.High,
                Difficulty = TaskDifficulty.Easiest,
                OverDue = 8,
                AssignTime = new DateTime(2014, 8, 15),
                OwnTime = new DateTime(2014, 8, 15),
                ExpectCompleteTime = new DateTime(2014, 8, 14),
                ActualCompleteTime = new DateTime(2014, 5, 3),
                LatestUpdateTime = new DateTime(2014, 6, 11),
                Owner = user_1,
                Accepter = user_1,
                Publisher = user_1,
                CurrentStatus = Status.Pause,
                Quality = TaskQuality.Good,
                Project = project_1
            };
            task_not_SelectedPriorityLevel.MockActualWorkPeriod(12);
            task_not_SelectedPriorityLevel.MockCreateTime(new DateTime(2014, 3, 16));

            Task task_not_SelectedDifficulty = new Task
            {
                Difficulty = TaskDifficulty.Easiest,
                OverDue = 8,
                AssignTime = new DateTime(2014, 8, 15),
                OwnTime = new DateTime(2014, 8, 15),
                ExpectCompleteTime = new DateTime(2014, 8, 14),
                ActualCompleteTime = new DateTime(2014, 5, 3),
                LatestUpdateTime = new DateTime(2014, 6, 11),
                Owner = user_1,
                Accepter = user_1,
                Publisher = user_1,
                CurrentStatus = Status.Pause,
                Quality = TaskQuality.Good,
                Project = project_1,
                Priority = TaskPriority.Highest
            };
            task_not_SelectedDifficulty.MockActualWorkPeriod(12);
            task_not_SelectedDifficulty.MockCreateTime(new DateTime(2014, 3, 16));

            Task task_not_SelectedOwnerId = new Task
            {
                Owner = user_2,
                OverDue = 8,
                AssignTime = new DateTime(2014, 8, 15),
                OwnTime = new DateTime(2014, 8, 15),
                ExpectCompleteTime = new DateTime(2014, 8, 14),
                ActualCompleteTime = new DateTime(2014, 5, 3),
                LatestUpdateTime = new DateTime(2014, 6, 11),
                Difficulty = TaskDifficulty.Hardest,
                Accepter = user_1,
                Publisher = user_1,
                CurrentStatus = Status.Pause,
                Quality = TaskQuality.Good,
                Project = project_1,
                Priority = TaskPriority.Highest
            };
            task_not_SelectedOwnerId.MockActualWorkPeriod(12);
            task_not_SelectedOwnerId.MockCreateTime(new DateTime(2014, 3, 16));

            Task task_not_SelectedAccepterId = new Task
            {
                OverDue = 8,
                AssignTime = new DateTime(2014, 8, 15),
                OwnTime = new DateTime(2014, 8, 15),
                ExpectCompleteTime = new DateTime(2014, 8, 14),
                ActualCompleteTime = new DateTime(2014, 5, 3),
                LatestUpdateTime = new DateTime(2014, 6, 11),
                Difficulty = TaskDifficulty.Hardest,
                Owner = user_1,
                Accepter = user_2,
                Publisher = user_1,
                CurrentStatus = Status.Pause,
                Quality = TaskQuality.Good,
                Project = project_1,
                Priority = TaskPriority.Highest
            };
            task_not_SelectedAccepterId.MockActualWorkPeriod(12);
            task_not_SelectedAccepterId.MockCreateTime(new DateTime(2014, 3, 16));

            Task task_not_SelectedPublisherId = new Task
            {
                OverDue = 8,
                AssignTime = new DateTime(2014, 8, 15),
                OwnTime = new DateTime(2014, 8, 15),
                ExpectCompleteTime = new DateTime(2014, 8, 14),
                ActualCompleteTime = new DateTime(2014, 5, 3),
                LatestUpdateTime = new DateTime(2014, 6, 11),
                Difficulty = TaskDifficulty.Hardest,
                Owner = user_1,
                Accepter = user_1,
                Publisher = user_2,
                CurrentStatus = Status.Pause,
                Quality = TaskQuality.Good,
                Project = project_1,
                Priority = TaskPriority.Highest
            };
            task_not_SelectedPublisherId.MockActualWorkPeriod(12);
            task_not_SelectedPublisherId.MockCreateTime(new DateTime(2014, 3, 16));

            Task task_not_SelectedStage = new Task
            {
                CurrentStatus = Status.Publish,
                OverDue = 8,
                AssignTime = new DateTime(2014, 8, 15),
                OwnTime = new DateTime(2014, 8, 15),
                ExpectCompleteTime = new DateTime(2014, 8, 14),
                ActualCompleteTime = new DateTime(2014, 5, 3),
                LatestUpdateTime = new DateTime(2014, 6, 11),
                Difficulty = TaskDifficulty.Hardest,
                Owner = user_1,
                Accepter = user_1,
                Publisher = user_1,
                Quality = TaskQuality.Good,
                Project = project_1,
                Priority = TaskPriority.Highest
            };
            task_not_SelectedStage.MockActualWorkPeriod(12);
            task_not_SelectedStage.MockCreateTime(new DateTime(2014, 3, 16));

            Task task_not_SelectedQuality = new Task
            {
                Quality = TaskQuality.Perfect,
                OverDue = 8,
                AssignTime = new DateTime(2014, 8, 15),
                OwnTime = new DateTime(2014, 8, 15),
                ExpectCompleteTime = new DateTime(2014, 8, 14),
                ActualCompleteTime = new DateTime(2014, 5, 3),
                LatestUpdateTime = new DateTime(2014, 6, 11),
                Difficulty = TaskDifficulty.Hardest,
                Owner = user_1,
                Accepter = user_1,
                Publisher = user_1,
                CurrentStatus = Status.Pause,
                Project = project_1,
                Priority = TaskPriority.Highest
            };
            task_not_SelectedQuality.MockActualWorkPeriod(12);
            task_not_SelectedQuality.MockCreateTime(new DateTime(2014, 3, 16));

            Task task_not_SelectedNodeType = new Task
            {
                OverDue = 8,
                AssignTime = new DateTime(2014, 8, 15),
                OwnTime = new DateTime(2014, 8, 15),
                ExpectCompleteTime = new DateTime(2014, 8, 14),
                ActualCompleteTime = new DateTime(2014, 5, 3),
                LatestUpdateTime = new DateTime(2014, 6, 11),
                Difficulty = TaskDifficulty.Hardest,
                Owner = user_1,
                Accepter = user_1,
                Publisher = user_1,
                CurrentStatus = Status.Pause,
                Quality = TaskQuality.Good,
                Project = project_1,
                Priority = TaskPriority.Highest
            };
            task_not_SelectedNodeType.Parent = new Task();
            task_not_SelectedNodeType.Children = null;
            task_not_SelectedNodeType.MockActualWorkPeriod(12);
            task_not_SelectedNodeType.MockCreateTime(new DateTime(2014, 3, 16));

            #endregion

            #region initialization session.Query<Task>()

            Save(
                task_1,
                task_Greater_OverDue,
                task_Less_OverDue,
                task_Greater_WorkPeriod,
                task_Less_WorkPeriod,
                task_Greater_Publish,
                task_Less_Publish,
                task_Greater_AssignTime,
                task_Less_AssignTime,
                task_From_OwnTime,
                task_To_OwnTime,
                task_From_ExpectCompleteTime,
                task_To_ExpectCompleteTime,
                task_From_ActualComplete,
                task_To_ActualComplete,
                task_From_LastestUpdateTime,
                task_To_LastestUpdateTime,
                task_not_SelectedPriorityLevel,
                task_not_SelectedDifficulty,
                task_not_SelectedOwnerId,
                task_not_SelectedAccepterId,
                task_not_SelectedPublisherId,
                task_not_SelectedStage,
                task_not_SelectedQuality,
                task_not_SelectedNodeType);

            #endregion

            set_Current_Project();

            #region initialization model

            model.GreaterOverDue = 3;
            model.LessOverDue = 10;

            model.GreaterWorkPeriod = 5;
            model.LessWorkPeriod = 17;

            model.TimeSpan.FromPublish = new DateTime(2014, 3, 15);
            model.TimeSpan.ToPublish = new DateTime(2014, 3, 18);

            model.TimeSpan.FromAssign = new DateTime(2014, 7, 15);
            model.TimeSpan.ToAssign = new DateTime(2014, 10, 15);

            model.TimeSpan.FromOwn = new DateTime(2014, 8, 1);
            model.TimeSpan.ToOwn = new DateTime(2014, 9, 1);

            model.TimeSpan.FromExpectComplete = new DateTime(2014, 8, 12);
            model.TimeSpan.ToExpectComplete = new DateTime(2014, 9, 11);

            model.TimeSpan.FromActualComplete = new DateTime(2014, 5, 1);
            model.TimeSpan.ToActualComplete = new DateTime(2014, 5, 9);

            model.TimeSpan.FromLastestUpdateTime = new DateTime(2014, 6, 9);
            model.TimeSpan.ToLastestUpdateTime = new DateTime(2014, 7, 15);

            model.SelectedPriority = TaskPriority.Highest;
            model.SelectedDifficulty = TaskDifficulty.Hardest;
            model.SelectedOwnerId = user_1.Id;
            model.SelectedAccepterId = user_1.Id;
            model.SelectedPublisherId = user_1.Id;
            model.SelectedStage = (int)Status.Pause;
            model.SelectedQuality = TaskQuality.Good;
            model.SelectedNodeType = NodeType.Root;

            #endregion

            tasks = session.Query<Task>().Get(model).ToList();
            Assert.That(tasks.Count, Is.EqualTo(1));
            Contains(tasks, task_1);
        }

        [Test]
        public void Get_By_Single_Multiple_Status()
        {
            task_11.CurrentStatus = Status.Pause;
            task_12.CurrentStatus = Status.Quit;
            task_13.CurrentStatus = Status.Quit;
            task_14.CurrentStatus = Status.Accept;
            task_15.CurrentStatus = Status.Accept;
            task_16.CurrentStatus = Status.Accept;

            task_21.CurrentStatus = Status.Quit;

            save();
            set_Current_Project();

            #region Only Single Status

            model.SelectedStage = (int)Status.Pause;
            tasks = session.Query<Task>().Get(model).ToList();
            Assert.That(tasks.Count, Is.EqualTo(1));
            Contains(tasks, task_11);

            #endregion

            #region Only Mulitiple Status

            model.SelectedStages = new List<int>
            {
                (int)Status.Accept,
                (int)Status.Quit
            };
            tasks = session.Query<Task>().Get(model).ToList();
            Assert.That(tasks.Count, Is.EqualTo(5));
            Contains(tasks, task_12);
            Contains(tasks, task_13);
            Contains(tasks, task_14);
            Contains(tasks, task_15);
            Contains(tasks, task_16);

            #endregion

            #region Both Single and Mulitiple Status

            model.SelectedStage = (int)Status.Pause;
            model.SelectedStages = new List<int>
            {
                (int)Status.Accept,
                (int)Status.Quit
            };
            tasks = session.Query<Task>().Get(model).ToList();
            Assert.That(tasks.Count, Is.EqualTo(5));
            Contains(tasks, task_12);
            Contains(tasks, task_13);
            Contains(tasks, task_14);
            Contains(tasks, task_15);
            Contains(tasks, task_16);

            #endregion
        }

        #endregion

        [Test]
        public void Charged_By_UserId()
        {
            User user_1 = new User();
            User user_2 = new User();
            User user_3 = new User();

            task_11.Owner = user_1;
            task_12.Owner = user_1;
            task_13.Accepter = user_1;
            task_14.Publisher = user_1;
            task_15.Owner = user_2;
            task_16.Publisher = user_3;

            task_21.Owner = user_2;

            save();
            int user_1_id = user_1.Id;
            int user_2_id = user_2.Id;
            int user_3_id = user_3.Id;

            model.SelectedOwnerId = null;
            tasks = session.Query<Task>().ChargedBy(user_1_id).ToList();
            Assert.That(tasks.Count, Is.EqualTo(4));
            Contains(tasks, task_11);
            Contains(tasks, task_12);
            Contains(tasks, task_13);
            Contains(tasks, task_14);
        }

        private void save()
        {
            Save(task_root_1,
                task_root_2,
                task_root_3,
                task_11,
                task_12,
                task_13,
                task_14,
                task_15,
                task_16,
                task_21,
                task_22,
                task_1_11,
                task_1_12,
                task_1_13);
        }

        private void set_Current_Project()
        {
            model.CurrentProject = new FFLTask.SRV.ViewModel.Project._DropdownlistLinkedModel
            {
                TailSelectedProject = new FFLTask.SRV.ViewModel.Project.LiteItemModel
                {
                    Id = project_1.Id
                }
            };
        }

        #region Sort

        [Test]
        public void Sort_By_ExpectedComplete()
        {
            Task task_null = new Task();
            Task task_2014_0101 = new Task { ExpectCompleteTime = new DateTime(2014, 1, 1) };
            Task task_2014_0401 = new Task { ExpectCompleteTime = new DateTime(2014, 4, 1) };

            Save(task_null,
                task_2014_0101,
                task_2014_0401);

            var result_des = session.Query<Task>().Sort(TaskList.Sort_By_ExpectedComplete, true).ToList();
            Assert.That(result_des[0].Id, Is.EqualTo(task_2014_0401.Id));
            Assert.That(result_des[1].Id, Is.EqualTo(task_2014_0101.Id));
            Assert.That(result_des[2].Id, Is.EqualTo(task_null.Id));

            var result_asc = session.Query<Task>().Sort(TaskList.Sort_By_ExpectedComplete, false).ToList();
            Assert.That(result_asc[0].Id, Is.EqualTo(task_null.Id));
            Assert.That(result_asc[1].Id, Is.EqualTo(task_2014_0101.Id));
            Assert.That(result_asc[2].Id, Is.EqualTo(task_2014_0401.Id));
        }

        [Test]
        public void Sort_By_ActualComplete()
        {
            Task task_null = new Task();
            Task task_2015_0101 = new Task { ActualCompleteTime = new DateTime(2015, 1, 1) };
            Task task_2015_0401 = new Task { ActualCompleteTime = new DateTime(2015, 4, 1) };

            Save(task_null,
                task_2015_0101,
                task_2015_0401);

            var result_des = session.Query<Task>().Sort(TaskList.Sort_By_ActualComplete, true).ToList();
            Assert.That(result_des[0].Id, Is.EqualTo(task_2015_0401.Id));
            Assert.That(result_des[1].Id, Is.EqualTo(task_2015_0101.Id));
            Assert.That(result_des[2].Id, Is.EqualTo(task_null.Id));

            var result_asc = session.Query<Task>().Sort(TaskList.Sort_By_ActualComplete, false).ToList();
            Assert.That(result_asc[0].Id, Is.EqualTo(task_null.Id));
            Assert.That(result_asc[1].Id, Is.EqualTo(task_2015_0101.Id));
            Assert.That(result_asc[2].Id, Is.EqualTo(task_2015_0401.Id));
        }

        [Test]
        public void Sort_By_ExpectedWorkPeriod()
        {
            Task task_null = new Task();
            Task task_0 = new Task { ExpectWorkPeriod = 0 };
            Task task_22 = new Task { ExpectWorkPeriod = 22 };
            Task task_66 = new Task { ExpectWorkPeriod = 66 };

            Save(task_null,
                task_0,
                task_22,
                task_66);

            var result_des = session.Query<Task>().Sort(TaskList.Sort_By_ExpectedWorkPeriod, true).ToList();
            Assert.That(result_des[0].Id, Is.EqualTo(task_66.Id));
            Assert.That(result_des[1].Id, Is.EqualTo(task_22.Id));
            Assert.That(result_des[2].Id, Is.EqualTo(task_0.Id));
            Assert.That(result_des[3].Id, Is.EqualTo(task_null.Id));

            var result_asc = session.Query<Task>().Sort(TaskList.Sort_By_ExpectedWorkPeriod, false).ToList();
            Assert.That(result_asc[0].Id, Is.EqualTo(task_null.Id));
            Assert.That(result_asc[1].Id, Is.EqualTo(task_0.Id));
            Assert.That(result_asc[2].Id, Is.EqualTo(task_22.Id));
            Assert.That(result_asc[3].Id, Is.EqualTo(task_66.Id));
        }

        [Test]
        public void Sort_By_ActualWorkPeriod()
        {
            Task task_null = new Task();
            Task task_0 = new Task();
            task_0.SetProperty("ActualWorkPeriod", 0);
            Task task_22 = new Task();
            task_22.SetProperty("ActualWorkPeriod", 22);
            Task task_66 = new Task();
            task_66.SetProperty("ActualWorkPeriod", 66);

            Save(task_0,
                task_null,
                task_22,
                task_66);

            var result_des = session.Query<Task>().Sort(TaskList.Sort_By_ActualWorkPeriod, true).ToList();
            Assert.That(result_des[0].Id, Is.EqualTo(task_66.Id));
            Assert.That(result_des[1].Id, Is.EqualTo(task_22.Id));
            Assert.That(result_des[2].Id, Is.EqualTo(task_0.Id));
            Assert.That(result_des[3].Id, Is.EqualTo(task_null.Id));

            var result_asc = session.Query<Task>().Sort(TaskList.Sort_By_ActualWorkPeriod, false).ToList();
            Assert.That(result_asc[0].Id, Is.EqualTo(task_null.Id));
            Assert.That(result_asc[1].Id, Is.EqualTo(task_0.Id));
            Assert.That(result_asc[2].Id, Is.EqualTo(task_22.Id));
            Assert.That(result_asc[3].Id, Is.EqualTo(task_66.Id));
        }

        [Test]
        public void Sort_By_Created()
        {
            Task task_2015_0101 = new Task();
            task_2015_0101.SetPropertyInBase("CreateTime", new DateTime(2015, 1, 1));
            Task task_2015_0401 = new Task();
            task_2015_0401.SetPropertyInBase("CreateTime", new DateTime(2015, 4, 1));

            Save(task_2015_0101,
                task_2015_0401);

            var result_des = session.Query<Task>().Sort(TaskList.Sort_By_Created, true).ToList();
            Assert.That(result_des[0].Id, Is.EqualTo(task_2015_0401.Id));
            Assert.That(result_des[1].Id, Is.EqualTo(task_2015_0101.Id));

            var result_asc = session.Query<Task>().Sort(TaskList.Sort_By_Created, false).ToList();
            Assert.That(result_asc[0].Id, Is.EqualTo(task_2015_0101.Id));
            Assert.That(result_asc[1].Id, Is.EqualTo(task_2015_0401.Id));
        }

        [Test]
        public void Sort_By_Assign()
        {
            Task task_null = new Task();
            Task task_2015_0101 = new Task { AssignTime = new DateTime(2015, 1, 1) };
            Task task_2015_0401 = new Task { AssignTime = new DateTime(2015, 4, 1) };

            Save(task_null,
                task_2015_0101,
                task_2015_0401);

            var result_des = session.Query<Task>().Sort(TaskList.Sort_By_Assign, true).ToList();
            Assert.That(result_des[0].Id, Is.EqualTo(task_2015_0401.Id));
            Assert.That(result_des[1].Id, Is.EqualTo(task_2015_0101.Id));
            Assert.That(result_des[2].Id, Is.EqualTo(task_null.Id));

            var result_asc = session.Query<Task>().Sort(TaskList.Sort_By_Assign, false).ToList();
            Assert.That(result_asc[0].Id, Is.EqualTo(task_null.Id));
            Assert.That(result_asc[1].Id, Is.EqualTo(task_2015_0101.Id));
            Assert.That(result_asc[2].Id, Is.EqualTo(task_2015_0401.Id));
        }

        [Test]
        public void Sort_By_Own()
        {
            Task task_null = new Task();
            Task task_2015_0101 = new Task { OwnTime = new DateTime(2015, 1, 1) };
            Task task_2015_0401 = new Task { OwnTime = new DateTime(2015, 4, 1) };

            Save(task_null,
                task_2015_0101,
                task_2015_0401);

            var result_des = session.Query<Task>().Sort(TaskList.Sort_By_Own, true).ToList();
            Assert.That(result_des[0].Id, Is.EqualTo(task_2015_0401.Id));
            Assert.That(result_des[1].Id, Is.EqualTo(task_2015_0101.Id));
            Assert.That(result_des[2].Id, Is.EqualTo(task_null.Id));

            var result_asc = session.Query<Task>().Sort(TaskList.Sort_By_Own, false).ToList();
            Assert.That(result_asc[0].Id, Is.EqualTo(task_null.Id));
            Assert.That(result_asc[1].Id, Is.EqualTo(task_2015_0101.Id));
            Assert.That(result_asc[2].Id, Is.EqualTo(task_2015_0401.Id));
        }

        [Test]
        public void Sort_By_LatestUpdate()
        {
            Task task_null = new Task();
            Task task_2015_0101 = new Task { LatestUpdateTime = new DateTime(2015, 1, 1) };
            Task task_2015_0401 = new Task { LatestUpdateTime = new DateTime(2015, 4, 1) };

            Save(task_null,
                task_2015_0101,
                task_2015_0401);

            var result_des = session.Query<Task>().Sort(TaskList.Sort_By_LatestUpdate, true).ToList();
            Assert.That(result_des[0].Id, Is.EqualTo(task_2015_0401.Id));
            Assert.That(result_des[1].Id, Is.EqualTo(task_2015_0101.Id));
            Assert.That(result_des[2].Id, Is.EqualTo(task_null.Id));

            var result_asc = session.Query<Task>().Sort(TaskList.Sort_By_LatestUpdate, false).ToList();
            Assert.That(result_asc[0].Id, Is.EqualTo(task_null.Id));
            Assert.That(result_asc[1].Id, Is.EqualTo(task_2015_0101.Id));
            Assert.That(result_asc[2].Id, Is.EqualTo(task_2015_0401.Id));
        }

        [Test]
        public void Sort_By_Priority()
        {
            Task task_null = new Task();
            Task task_common = new Task { Priority = TaskPriority.Common };
            Task task_highest = new Task { Priority = TaskPriority.Highest };
            Task task_high = new Task { Priority = TaskPriority.High };
            Task task_low = new Task { Priority = TaskPriority.Low };
            Task task_lowest = new Task { Priority = TaskPriority.Lowest };

            Save(task_null,
                task_common,
                task_high,
                task_highest,
                task_low,
                task_lowest);

            var result_des = session.Query<Task>().Sort(TaskList.Sort_By_Priority, true).ToList();
            Assert.That(result_des[0].Id, Is.EqualTo(task_highest.Id));
            Assert.That(result_des[1].Id, Is.EqualTo(task_high.Id));
            Assert.That(result_des[2].Id, Is.EqualTo(task_common.Id));
            Assert.That(result_des[3].Id, Is.EqualTo(task_low.Id));
            Assert.That(result_des[4].Id, Is.EqualTo(task_lowest.Id)); ;
            Assert.That(result_des[5].Id, Is.EqualTo(task_null.Id));

            var result_asc = session.Query<Task>().Sort(TaskList.Sort_By_Priority, false).ToList();
            Assert.That(result_asc[0].Id, Is.EqualTo(task_null.Id));
            Assert.That(result_asc[1].Id, Is.EqualTo(task_lowest.Id));
            Assert.That(result_asc[2].Id, Is.EqualTo(task_low.Id));
            Assert.That(result_asc[3].Id, Is.EqualTo(task_common.Id));
            Assert.That(result_asc[4].Id, Is.EqualTo(task_high.Id));
            Assert.That(result_asc[5].Id, Is.EqualTo(task_highest.Id));
        }

        [Test]
        public void Sort_By_OverDue()
        {
            Task task_null = new Task();
            Task task = new Task { OverDue = -2 };
            Task task_2 = new Task { OverDue = 2 };
            Task task_44 = new Task { OverDue = 44 };

            Save(task_null,
                task,
                task_2,
                task_44);

            var result_des = session.Query<Task>().Sort(TaskList.Sort_By_OverDue, true).ToList();
            Assert.That(result_des[0].Id, Is.EqualTo(task_44.Id));
            Assert.That(result_des[1].Id, Is.EqualTo(task_2.Id));
            Assert.That(result_des[2].Id, Is.EqualTo(task.Id));
            Assert.That(result_des[3].Id, Is.EqualTo(task_null.Id));

            var result_asc = session.Query<Task>().Sort(TaskList.Sort_By_OverDue, false).ToList();
            Assert.That(result_asc[0].Id, Is.EqualTo(task_null.Id));
            Assert.That(result_asc[1].Id, Is.EqualTo(task.Id));
            Assert.That(result_asc[2].Id, Is.EqualTo(task_2.Id));
            Assert.That(result_asc[3].Id, Is.EqualTo(task_44.Id));
        }

        [Test]
        public void Sort_By_Delay()
        {
            Task task_null = new Task();
            Task task = new Task { Delay = -6 };
            Task task_2 = new Task { Delay = 2 };
            Task task_44 = new Task { Delay = 44 };

            Save(task_null,
                task,
                task_2,
                task_44);

            var result_des = session.Query<Task>().Sort(TaskList.Sort_By_Delay, true).ToList();
            Assert.That(result_des[0].Id, Is.EqualTo(task_44.Id));
            Assert.That(result_des[1].Id, Is.EqualTo(task_2.Id));
            Assert.That(result_des[2].Id, Is.EqualTo(task.Id));
            Assert.That(result_des[3].Id, Is.EqualTo(task_null.Id));

            var result_asc = session.Query<Task>().Sort(TaskList.Sort_By_Delay, false).ToList();
            Assert.That(result_asc[0].Id, Is.EqualTo(task_null.Id));
            Assert.That(result_asc[1].Id, Is.EqualTo(task.Id));
            Assert.That(result_asc[2].Id, Is.EqualTo(task_2.Id));
            Assert.That(result_asc[3].Id, Is.EqualTo(task_44.Id));
        }

        #endregion
    }
}
