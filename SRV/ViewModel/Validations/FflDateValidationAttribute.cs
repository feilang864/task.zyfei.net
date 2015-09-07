using System;
using System.ComponentModel.DataAnnotations;

namespace FFLTask.SRV.ViewModel.Validations
{
    public class FflDateValidationAttribute : RegularExpressionAttribute
    {
        public FflDateValidationAttribute()
            : base(@"\d{4}-\d{2}-\d{2}")
        {

        }

        public override string FormatErrorMessage(string name)
        {
            return string.Format("* 日期格式错误", name);
        }

        public override bool IsValid(object value)
        {
            if (value == null)
            {
                return true;
            }
            else
            {
                DateTime parsed = new DateTime();
                return DateTime.TryParse(value.ToString(), out parsed);
            }
        }
    }
}
