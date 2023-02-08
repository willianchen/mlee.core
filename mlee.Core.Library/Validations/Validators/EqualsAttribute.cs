using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Text;

namespace mlee.Core.Library.Validations.Validators
{
    /// <summary>
    /// 等值属性
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class EqualsAttribute : ValidationAttribute
    {
        public string OtherProperty { get; private set; }

        public EqualsAttribute(string otherProperty, string errorMessage) : base(errorMessage)
        {
            OtherProperty = otherProperty;
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
            PropertyInfo otherPropertyInfo = validationContext.ObjectType.GetProperty(OtherProperty);
            if (otherPropertyInfo == null)
            {
                return new ValidationResult($"{OtherProperty} 字段不存在");
            }
            if (value == null)
                return ValidationResult.Success;

            object otherPropertyValue = otherPropertyInfo.GetValue(validationContext.ObjectInstance, null);
            if (otherPropertyValue != null && !otherPropertyValue.Equals(value))
            {
                return new ValidationResult(FormatErrorMessage(string.Empty));
            }
            return ValidationResult.Success;


        }
    }
}