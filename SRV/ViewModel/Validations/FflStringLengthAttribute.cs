using System.ComponentModel.DataAnnotations;

namespace FFLTask.SRV.ViewModel.Validations
{
    public sealed class FflStringLengthAttribute : StringLengthAttribute
    {
        public FflStringLengthAttribute(int maximumLength)
            : base(maximumLength)
        {

        }

        public override string FormatErrorMessage(string name)
        {
            if (MinimumLength == MaximumLength)
            {
                return string.Format("* {0}的长度只能等于{1}", name, MinimumLength);
            }
            else if (MinimumLength == 0)
            {
                return string.Format("* {0}的长度不能大于{1}", name, MaximumLength);
            }
            else
            {
                return string.Format("* {0}的长度不能小于{1}，大于{2}", name, MinimumLength, MaximumLength);
            }
            
        }
    }

}