using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FFLTask.BLL.NHMap.Task
{
    class WorkPeriodMap : EntityMap<BLL.Entity.WorkPeriod>
    {
        public WorkPeriodMap()
        {
            References(m => m.Task).ForeignKey("FK_Pause_Task");
            Map(m => m.Begin);
            Map(m => m.End);
            Map(m => m.Duration);
        }
    }
}
