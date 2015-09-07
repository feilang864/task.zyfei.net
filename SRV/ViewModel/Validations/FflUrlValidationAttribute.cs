using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace FFLTask.SRV.ViewModel.Validations
{
    public class FflUrlValidationAttribute: RegularExpressionAttribute
    {
        public FflUrlValidationAttribute() : base(@"http(s)?://([\S-]+\.)+[\S-]+(/[\S- ./?%&=]*)?") { }

        public override string FormatErrorMessage(string name)
        {
            return string.Format("* URL格式不正确，请包含http(s)协议头");
        }
    }
}