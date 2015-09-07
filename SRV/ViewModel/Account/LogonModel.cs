using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using FFLTask.SRV.ViewModel.Validations;
using System.ComponentModel.DataAnnotations;
using FFLTask.SRV.ViewModel.Shared;

namespace FFLTask.SRV.ViewModel.Account
{
  public class LogonModel
    {
        [DisplayName("记住我的登陆状态")]
        public bool RememberMe { get; set; }

        [FflRequired]
        [DisplayName("用户名")]
        [FflStringLength(255)]
        public string UserName { get; set; }

        [FflRequired]
        [DataType(DataType.Password)]
        [FflStringLength(20, MinimumLength = 4)]
        [DisplayName("密码")]
        public string Password { get; set; }

        public ImageCodeModel ImageCode { get; set; }
    }
}
