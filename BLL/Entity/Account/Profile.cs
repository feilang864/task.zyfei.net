using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FFLTask.BLL.Entity
{
    public class Profile
    {
        public virtual string Greet { get; set; }
        public virtual bool? Female { get; set; }
        public virtual DateTime? Birthday { get; set; }
        public virtual string Province { get; set; }
        public virtual string City { get; set; }
        public virtual string Phone { get; set; }
        public virtual string QQ { get; set; }
        public virtual string OtherContact { get; set; }
        public virtual string Interested {get;set;}
        public virtual string SelfDescription { get; set; }
    }
}
