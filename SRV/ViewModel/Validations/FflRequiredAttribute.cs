using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace FFLTask.SRV.ViewModel.Validations
{
    public class FflRequiredAttribute : RequiredAttribute
    {
        public override string FormatErrorMessage(string name)
        {
            return string.Format("* {0}不能为空", name);
        }
    }
}