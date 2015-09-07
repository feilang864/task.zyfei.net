using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using FFLTask.BLL.Entity;
using NHibernate;
using FFLTask.BLL.NHMap;

namespace FFLTask.BLL.NHMapTest
{
    [TestFixture]
    class AuthorizationMapTest : BaseMapTest<Authorization>
    {
        [Test]
        public void Normal()
        {
            Authorization authorization = authorization = new Authorization
            {
                IsFounder = true,
                IsOwner = true,
                IsPublisher = true,
                IsAdmin = true,
                User = new User(),
                Project = new Project()
            };

            session.Save(authorization);

            Authorization load_authorization = Save(authorization);

            Assert.That(load_authorization.IsFounder, Is.EqualTo(true));
            Assert.That(load_authorization.IsOwner, Is.EqualTo(true));
            Assert.That(load_authorization.IsPublisher, Is.EqualTo(true));
            Assert.That(load_authorization.IsAdmin, Is.EqualTo(true));
            DBAssert.AreInserted(load_authorization.User);
            DBAssert.AreInserted(load_authorization.Project);

        }
    }
}
