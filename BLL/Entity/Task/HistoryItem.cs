using FFLTask.GLB.Global.Enum;
//using System.Collections.Generic;

namespace FFLTask.BLL.Entity
{
    public class HistoryItem : BaseEntity
    {
        public virtual Task Belong { get; protected internal set; }
        public virtual User Executor { get; protected internal set; }
        public virtual string Description { get; protected internal set; }
        public virtual string Comment { get; set; }
        public virtual Status Status { get; protected internal set; }
    }
}
