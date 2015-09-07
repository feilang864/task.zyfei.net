using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Mapping;
using FFLTask.BLL.Entity;

namespace FFLTask.BLL.NHMap
{
    class EntityMap<T>: ClassMap<T> where T: BaseEntity
    {
        public EntityMap()
        {
            Id(m => m.Id);
            Map(m => m.CreateTime).Index("IX_CreateTime").CustomSqlType("DATETIME(6)");
        }
    }
}
