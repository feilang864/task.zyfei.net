using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FFLTask.SRV.ViewModel.Shared;
using FFLTask.SRV.ViewModel.Validations;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Web.Mvc;

namespace FFLTask.SRV.ViewModel.Account
{
    public class RegisterModel
    {
        public ImageCodeModel ImageCode { get; set; }
        public UserModel CurrentUser { get; set; }

        [FflRequired]
        [Display(Name = "用户名")]
        [FflStringLength(20)]
        public string UserName { get; set; }

        [FflRequired]
        [DataType(DataType.Password)]
        [FflStringLength(20, MinimumLength = 4)]
        [Display(Name = "密码")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [DisplayName("确认密码")]
        [Compare("Password", ErrorMessage = "* 确认密码和密码不一致")]
        public string ConfirmPassword { get; set; }

        public string UserRealName { get; set; }

        public bool BuildProject { get; set; }

    }
}
