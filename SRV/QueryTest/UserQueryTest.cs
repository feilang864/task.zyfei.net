using System.Collections.Generic;
using System.Linq;
using FFLTask.BLL.Entity;
using FFLTask.SRV.Query;
using NUnit.Framework;

namespace FFLTask.SRV.QueryTest
{
    [TestFixture]
    class UserQueryTest
    {
        [Test]
        public void Get_By_Name()
        {
            IQueryable<User> queryResource = new List<User>
            {
                new User{ Name = "Jonason"},
                new User { Name = "Jonaso"},
                new User { Name = "naso"},
                new User { Name = "Jonason, Zhang"}
            }.AsQueryable<User>();

            UserQuery query = new UserQuery(queryResource);
            
            User Jonason = query.getByName("Jonason").SingleOrDefault();
            Assert.That(Jonason.Name, Is.EqualTo("Jonason"));
        }
    }
}
