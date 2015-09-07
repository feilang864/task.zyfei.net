using FFLTask.BLL.Entity;
using NHibernate;

namespace FFLTask.Tool.BuildDatabase
{
    class TaskRelationFactory
    {
        private static readonly ISession session = NHProvider.session;

        private static User publisher;
        private static Project project;

        internal static void Create()
        {
            create_user();
            create_project();
            create_task();
        }

        private static void create_user()
        {
            publisher = UserFactory.create("任务关系测试人");
        }

        private static void create_project()
        {
            project = ProjectFactory.create(publisher, "任务关系测试项目", string.Empty, null);
        }

        private static void create_task()
        {
            Task root = TaskFactory.CreateTask(project, publisher, publisher, publisher, "根任务");
            Task parent_parent = TaskFactory.CreateTask(project, publisher, publisher, publisher, "当前任务爷任务");
            Task parent = TaskFactory.CreateTask(project, publisher, publisher, publisher, "当前任务父任务");
            Task current = TaskFactory.CreateTask(project, publisher, publisher, publisher, "当前任务");
            Task brother_1 = TaskFactory.CreateTask(project, publisher, publisher, publisher, "当前任务兄弟-1");
            Task brother_2 = TaskFactory.CreateTask(project, publisher, publisher, publisher, "当前任务兄弟-2");
            Task child_1 = TaskFactory.CreateTask(project, publisher, publisher, publisher, "当前任务子任务-1");
            Task child_2 = TaskFactory.CreateTask(project, publisher, publisher, publisher, "当前任务子任务-2");
            Task child_3 = TaskFactory.CreateTask(project, publisher, publisher, publisher, "当前任务子任务-3");

            root.AddChild(parent_parent);
            parent_parent.AddChild(parent);
            parent.AddChild(brother_1);
            parent.AddChild(current);
            parent.AddChild(brother_2);

            current.AddChild(child_1);
            current.AddChild(child_2);
            current.AddChild(child_3);

            brother_1.Assign();
            brother_1.BeginWork();
            brother_1.Doubt();

            current.Assign();

            brother_2.Assign();
            brother_2.Quit();

            child_1.Remove();

            child_2.Assign();
            child_2.BeginWork();
            child_2.Pause();

            child_3.Assign();
            child_3.BeginWork();
        }
    }
}
