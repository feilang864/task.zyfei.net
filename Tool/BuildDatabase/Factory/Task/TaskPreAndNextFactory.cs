using FFLTask.BLL.Entity;

namespace FFLTask.Tool.BuildDatabase
{
    class TaskPreAndNextFactory
    {
        private static Project project;

        private static User user;

        private static Task parent_task;
        private static Task first_task;
        private static Task center_task;
        private static Task last_task;

        internal static void Create()
        {
            create_user_and_project();
            create_task();
            set_task_relation();
        }

        private static void create_user_and_project()
        {
            user = UserFactory.create("PreNxt-用户");
            project = ProjectFactory.create(user, "上下任务所属项目", string.Empty, null);
        }

        private static void create_task()
        {
            parent_task = TaskFactory.CreateTask(project, user, user, "PreNxt-父任务");
            first_task = TaskFactory.CreateTask(project, user, user, "PreNxt-首任务");
            center_task = TaskFactory.CreateTask(project, user, user, "PreNxt-中间任务");
            last_task = TaskFactory.CreateTask(project, user, user, "PreNxt-尾任务");
        }

        private static void set_task_relation()
        {
            parent_task.AddChild(first_task);
            parent_task.AddChild(center_task);
            parent_task.AddChild(last_task);
        }
    }
}
