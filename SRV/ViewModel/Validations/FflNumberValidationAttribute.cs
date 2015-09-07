using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace FFLTask.SRV.ViewModel.Validations
{
    public class FflNumberValidationAttribute : RegularExpressionAttribute
    {
        public FflNumberValidationAttribute()
            : base(@"^[0-9]*$") { }

        public override string FormatErrorMessage(string name)
        {
            return string.Format("* 只能为数字");
        }
    }
}
