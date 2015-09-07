using System;
using System.Linq;
using FFLTask.BLL.Entity;
using Global.Core.Helper;

namespace FFLTask.Tool.BuildDatabase
{
    class ToMeListFactory
    {
        static User addressee, addresser_A, addresser_B, addresser_C;

        internal static void Create()
        {
            SystemTime.SetDateTime(DateTime.Now.AddDays(-10));

            create_users();
            join_project();
            set_auth();

            Task task_1 = TaskFactory.CreateTask(ProjectFactory.rwgl, addressee, addresser_A, "引入留言功能");
            Task task_2 = TaskFactory.CreateTask(ProjectFactory.rwgl, addressee, addresser_B, "在BuildDatabase中准备好数据");
            Task task_3 = TaskFactory.CreateTask(ProjectFactory.jicheng, addressee, addresser_C, "实现统计的NHQuery方法");

            SystemTime.SetDateTime(DateTime.Now.AddDays(-9));
            task_1.Comment(addresser_A, addressee, "1-验收时注意区分纯留言");
            SystemTime.SetDateTime(DateTime.Now.AddDays(-8));
            addressee.MessagesToMe.FirstOrDefault().Read();

            SystemTime.SetDateTime(DateTime.Now.AddDays(-3));
            task_1.Comment(addresser_A, addressee, "2-验收时注意纯留言");
            task_1.Comment(addresser_A, addressee, "3-验收时注意区分纯留言");
            task_1.Comment(addresser_A, addressee, "4-注意分纯留言");
            task_1.Comment(addresser_A, addressee, "5-验收注意区分纯留言");
            task_1.Comment(addresser_A, addressee, "6-验收时注意区分纯留言");
            task_1.Comment(addresser_A, addressee, "7-验收时注意纯留言");
            task_1.Comment(addresser_A, addressee, "8-验收时注意区分纯留言");
            task_1.Comment(addresser_A, addressee, "9-注意分纯留言");
            task_1.Comment(addresser_A, addressee, "10-验收注意区分纯留言");
            task_1.Comment(addresser_A, addressee, "11-做该任务需要谨慎验收时注意区分纯留言");
            task_1.Comment(addresser_A, addressee, "12-做该任务需要谨慎");
            task_1.Comment(addresser_A, addressee, "13-验收时注意区分纯留言验收时注意区分纯留言做该任务需要谨慎");
            task_1.Comment(addresser_A, addressee, "14-做该任务需要谨慎");
            task_1.Comment(addresser_A, addressee, "15-做该任务需验收时注意区分要谨慎");
            task_1.Comment(addresser_A, addressee, "16-做需要谨慎");
            task_1.Comment(addresser_A, addressee, "17-做该任务需要谨慎");
            task_1.Comment(addresser_A, addressee, "18-做该任务谨慎");
            task_1.Comment(addresser_A, addressee, "19-做该任务需要谨慎");
            task_1.Comment(addresser_A, addressee, "20-任务需要谨慎");

            SystemTime.SetDateTime(DateTime.Now.AddDays(-5));
            task_2.Comment(addresser_B, addressee, "21-先准备测试文档");
            task_2.Comment(addresser_B, addressee, "22-先准备测试文档");
            task_2.Comment(addresser_B, addressee, "23-先准备测试文档");
            SystemTime.SetDateTime(DateTime.Now.AddDays(-4));
            addressee.MessagesToMe.FirstOrDefault().Read();

            SystemTime.SetDateTime(DateTime.Now.AddDays(-7));
            task_3.Comment(addresser_C, addressee, "24-不需要全部弄成query，简单的就不用了");
            SystemTime.SetDateTime(DateTime.Now.AddDays(-6));
            addressee.MessagesToMe.FirstOrDefault().Read();

            SystemTime.ResetDateTime();
        }

        private static void create_users()
        {
            addressee = UserFactory.create("接收留言测试人");
            addresser_A = UserFactory.create("留言人-A");
            addresser_B = UserFactory.create("留言人-B");
            addresser_C = UserFactory.create("留言人-C");
        }

        private static void join_project()
        {
            addressee.Join(ProjectFactory.rwgl);
            addressee.Join(ProjectFactory.jicheng);

            addresser_A.Join(ProjectFactory.rwgl);
            addresser_B.Join(ProjectFactory.rwgl);
            addresser_C.Join(ProjectFactory.rwgl);
        }

        private static void set_auth()
        {
            AuthorizationFactory.SetAuth(addressee, ProjectFactory.rwgl, false, true, false);
            AuthorizationFactory.SetAuth(addressee, ProjectFactory.jicheng, false, true, false);
        }
    }
}
