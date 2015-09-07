using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace FFLTask.SRV.ViewModel.Validations
{
    public class FflEmailValidationAttribute : RegularExpressionAttribute
    {
        public FflEmailValidationAttribute() : base(@"\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"){}

        public override string FormatErrorMessage(string name)
        {
            return string.Format("* 电子邮件格式不正确");
        }
    }
}