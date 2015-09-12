using System;
using Global.Core.Validation;

namespace FFLTask.SRV.ViewModel.User
{
    public class ProfileModel
    {
        [ChineseLength(10, ErrorMessage="* 称谓最多5个汉字/10个字母数字")]
        public string Greet { get; set; }
        public bool? Female { get; set; }

        [Date]
        public DateTime? Birthday { get; set; }
        
        public string Province { get; set; }
        public string City { get; set; }
        
        [Phone]
        public string Phone { get; set; }
        
        [QQ]
        public string QQ { get; set; }
        
        public string OtherContact { get; set; }
        public string Interested { get; set; }
        public string SelfDescription { get; set; }

        public bool BuildProject { get; set; }
    }
}
