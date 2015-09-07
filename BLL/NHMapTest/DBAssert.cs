using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using FFLTask.BLL.Entity;

namespace FFLTask.BLL.NHMapTest
{
    class DBAssert : Assert
    {
        public static void AreInserted(BaseEntity entity)
        {
            if (entity == null)
            {
                throw new NullReferenceException("the entity is null");
            }
            else if (entity.Id <= 0)
            {
                throw new AssertionException(
                    string.Format("{0} is not inserted into database", entity.GetType().Name));
            }
        }
    }
}
