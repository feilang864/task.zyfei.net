using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FFLTask.GLB.Global;
using Global.Core.Helper;

namespace FFLTask.BLL.Entity
{
    public class BaseEntity
    {
        public BaseEntity()
        {
            CreateTime = SystemTime.Now();
        }

        public virtual int Id { get; private set; }
        public virtual void MockId(int mockId)
        {
            Id = mockId;
        }

        public virtual DateTime CreateTime { get; private set; }
        public virtual void MockCreateTime(DateTime mockCreateTime)
        {
            CreateTime = mockCreateTime;
        }

    }
}
