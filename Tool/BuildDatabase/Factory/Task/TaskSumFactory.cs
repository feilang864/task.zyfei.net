using FFLTask.BLL.Entity;
using FFLTask.GLB.Global.Enum;

namespace FFLTask.Tool.BuildDatabase
{
    public class TaskSumFactory
    {
        static Project root_project, child_project;
        static User user_1, user_2, user_3;

        internal static void Create()
        {
            create_user();

            Task task_1 = TaskFactory.CreateTask(root_project, user_1, user_3, "1");
            Task task_2 = TaskFactory.CreateTask(root_project, user_1, user_3, user_1, "2");
            Task task_3 = TaskFactory.CreateTask(root_project, user_1, user_3, user_1, "3");
            Task task_4 = TaskFactory.CreateTask(root_project, user_1, user_2, user_3, "4");
            Task task_5 = TaskFactory.CreateTask(root_project, user_1, user_2, user_3, "5");
            Task task_6 = TaskFactory.CreateTask(root_project, user_1, user_2, user_3, "6");
            Task task_7 = TaskFactory.CreateTask(root_project, user_1, user_2, user_3, "7");
            Task task_8 = TaskFactory.CreateTask(root_project, user_1, user_1, user_3, "8");
            Task task_9 = TaskFactory.CreateTask(root_project, user_1, user_1, user_3, "9");
            Task task_10 = TaskFactory.CreateTask(root_project, user_1, user_1, user_3, "10");
            Task task_11 = TaskFactory.CreateTask(root_project, user_2, user_1, user_3, "11");
            Task task_12 = TaskFactory.CreateTask(root_project, user_2, user_1, user_3, "12");
            Task task_13 = TaskFactory.CreateTask(root_project, user_2, user_1, user_3, "13");
            Task task_14 = TaskFactory.CreateTask(root_project, user_2, user_1, user_3, "14");
            Task task_15 = TaskFactory.CreateTask(root_project, user_2, user_1, user_3, "15");
            Task task_16 = TaskFactory.CreateTask(root_project, user_2, user_1, user_3, "16");
            Task task_17 = TaskFactory.CreateTask(root_project, user_2, user_1, user_3, "17");
            Task task_18 = TaskFactory.CreateTask(root_project, user_2, user_1, user_3, "18");
            Task task_19 = TaskFactory.CreateTask(root_project, user_2, user_1, user_3, "19");
            Task task_20 = TaskFactory.CreateTask(root_project, user_2, user_1, user_2, "20");
            Task task_21 = TaskFactory.CreateTask(child_project, user_2, user_1, user_2, "21");
            Task task_22 = TaskFactory.CreateTask(child_project, user_3, user_1, user_2, "22");

            task_1.IsVirtual = true;

            task_1.Priority = TaskPriority.Highest;
            task_2.Priority = TaskPriority.High;
            task_3.Priority = TaskPriority.High;
            task_4.Priority = TaskPriority.Common;
            task_5.Priority = TaskPriority.Common;
            task_6.Priority = TaskPriority.Common;
            task_7.Priority = TaskPriority.Low;
            task_8.Priority = TaskPriority.Low;
            task_9.Priority = TaskPriority.Low;
            task_10.Priority = TaskPriority.Low;
            task_11.Priority = TaskPriority.Lowest;
            task_12.Priority = TaskPriority.Lowest;
            task_13.Priority = TaskPriority.Lowest;
            task_14.Priority = TaskPriority.Lowest;
            task_15.Priority = TaskPriority.Lowest;

            task_1.Difficulty = TaskDifficulty.Hardest;
            task_2.Difficulty = TaskDifficulty.Hard;
            task_3.Difficulty = TaskDifficulty.Hard;
            task_4.Difficulty = TaskDifficulty.Common;
            task_5.Difficulty = TaskDifficulty.Common;
            task_6.Difficulty = TaskDifficulty.Common;
            task_7.Difficulty = TaskDifficulty.Easy;
            task_8.Difficulty = TaskDifficulty.Easy;
            task_9.Difficulty = TaskDifficulty.Easy;
            task_10.Difficulty = TaskDifficulty.Easy;
            task_11.Difficulty = TaskDifficulty.Easiest;
            task_12.Difficulty = TaskDifficulty.Easiest;
            task_13.Difficulty = TaskDifficulty.Easiest;
            task_14.Difficulty = TaskDifficulty.Easiest;
            task_15.Difficulty = TaskDifficulty.Easiest;

            task_2.Assign();
            task_3.Assign();

            task_4.Own();
            task_5.Own();
            task_6.Own();

            task_7.Assign();
            task_7.BeginWork();
            task_8.Assign();
            task_8.BeginWork();
            task_9.Assign();
            task_9.BeginWork();
            task_10.Assign();
            task_10.BeginWork();

            task_11.Assign();
            task_11.BeginWork();
            task_11.Pause();
            task_12.Assign();
            task_12.BeginWork();
            task_12.Pause();

            task_13.Assign();
            task_13.BeginWork();
            task_13.Complete();

            task_14.Assign();
            task_14.BeginWork();
            task_14.Quit();

            task_15.Assign();
            task_15.BeginWork();
            task_15.Doubt();
            task_15.UpdateProperty();

            task_16.Remove();

            task_17.Assign();
            task_17.BeginWork();
            task_17.Doubt();

            task_18.Assign();
            task_18.BeginWork();
            task_18.Complete();
            task_18.Accept(TaskQuality.Qualified);
            task_19.Assign();
            task_19.BeginWork();
            task_19.Complete();
            task_19.Accept(TaskQuality.Good);
            task_20.Assign();
            task_20.BeginWork();
            task_20.Complete();
            task_20.Accept(TaskQuality.Good);
            task_21.Assign();
            task_21.BeginWork();
            task_21.Complete();
            task_21.Accept(TaskQuality.Perfect);

            task_22.Assign();
            task_22.BeginWork();
            task_22.Complete();
            task_22.RefuseAccept();
            task_22.Complete();
            task_22.RefuseAccept();

            task_18.ExpectWorkPeriod = 20;
            task_18.MockActualWorkPeriod(35);
            task_18.OverDue = 15;

            task_22.OverDue = 35;

        }

        private static void create_user()
        {
            user_1 = UserFactory.create("统计测试员-A");
            user_2 = UserFactory.create("统计测试员-B");
            user_3 = UserFactory.create("统计测试员-C");

            root_project = ProjectFactory.create(user_1, "统计测试项目根项目", string.Empty, null);
            child_project = ProjectFactory.create(user_1, "统计测试项目子项目", string.Empty, root_project);
            user_2.Join(root_project);
            user_2.Join(child_project);
            user_3.Join(root_project);
            user_3.Join(child_project);

            AuthorizationFactory.SetAuth(user_2, root_project, false, true, true);
            AuthorizationFactory.SetAuth(user_2, child_project, false, true, true);
            AuthorizationFactory.SetAuth(user_3, root_project, false, true, true);
            AuthorizationFactory.SetAuth(user_3, child_project, false, true, true);
        }
    }
}
