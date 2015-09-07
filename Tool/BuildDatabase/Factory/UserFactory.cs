using FFLTask.BLL.Entity;
using Global.Core.ExtensionMethod;
using Global.Core.Helper;
using NHibernate;

namespace FFLTask.Tool.BuildDatabase
{
    class UserFactory
    {
        private static readonly ISession session = NHProvider.session;

        internal static User zyfei;
        internal static User yezi;
        internal static User xinqing;
        internal static User kejigaibianshenhuo;
        internal static User jishuzai;

        internal static User yongbaosijie;
        internal static User jikemengxiang;
        internal static User meilunmeihuan;
        internal static User zhangsan;
        internal static User lisi;
        internal static User wutuandui;

        internal static void Create()
        {
            xinqing = create("心晴");
            zyfei = create("自由飞");
            yezi = create("叶子");
            kejigaibianshenhuo = create("科技改变生活");
            jishuzai = create("技术宅");

            yongbaosijie = create("拥抱世界");
            jikemengxiang = create("极客梦想");
            meilunmeihuan = create("美轮美奂");
            zhangsan = create("张三");
            lisi = create("李四");
            wutuandui = create("无团队");
        }

        internal static User create(string username)
        {
            User user = new User
            {
                Name = username,
                Password = "1234".Md5Encypt(),
                AuthenticationCode = RandomGenerator.GetNumbers(6)
            };
            session.Save(user);
            return user;
        }
    }
}
