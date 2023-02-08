using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace mlee.Core.Library.Validations.Validators
{

    /// <summary>
    /// 关联字段不为空校验
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class MinNumberAttribute : ValidationAttribute
    {
        public decimal MinValue { get; private set; }

        /// <summary>
        /// minValue为decimal类型的字符串
        /// </summary>
        /// <param name="minValue"></param>
        /// <param name="errorMessage"></param>
        public MinNumberAttribute(string minValue, string errorMessage) : base(errorMessage)
        {
            MinValue = minValue.ToDecimal();
        }

        /// <summary>
        /// 格式化错误消息
        /// </summary>
        public override string FormatErrorMessage(string name)
        {
            //if (ErrorMessage == null && ErrorMessageResourceName == null)
            //    ErrorMessage = "字段不能为空";
            return ErrorMessageString;
        }

        /// <summary>
        /// 是否验证通过
        /// </summary>
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null)
            {
                return ValidationResult.Success;
            }
            if (value.ToDecimal() < MinValue)
            {
                return new ValidationResult(FormatErrorMessage(string.Empty));
            }
            return ValidationResult.Success;


        }
    }
}