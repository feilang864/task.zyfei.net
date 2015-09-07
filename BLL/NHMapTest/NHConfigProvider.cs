using FFLTask.BLL.NHMap;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate.Cfg;
using NHibernate.Dialect;

namespace FFLTask.BLL.NHMapTest
{
    class NHConfigProvider
    {
        internal static Configuration Get()
        {
            string connStr = "server=localhost;Uid=root;database=task_map_test;Charset=utf8";
            return Fluently.Configure()
                .Database(
                    MySQLConfiguration.Standard.ConnectionString(connStr).Dialect<MySQL5Dialect>()
                    .ShowSql().FormatSql())
                .Mappings(ConfigurationProvider.Action)
                .ExposeConfiguration(
                    c => c.SetProperty(NHibernate.Cfg.Environment.CurrentSessionContextClass, "thread_static"))
                .BuildConfiguration();
        }
    }
}
