using AutoMapper;
using FFLTask.BLL.NHMap;
using FFLTask.SRV.ProdService.ViewModelMap;
using FFLTask.SRV.ViewModel.Shared;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate;
using NHibernate.Caches.SysCache;
using NHibernate.Context;
using NHibernate.Dialect;
using System;
using System.Configuration;
using System.Linq;

namespace FFLTask.SRV.ProdService
{
    public class BaseService
    {
        private static ISessionFactory sessionFactory;

        static BaseService()
        {
            var log = log4net.LogManager.GetLogger("ServiceInfo");
            log.Info("begin to build a session factory");

            string connStr = ConfigurationManager.ConnectionStrings["dev"].ConnectionString;
            sessionFactory = Fluently.Configure()
                .Database(
                    MySQLConfiguration.Standard.ConnectionString(connStr).Dialect<MySQL5Dialect>()
#if DEBUG
.ShowSql().FormatSql()
#endif
)
                .Mappings(ConfigurationProvider.Action)
                .Cache(x => x.UseSecondLevelCache().ProviderClass<SysCacheProvider>())
                .ExposeConfiguration(
                    c => c.SetProperty(NHibernate.Cfg.Environment.CurrentSessionContextClass, "web"))
                .BuildSessionFactory();

            log.Info("has completed to build the session factory");

            createMap();

        }

        private static void createMap()
        {
            TeamMap.Init();
            TaskMap.Init();
#if DEBUG
            Mapper.AssertConfigurationIsValid();
#endif

        }

        protected ISession session
        {
            get
            {
                //since Begin() do nothing, so the _session can be unbind...
                ISession _session;
                if (!CurrentSessionContext.HasBind(sessionFactory))
                {
                    _session = sessionFactory.OpenSession();
                    CurrentSessionContext.Bind(_session);
                }
                else
                {
                    _session = sessionFactory.GetCurrentSession();
                }

                //TODO: not sure the meaning of IsActive
                //seems _session.Transaction can't be null always
                if (!_session.Transaction.IsActive)
                {
                    _session.BeginTransaction();
                }

                return _session;
            }
        }

        public static void EndSession()
        {
            if (CurrentSessionContext.HasBind(sessionFactory))
            {
                //NOTE: must be session from context
                ISession sessionFromContext = sessionFactory.GetCurrentSession();
                using (sessionFromContext)
                {
                    using (sessionFromContext.Transaction)
                    {
                        try
                        {
                            sessionFromContext.Transaction.Commit();
                        }
                        catch (Exception)
                        {
                            sessionFromContext.Transaction.Rollback();
                            throw;
                        }
                    }

                    //needn't since Commit() will do Flush() by default.
                    //session.Flush();
                }

                CurrentSessionContext.Unbind(sessionFactory);
            }
        }
    }

    internal static class BaseExtensionMethods
    {
        internal static IQueryable<T> Paged<T>(this IQueryable<T> items, PagerModel pager)
        {
            return items
                .Skip(pager.PageSize * (pager.PageIndex - 1))
                .Take(pager.PageSize);
        }
    }
}