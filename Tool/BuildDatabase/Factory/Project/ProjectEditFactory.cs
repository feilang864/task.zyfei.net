using FFLTask.BLL.Entity;

namespace FFLTask.Tool.BuildDatabase
{
    class ProjectEditFactory
    {
        private static User founder_a, founder_b;
        private static Project project_a, project_b;

        internal static void Create()
        {
            founder_a = UserFactory.create("修改项目测试员-A");
            founder_b = UserFactory.create("修改项目测试员-B");
            project_a = ProjectFactory.create(founder_b, "修改测试项目-A", string.Empty, null);
            project_b = ProjectFactory.create(founder_a, "修改测试项目-B", string.Empty, null);
        }
    }
}
