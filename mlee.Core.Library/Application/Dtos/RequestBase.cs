using mlee.Core.Library.Exceptions;
using mlee.Core.Library.Validations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace mlee.Core.Library.Application.Dtos
{
    /// <summary>
    /// 请求参数
    /// </summary>
    public abstract class RequestBase : IRequest
    {
        /// <summary>
        /// 验证
        /// </summary>
        public virtual ValidationResultCollection Validate()
        {
            var result = DataAnnotationValidation.Validate(this);
            if (result.IsValid)
                return ValidationResultCollection.Success;
            throw new Warning(result.First().ErrorMessage);
        }
    }
}