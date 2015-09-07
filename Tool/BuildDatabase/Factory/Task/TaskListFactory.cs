using System;
using FFLTask.BLL.Entity;
using FFLTask.GLB.Global.Enum;
using Global.Core.ExtensionMethod;

namespace FFLTask.Tool.BuildDatabase
{
    public class TaskListFactory
    {
        private static User admin,
            publisher,
            owner,
            accepter;

        private static Project parent_project, child_project;

        internal static void Create()
        {
            create_user();
            create_project();
            join_project();
            set_auth();
            create_tasks();
        }

        private static void create_user()
        {
            admin = UserFactory.create("任务列表测试管理员");
            publisher = UserFactory.create("任务列表测试发布人");
            owner = UserFactory.create("任务列表测试承接人");
            accepter = UserFactory.create("任务列表测试验收人");
        }

        private static void create_project()
        {
            parent_project = ProjectFactory.create(admin, "任务列表测试父项目", string.Empty, null);
            child_project = ProjectFactory.create(admin, "任务列表测试子项目", string.Empty, parent_project);
        }

        private static void join_project()
        {
            publisher.Join(parent_project);
            publisher.Join(child_project);
            owner.Join(parent_project);
            owner.Join(child_project);
            accepter.Join(parent_project);
            accepter.Join(child_project);
        }

        private static void set_auth()
        {
            AuthorizationFactory.SetAuth(publisher, parent_project, false, true, false);
            AuthorizationFactory.SetAuth(publisher, child_project, false, true, false);
            AuthorizationFactory.SetAuth(owner, parent_project, false, false, true);
            AuthorizationFactory.SetAuth(owner, child_project, false, false, true);
        }

        private static void create_tasks()
        {
            Task task_1 = TaskFactory.CreateTask(parent_project, admin, accepter, "任务列表测试任务_1");
            Task task_2 = TaskFactory.CreateTask(parent_project, admin, accepter, "任务列表测试任务_2");
            Task task_3 = TaskFactory.CreateTask(parent_project, admin, accepter, "任务列表测试任务_3");
            Task task_4 = TaskFactory.CreateTask(parent_project, admin, accepter, "任务列表测试任务_4");
            Task task_5 = TaskFactory.CreateTask(parent_project, publisher, accepter, "任务列表测试任务_5");
            Task task_6 = TaskFactory.CreateTask(parent_project, publisher, accepter, "任务列表测试任务_6");
            Task task_7 = TaskFactory.CreateTask(parent_project, publisher, accepter, "任务列表测试任务_7");
            Task task_8 = TaskFactory.CreateTask(parent_project, publisher, accepter, "任务列表测试任务_8");
            Task task_9 = TaskFactory.CreateTask(parent_project, publisher, accepter, "任务列表测试任务_9");
            Task task_10 = TaskFactory.CreateTask(parent_project, publisher, accepter, "任务列表测试任务_10");
            Task task_11 = TaskFactory.CreateTask(child_project, publisher, accepter, "任务列表测试任务_11");
            Task task_12 = TaskFactory.CreateTask(child_project, publisher, accepter, owner, "任务列表测试任务_12");
            Task task_13 = TaskFactory.CreateTask(child_project, publisher, accepter, owner, "任务列表测试任务_13");
            Task task_14 = TaskFactory.CreateTask(child_project, publisher, accepter, owner, "任务列表测试任务_14");
            Task task_15 = TaskFactory.CreateTask(child_project, publisher, accepter, owner, "任务列表测试任务_15");
            Task task_16 = TaskFactory.CreateTask(child_project, publisher, accepter, owner, "任务列表测试任务_16");
            Task task_17 = TaskFactory.CreateTask(child_project, publisher, accepter, owner, "任务列表测试任务_17");
            Task task_18 = TaskFactory.CreateTask(child_project, publisher, accepter, owner, "任务列表测试任务_18");
            Task task_19 = TaskFactory.CreateTask(child_project, publisher, accepter, owner, "任务列表测试任务_19");
            Task task_20 = TaskFactory.CreateTask(child_project, publisher, accepter, owner, "任务列表测试任务_20");
            Task task_21 = TaskFactory.CreateTask(child_project, publisher, accepter, admin, "任务列表测试任务_21");
            Task task_22 = TaskFactory.CreateTask(child_project, publisher, admin, owner, "任务列表测试任务_22");
            Task task_23 = TaskFactory.CreateTask(child_project, publisher, accepter, owner, "任务列表测试任务_23");
            Task task_24 = TaskFactory.CreateTask(child_project, publisher, accepter, owner, "任务列表测试任务_24");
            Task task_25 = TaskFactory.CreateTask(child_project, publisher, accepter, owner, "任务列表测试任务_25");

            task_1.AddChild(task_2);
            task_2.AddChild(task_3);
            task_2.AddChild(task_4);

            task_5.Priority = TaskPriority.Low;
            task_6.Priority = TaskPriority.Common;
            task_7.Priority = TaskPriority.Common;
            task_8.Priority = TaskPriority.High;
            task_9.Priority = TaskPriority.High;
            task_10.Priority = TaskPriority.High;
            task_11.Priority = TaskPriority.High;
            task_12.Priority = TaskPriority.High;
            task_13.Priority = TaskPriority.High;
            task_14.Priority = TaskPriority.High;
            task_15.Priority = TaskPriority.High;
            task_16.Priority = TaskPriority.High;
            task_17.Priority = TaskPriority.High;
            task_18.Priority = TaskPriority.High;
            task_19.Priority = TaskPriority.High;
            task_20.Priority = TaskPriority.High;
            task_21.Priority = TaskPriority.High;
            task_22.Priority = TaskPriority.High;
            task_23.Priority = TaskPriority.High;
            task_24.Priority = TaskPriority.High;
            task_25.Priority = TaskPriority.High;

            task_8.Difficulty = TaskDifficulty.Hard;
            task_9.Difficulty = TaskDifficulty.Common;
            task_10.Difficulty = TaskDifficulty.Common;
            task_11.Difficulty = TaskDifficulty.Easy;
            task_12.Difficulty = TaskDifficulty.Easy;
            task_13.Difficulty = TaskDifficulty.Easy;
            task_14.Difficulty = TaskDifficulty.Easy;
            task_15.Difficulty = TaskDifficulty.Easy;
            task_16.Difficulty = TaskDifficulty.Easy;
            task_17.Difficulty = TaskDifficulty.Easy;
            task_18.Difficulty = TaskDifficulty.Easy;
            task_19.Difficulty = TaskDifficulty.Easy;
            task_20.Difficulty = TaskDifficulty.Easy;
            task_21.Difficulty = TaskDifficulty.Easy;
            task_22.Difficulty = TaskDifficulty.Easy;
            task_23.Difficulty = TaskDifficulty.Easy;
            task_24.Difficulty = TaskDifficulty.Easy;
            task_25.Difficulty = TaskDifficulty.Easy;

            set_task_status(task_12, Status.BeginWork);
            set_task_status(task_13, Status.BeginWork);
            set_task_status(task_14, Status.Remove);
            set_task_status(task_15, Status.Remove);
            set_task_status(task_16, Status.Remove);
            set_task_status(task_17, Status.Complete);
            set_task_status(task_18, Status.Complete);
            set_task_status(task_19, Status.Complete);
            set_task_status(task_20, Status.Complete);
            set_task_status(task_21, Status.Accept);
            set_task_status(task_22, Status.Accept);
            set_task_status(task_23, Status.Accept);
            set_task_status(task_24, Status.Accept);
            set_task_status(task_25, Status.Accept);

            task_23.Quality = TaskQuality.Qualified;
            task_24.Quality = TaskQuality.Good;
            task_25.Quality = TaskQuality.Good;

            DateTime baseTime = DateTime.Now.AddDays(-7);
            DateTime time_1 = baseTime;
            DateTime time_2 = time_1.AddMinutes(15);
            DateTime time_3 = time_2.AddMinutes(24);

            task_23.SetPropertyInBase("CreateTime", time_2);
            task_24.SetPropertyInBase("CreateTime", time_3);
            task_25.SetPropertyInBase("CreateTime", time_1);

            task_23.AssignTime = time_1;
            task_24.AssignTime = time_2;
            task_25.AssignTime = time_3;

            task_23.OwnTime = time_2;
            task_24.OwnTime = time_1;
            task_25.OwnTime = time_3;

            task_23.ExpectCompleteTime = time_1;
            task_24.ExpectCompleteTime = time_3;
            task_25.ExpectCompleteTime = time_2;

            task_23.ActualCompleteTime = time_2;
            task_24.ActualCompleteTime = time_1;
            task_25.ActualCompleteTime = time_3;

            task_23.LatestUpdateTime = time_3;
            task_24.LatestUpdateTime = time_2;
            task_25.LatestUpdateTime = time_1;

            task_23.Delay = 2;
            task_24.Delay = 22;
            task_25.Delay = 4;

            task_23.OverDue = 27;
            task_24.OverDue = 6;
            task_25.OverDue = 9;

            task_23.ExpectWorkPeriod = 11;
            task_24.ExpectWorkPeriod = 33;
            task_25.ExpectWorkPeriod = 10;

            task_23.SetProperty("ActualWorkPeriod", 4);
            task_24.SetProperty("ActualWorkPeriod", 5);
            task_25.SetProperty("ActualWorkPeriod", 2);
        }

        private static void set_task_status(Task task, Status status)
        {
            switch (status)
            {
                case Status.Assign:
                    task.Assign();
                    break;
                case Status.Remove:
                    task.Remove();
                    break;
                case Status.BeginWork:
                    task.Assign();
                    task.BeginWork();
                    break;
                case Status.Complete:
                    task.Assign();
                    task.BeginWork();
                    task.Complete();
                    break;
                case Status.Accept:
                    task.Assign();
                    task.BeginWork();
                    task.Complete();
                    task.Accept(null);
                    break;
            }
        }
    }
}
