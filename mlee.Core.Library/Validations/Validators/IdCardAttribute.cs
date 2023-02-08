using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.RegularExpressions;

namespace mlee.Core.Library.Validations
{
    /// <summary>
    /// 身份证验证
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class IdCardAttribute : ValidationAttribute
    {
        /// <summary>
        /// 格式化错误消息
        /// </summary>
        public override string FormatErrorMessage(string name)
        {
            if (ErrorMessage == null && ErrorMessageResourceName == null)
                ErrorMessage = "身份证号码不正确";
            return ErrorMessage;
        }

        /// <summary>
        /// 是否验证通过
        /// </summary>
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (string.IsNullOrEmpty(value.ToSafeString()))
                return ValidationResult.Success;
            if (Regex.IsMatch(value.ToSafeString(), ValidatePattern.IdCardPattern))
                return ValidationResult.Success;
            return new ValidationResult(FormatErrorMessage(string.Empty));
        }
    }
}