using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using mlee.Core.Library.Dto;
using mlee.Core.Library.Helpers;
using mlee.Core.Library.Logs;

namespace mlee.Core.Filters
{

    /// <summary>
    /// 输入模型验证过滤器
    /// </summary>
    public class ValidateInputFilter : IAsyncActionFilter
    {
        public ValidateInputFilter()
        {
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if (!context.ModelState.IsValid)
            {
                try
                {
                    var logger = Ioc.Create<ILog>();
                    var errors = context.ModelState
                    .Where(m => m.Value.ValidationState == ModelValidationState.Invalid)
                    .Select(m =>
                    {
                        var sb = new StringBuilder();
                        sb.AppendFormat("{0}：", m.Key);
                        sb.Append(m.Value.Errors.Select(n => n.ErrorMessage).Aggregate((x, y) => x + ";" + y));
                        return sb.ToString();
                    })
                    .Aggregate((x, y) => x + "|" + y);
                    logger.Error(errors);
                    context.Result = new JsonResult(ApiResult.ToFailed(ApiStatusEnum.ServerError, errors));
                }
                catch
                {
                    context.Result = new StatusCodeResult(ApiStatusEnum.ServerError.GetHashCode());
                }
                return;
            }

            await next();
        }
    }
}
