using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FFLTask.BLL.Entity;
using FFLTask.GLB.Global.Enum;

namespace FFLTask.Tool.BuildDatabase
{
    class TaskEditProcessTestFactory
    {
        private static User publish_a;
        private static User publish_b;
        private static User own_a;
        private static User third_a;
        private static User third_b;

        private static Project project;

        internal static void Create()
        {
            create_users();
            create_project(publish_b);
            join();
            auth();
        }

        private static void auth()
        {
            AuthorizationFactory.SetAuth(publish_a, project, true, true, false);
            AuthorizationFactory.SetAuth(own_a, project, false, false, true);
            AuthorizationFactory.SetAuth(third_a, project, false, false, false);
            AuthorizationFactory.SetAuth(third_b, project, false, false, true);
        }

        private static void join()
        {
            publish_a.Join(project);
            own_a.Join(project);
            third_a.Join(project);
            third_b.Join(project);
        }

        private static void create_project(User creator)
        {
            project = ProjectFactory.create(creator, "TaskEdit流程测试", "测试Edit页面流程用的项目", null);
        }

        private static void create_users()
        {
            publish_a = UserFactory.create("发布人A");
            publish_b = UserFactory.create("发布人B");
            own_a = UserFactory.create("承接人A");
            third_a = UserFactory.create("第三人A");
            third_b = UserFactory.create("第三人B");
        }
    }
}
