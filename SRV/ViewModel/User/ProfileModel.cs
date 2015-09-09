using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FFLTask.SRV.ViewModel.User
{
    public class ProfileModel
    {
        public string Greet { get; set; }
        public bool? Female { get; set; }
        public DateTime? Birthday { get; set; }
        public string Province { get; set; }
        public string City { get; set; }
        public string Phone { get; set; }
        public string QQ { get; set; }
        public string OtherContact { get; set; }
        public string Interested { get; set; }
        public string SelfDescription { get; set; }
    }
}
