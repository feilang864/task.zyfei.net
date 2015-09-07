using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate.Dialect;
using FFLTask.BLL.NHMap;

namespace FFLTask.Tool.BuildDatabase
{
    class NHProvider
    {

        private static ISession _session;
        internal static ISession session
        {
            get
            {
                if (_session == null)
                {
                    _session = getConfig()
                        .BuildSessionFactory().OpenSession();
                }
                return _session;
            }
        }

        internal static FluentConfiguration getConfig()
        {
#if Release
            string connStr = "server=localhost;Uid=root;database=task_prod_copy;Charset=utf8";
#else
            string connStr = "server=localhost;Uid=root;database=task_dev;Charset=utf8";
#endif
            return Fluently.Configure()
                .Database(
                    MySQLConfiguration.Standard.ConnectionString(connStr).Dialect<MySQL5Dialect>()
                    .ShowSql().FormatSql())
                .Mappings(BLL.NHMap.ConfigurationProvider.Action);
        }
    }
}
