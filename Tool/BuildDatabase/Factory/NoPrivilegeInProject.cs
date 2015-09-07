using FFLTask.BLL.Entity;

namespace FFLTask.Tool.BuildDatabase
{
    class NoPrivilegeInProject
    {
        static Project not_publish_root;
        static Project not_publish_branch_1;
        static Project not_publish_leaf;
        static Project not_publish_branch_2;

        internal static User not_publish;

        internal static void Create()
        {
            create_project();
            create_user();
            join_project();
            set_authorization();
        }

        private static void create_project()
        {
            User user = new User();
            not_publish_root = ProjectFactory.create(user, "无发权项目-根", string.Empty, null);
            not_publish_branch_1 = ProjectFactory.create(user, "无发权项目-枝-1", string.Empty, not_publish_root);
            not_publish_leaf = ProjectFactory.create(user, "无发权项目-叶", string.Empty, not_publish_branch_1);
            not_publish_branch_2 = ProjectFactory.create(user, "无发权项目-枝-2", string.Empty, not_publish_root);
        }

        private static void create_user()
        {
            not_publish = UserFactory.create("无发权用户");
        }

        private static void join_project()
        {
            not_publish.Join(not_publish_root);
            not_publish.Join(not_publish_branch_1);
            not_publish.Join(not_publish_leaf);
        }

        private static void set_authorization()
        {
            AuthorizationFactory.SetAuth(not_publish, not_publish_leaf, false, true, false);
        }
    }
}
