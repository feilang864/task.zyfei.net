using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace FFLTask.SRV.ViewModel.Validations
{
    public class GreaterThanAttribute : ValidationAttribute, IClientValidatable
    {
        private string _compareToPropertyName;
        public GreaterThanAttribute(string comparedPropetyName)
        {
            _compareToPropertyName = comparedPropetyName;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null)
            {
                return null;
            }

            var basePropertyInfo = validationContext.ObjectType.GetProperty(_compareToPropertyName);
            var valOther = (IComparable)basePropertyInfo.GetValue(validationContext.ObjectInstance, null);
            if (valOther == null)
            {
                return null;
            }

            var valThis = (IComparable)value;

            if (valThis.CompareTo(valOther) <= 0)
            {
                return new ValidationResult(base.ErrorMessage);
            }
            return null;
        }

        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            ModelClientValidationRule rules = new ModelClientValidationRule
            {
                ValidationType = "greater",
                ErrorMessage = FormatErrorMessage(metadata.DisplayName)
            };
            rules.ValidationParameters["than"] = _compareToPropertyName;
            yield return rules;
        }
    }
}
