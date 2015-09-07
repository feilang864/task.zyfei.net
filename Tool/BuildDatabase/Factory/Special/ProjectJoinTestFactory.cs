using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FFLTask.BLL.Entity;

namespace FFLTask.Tool.BuildDatabase.Factory.Special
{
    public class ProjectJoinTestFactory
    {
        private static User founder;
        private static User project_join_test_user;

        private static Project renwuguanli;
        private static Project meigong;
        private static Project ui;
        private static Project houtai;
        private static Project csharp;
        private static Project dba;

        internal static void Create()
        {
            create_users();
            create_project();
            join();
        }

        private static void join()
        {
            project_join_test_user.Join(dba);
        }

        private static void create_project()
        {
            renwuguanli = ProjectFactory.create(founder, "任务管理", "测试ProjectJoin页面流程用的项目", null);
            meigong = ProjectFactory.create(founder, "美工", "测试ProjectJoin页面流程用的项目", renwuguanli);
            ui = ProjectFactory.create(founder, "UI", "测试ProjectJoin页面流程用的项目", renwuguanli);
            houtai = ProjectFactory.create(founder, "后台", "测试ProjectJoin页面流程用的项目", renwuguanli);
            csharp = ProjectFactory.create(founder, "C#", "测试EdProjectJoinit页面流程用的项目", houtai);
            dba = ProjectFactory.create(founder, "DBA", "测试EdProjectJoinit页面流程用的项目", houtai);
        }

        private static void create_users()
        {
            founder = UserFactory.create("项目创始人");
            project_join_test_user = UserFactory.create("项目加入测试人员");
        }
    }
}
