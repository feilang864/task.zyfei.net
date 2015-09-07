using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace FFLTask.SRV.ViewModel.Validations
{
    public class FflBlogUrlValidateAttribute : RegularExpressionAttribute
    {
        private static string regMatch = @"[a-z|A-Z|0-9|-]+";
        public FflBlogUrlValidateAttribute(): base(regMatch)
        {
            
        }

        public override string FormatErrorMessage(string name)
        {
            return string.Format("* {0}格式不正确", name);
        }

    }
}