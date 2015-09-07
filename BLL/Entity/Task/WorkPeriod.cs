using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FFLTask.BLL.Entity
{
    public class WorkPeriod : BaseEntity
    {
        public virtual Task Task { get; set; }
        public virtual DateTime Begin { get; set; }
        public virtual DateTime End { get; set; }
        public virtual int Duration { get; protected internal set; }
        public virtual void MockDuration(int mockDuration)
        {
            Duration = mockDuration;
        }
    }
}
