using FFLTask.BLL.Entity;
using FFLTask.GLB.Global.Enum;
using NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FFLTask.Tool.BuildDatabase
{
    class SearchFactory
    {
        private static readonly ISession session = NHProvider.session;

        internal static void Create()
        {
            Project project_1 = new Project { Name = "成员检索项目A" };
            Project project_2 = new Project { Name = "成员检索项目B" };
            Project project_3 = new Project { Name = "成员检索项目C" };

            User user_1 = UserFactory.create("成员检索测试员-A");
            AuthorizationFactory.SetAuth(user_1, project_1, false, true, true);
            AuthorizationFactory.SetAuth(user_1, project_2, false, true, true);
            AuthorizationFactory.SetAuth(user_1, project_3, false, true, true);
            User user_2 = UserFactory.create("成员检索测试员-B");
            AuthorizationFactory.SetAuth(user_2, project_1, true, false, true);

            Task task_1 = TaskFactory.CreateTask(project_1,
                publisher: user_1,
                title: "task_1");
            task_1.CurrentStatus = Status.Accept;

            Task task_2 = TaskFactory.CreateTask(project_1,
                owner: user_1,
                title: "task_2"
                );
            task_2.CurrentStatus = Status.Accept;

            Task task_3 = TaskFactory.CreateTask(project_2,
                owner: user_1,
                title: "task_3"
                );
            task_3.CurrentStatus = Status.Accept;

            Task task_3_1 = TaskFactory.CreateTask(project_2,
                owner: user_1,
                title: "task_3_1");
            task_3_1.CurrentStatus = Status.BeginWork;

            Task task_4 = TaskFactory.CreateTask(project_3,
                owner: user_1,
                title: "task_4");
            task_4.CurrentStatus = Status.Pause;

            Task task_5 = TaskFactory.CreateTask(project_1,
                accepter: user_1,
                title: "task_5");
            task_5.CurrentStatus = Status.Remove;

            Task task_6 = TaskFactory.CreateTask(project_2,
                accepter: user_1,
                title: "task_6");
            task_6.CurrentStatus = Status.Dissent;

            Task task_7 = TaskFactory.CreateTask(project_1,
                owner: user_2,
                title: "task_7");
            task_7.CurrentStatus = Status.BeginWork;

            Task task_8 = TaskFactory.CreateTask(project_1,
                owner: user_2,
                title: "task_8");
            task_8.CurrentStatus = Status.Accept;
        }
    }
}
