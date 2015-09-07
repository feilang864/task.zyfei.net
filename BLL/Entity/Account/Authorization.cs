using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FFLTask.BLL.Entity
{
    public class Authorization : BaseEntity
    {
        public virtual User User { get; set; }
        public virtual Project Project { get; set; }
        public virtual bool IsFounder { get; set; }
        public virtual bool IsPublisher { get; set; }
        public virtual bool IsOwner { get; set; }
        public virtual bool IsAdmin { get; set; }

        public virtual void CanAssign()
        {
            IsPublisher = true;
        }

        public virtual void CanNotAssign()
        {
            IsPublisher = false;
        }
    }
}
