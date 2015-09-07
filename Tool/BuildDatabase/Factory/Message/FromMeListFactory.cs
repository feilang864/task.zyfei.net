using System;
using System.Linq;
using FFLTask.BLL.Entity;
using Global.Core.Helper;

namespace FFLTask.Tool.BuildDatabase
{
    class FromMeListFactory
    {
        static User addresser, addressee_A, addressee_B, addressee_C;

        internal static void Create()
        {
            SystemTime.SetDateTime(DateTime.Now.AddDays(-10));

            create_users();
            join_project();
            set_auth();

            Task task_1 = TaskFactory.CreateTask(ProjectFactory.rwgl, addresser, addressee_A, "引入留言功能");
            Task task_2 = TaskFactory.CreateTask(ProjectFactory.rwgl, addresser, addressee_B, "在BuildDatabase中准备好数据");
            Task task_3 = TaskFactory.CreateTask(ProjectFactory.jicheng, addresser, addressee_C, "实现统计的NHQuery方法");

            SystemTime.SetDateTime(DateTime.Now.AddDays(-9));
            task_1.Comment(addresser, addressee_A, "1-验收时注意区分纯留言");
            task_1.Comment(addresser, addressee_A, "2-验收时注意纯留言");
            task_1.Comment(addresser, addressee_A, "3-验收时注意区分纯留言");
            task_1.Comment(addresser, addressee_A, "4-注意分纯留言");
            task_1.Comment(addresser, addressee_A, "5-验收注意区分纯留言");
            task_1.Comment(addresser, addressee_A, "6-验收时注意区分纯留言");
            task_1.Comment(addresser, addressee_A, "7-验收时注意纯留言");
            task_1.Comment(addresser, addressee_A, "8-验收时注意区分纯留言");
            task_1.Comment(addresser, addressee_A, "9-注意分纯留言");
            task_1.Comment(addresser, addressee_A, "10-验收注意区分纯留言");
            SystemTime.SetDateTime(DateTime.Now.AddDays(-8));
            addressee_A.MessagesToMe.FirstOrDefault().Read();

            SystemTime.SetDateTime(DateTime.Now.AddDays(-3));
            task_1.Comment(addresser, addressee_A, "11-做该任务需要谨慎验收时注意区分纯留言");
            task_1.Comment(addresser, addressee_A, "12-做该任务需要谨慎");
            task_1.Comment(addresser, addressee_A, "13-验收时注意区分纯留言验收时注意区分纯留言做该任务需要谨慎");
            task_1.Comment(addresser, addressee_A, "14-做该任务需要谨慎");
            task_1.Comment(addresser, addressee_A, "15-做该任务需验收时注意区分要谨慎");
            task_1.Comment(addresser, addressee_A, "16-做需要谨慎");
            task_1.Comment(addresser, addressee_A, "17-做该任务需要谨慎");
            task_1.Comment(addresser, addressee_A, "18-做该任务谨慎");
            task_1.Comment(addresser, addressee_A, "19-做该任务需要谨慎");
            task_1.Comment(addresser, addressee_A, "20-任务需要谨慎");

            SystemTime.SetDateTime(DateTime.Now.AddDays(-5));
            task_2.Comment(addresser, addressee_B, "21-先准备测试文档");
            task_2.Comment(addresser, addressee_B, "22-先准备测试文档");
            task_2.Comment(addresser, addressee_B, "23-先准备测试文档");
            SystemTime.SetDateTime(DateTime.Now.AddDays(-4));
            addressee_B.MessagesToMe.FirstOrDefault().Read();

            SystemTime.SetDateTime(DateTime.Now.AddDays(-7));
            task_3.Comment(addresser, addressee_C, "24-不需要全部弄成query，简单的就不用了");
            SystemTime.SetDateTime(DateTime.Now.AddDays(-6));
            addressee_C.MessagesToMe.FirstOrDefault().Read();

            SystemTime.ResetDateTime();
        }

        private static void create_users()
        {
            addresser = UserFactory.create("发布留言测试人");
            addressee_A = UserFactory.create("收信人-A");
            addressee_B = UserFactory.create("收信人-B");
            addressee_C = UserFactory.create("收信人-C");
        }

        private static void join_project()
        {
            addresser.Join(ProjectFactory.rwgl);
            addresser.Join(ProjectFactory.jicheng);

            addressee_A.Join(ProjectFactory.rwgl);
            addressee_B.Join(ProjectFactory.rwgl);
            addressee_C.Join(ProjectFactory.rwgl);
        }

        private static void set_auth()
        {
            AuthorizationFactory.SetAuth(addresser, ProjectFactory.rwgl, false, true, false);
            AuthorizationFactory.SetAuth(addresser, ProjectFactory.jicheng, false, true, false);
        }
    }
}
