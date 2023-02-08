using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace mlee.Core.Library.Validations.Validators
{
    /// <summary>
    /// 手机号码验证
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class PhoneAttribute : ValidationAttribute
    {
        /// <summary>
        /// 格式化错误消息
        /// </summary>
        public override string FormatErrorMessage(string name)
        {
            if (ErrorMessage == null && ErrorMessageResourceName == null)
                ErrorMessage = "手机号码格式不正确";
            return ErrorMessage;
        }

        /// <summary>
        /// 是否验证通过
        /// </summary>
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (string.IsNullOrEmpty(value.ToSafeString()))
                return ValidationResult.Success;
            if (Regex.IsMatch(value.ToSafeString(), ValidatePattern.MobilePhonePattern))
                return ValidationResult.Success;
            return new ValidationResult(FormatErrorMessage(string.Empty));
        }
    }
}
