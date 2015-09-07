using FFLTask.BLL.Entity;
using NHibernate;
using NHibernate.Context;
using NUnit.Framework;

namespace FFLTask.BLL.NHMapTest
{
    public class BaseMapTest<T> where T : BaseEntity
    {
        protected static ISessionFactory _sessionFactory;
        static BaseMapTest()
        {
            _sessionFactory = NHConfigProvider.Get().BuildSessionFactory();
        }

        protected ISession session
        {
            get
            {
                ISession _session = _sessionFactory.GetCurrentSession();

                if (!_session.Transaction.IsActive)
                {
                    _session.BeginTransaction();
                }

                return _session;
            }
        }

        [SetUp]
        public void SetUpSession()
        {
            ISession _session = _sessionFactory.OpenSession();
            CurrentSessionContext.Bind(_session);
        }

        [TearDown]
        public void TearDown()
        {
            using (session)
            {
                using (session.Transaction)
                {
                    session.Transaction.Rollback();
                    //session.Transaction.Commit();
                }
            };
            CurrentSessionContext.Unbind(_sessionFactory);
        }

        public T Save(T entity)
        {
            int id = -1;

            session.Save(entity);
            id = entity.Id;
            Assert.That(id, Is.GreaterThan(0));

            session.Flush();
            session.Clear();

            T loaded_entity = session.Load<T>(id);
            Assert.That(loaded_entity, Is.Not.EqualTo(entity));

            return loaded_entity;
        }
    }
}
