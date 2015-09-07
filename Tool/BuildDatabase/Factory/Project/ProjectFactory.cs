using FFLTask.BLL.Entity;
using NHibernate;

namespace FFLTask.Tool.BuildDatabase
{
    class ProjectFactory
    {
        private static readonly ISession session = NHProvider.session;

        internal static Project shougukeji;
        internal static Project chuangyejiayuan;
        internal static Project baichuangkaiyuan;
        internal static Project rwgl;
        internal static Project chuangyejiayuan_child;
        internal static Project drawing;
        internal static Project ui;
        internal static Project backend;
        internal static Project backend_csharp;
        internal static Project backend_DBA;
        internal static Project jicheng;
        internal static Project DBA;
        internal static Project chengxu;
        internal static Project chehua;
        internal static Project SEO;

        internal static void Create()
        {
            #region in group shougukeji

            shougukeji = create(UserFactory.yezi, "首顾科技", "首顾科技有限公司",
                new ProjectConfig
                {
                }, null);

            rwgl = create(UserFactory.yezi, "任务管理器", "任务管理分配，发布",
                new ProjectConfig
                {
                }, shougukeji);

            #region in project rwgl

            drawing = create(UserFactory.xinqing, "美工", "保证页面美观大方，出效果图，切片，生成静态HTML页面",
                new ProjectConfig
                {
                }, rwgl);

            ui = create(UserFactory.yezi, "UI", "负责页面的呈现、交互等前台功能，需要JS，Flash，ASP.NET MVC等技能",
                new ProjectConfig
                {
                }, rwgl);
            backend = create(UserFactory.zyfei, "后台", "负责后台的业务逻辑处理",
                new ProjectConfig
                {
                }, rwgl);

            #region in project backend

            backend_csharp = create(UserFactory.zyfei, "C#后台", "用C#实现业务逻辑",
                new ProjectConfig
                {
                }, backend);

            backend_DBA = create(UserFactory.jishuzai, "后台数据库", "负责后台模拟数据库的维护",
                new ProjectConfig
                {
                }, backend);

            #endregion

            #endregion

            chuangyejiayuan_child = create(UserFactory.yezi, "创业家园", "创业者的心灵家园",
                new ProjectConfig
                {
                }, shougukeji);

            #region in project cyjy

            jicheng = create(UserFactory.zyfei, "集成", "负责各个模块的组合",
                new ProjectConfig
                {
                }, chuangyejiayuan_child);
            DBA = create(UserFactory.yezi, "DBA", "数据库的维护管理",
                new ProjectConfig
                {
                }, chuangyejiayuan_child);
            #endregion

            #endregion

            #region in group chuangyejiayuan

            chuangyejiayuan = create(UserFactory.zyfei, "创业家园", "创业者的成长家园",
                new ProjectConfig
                {
                }, null);

            chengxu = create(UserFactory.zhangsan, "程序", "负责开发程序",
                new ProjectConfig
                {
                }, chuangyejiayuan);

            chehua = create(UserFactory.zyfei, "策划", "负责商业策划",
                new ProjectConfig
                {
                }, chuangyejiayuan);

            #endregion

            #region in group bcky

            baichuangkaiyuan = create(UserFactory.lisi, "百创开源", "百创开源有限公司",
                new ProjectConfig
                {
                }, null);

            SEO = create(UserFactory.lisi, "SEO", "网络方面的",
                new ProjectConfig
                {
                }, baichuangkaiyuan);

            #endregion

        }

        internal static Project create(User user, string name, string description, ProjectConfig config, Project parent)
        {
            Project project = new Project { Name = name, Description = description, Config = config, Parent = parent };
            user.Create(project);
            return project;
        }

        internal static Project create(User user, string name, string description, Project parent)
        {
            return create(user, name, description, null, parent);
        }

        internal static void Join()
        {
            UserFactory.meilunmeihuan.Join(drawing);
            UserFactory.xinqing.Join(ui);
            UserFactory.yongbaosijie.Join(backend_csharp);
            UserFactory.kejigaibianshenhuo.Join(backend_DBA); ;
            UserFactory.jikemengxiang.Join(DBA);

            UserFactory.xinqing.Join(rwgl);
            UserFactory.zyfei.Join(rwgl);
        }

        internal static void SetAssigner()
        {
            //Authorization auth = ui.Authorizations
            //    .Where(a => (a.User == UserFactory.zyfei && a.Project == ProjectFactory.ui))
            //    .SingleOrDefault();
            //auth.CanAssign();
        }
    }
}
