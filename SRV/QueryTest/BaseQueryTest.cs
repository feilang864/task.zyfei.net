using System.Collections.Generic;
using System.Linq;
using FFLTask.BLL.Entity;
using FFLTask.BLL.NHMap;
using Global.NHibernateTestHelper;
using NHibernate;
using NUnit.Framework;

namespace FFLTask.SRV.QueryTest
{
    public class BaseQueryTest
    {
        private static ISessionFactory _sessionFactory;
        static BaseQueryTest()
        {
            _sessionFactory = NHConfigProvider.Get(ConfigurationProvider.Action, Constant.DB_Name)
                .BuildSessionFactory();
        }

        protected ISession session
        {
            get
            {
                return SessionProvider.GetSession(_sessionFactory);
            }
        }

        [SetUp]
        public void SetUpSession()
        {
            SessionProvider.SetUpSession(_sessionFactory);
        }

        [TearDown]
        public void TearDown()
        {
            SessionProvider.TearDown(_sessionFactory);
        }

        protected void Save(params BaseEntity[] entities)
        {
            foreach (var entity in entities)
            {
                session.Save(entity);
            }

            session.Flush();
            session.Clear();
        }

        protected void Contains<T>(IList<T> entitys, T entity)
            where T : BaseEntity
        {
            Assert.That(entitys.Count(t => t.Id == entity.Id), Is.GreaterThan(0));
        }
    }
}
