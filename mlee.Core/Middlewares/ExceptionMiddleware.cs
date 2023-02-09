using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using mlee.Core.Library;
using mlee.Core.Library.Dto;
using mlee.Core.Library.Logs;
using mlee.Core.Library.Logs.Extensions;
using mlee.Core.Loggers.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace mlee.Core.Middlewares
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger _logger;

        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(httpContext, ex);
            }
        }

        private Task HandleExceptionAsync(HttpContext context, Exception exception)
        {

            context.Response.StatusCode = 200;
            var apiResult = new ApiResult() { Code = (int)ApiStatusEnum.ServerError, Message = exception.GetPrompt() };
            JsonResult result = new JsonResult(apiResult);
            /*      context.Result = result;
                  context.ExceptionHandled = true;*/

            var log = (ILog)context.RequestServices.GetService(typeof(ILog));
            log.Caption("异常捕获 - 异常处理过滤器").Content($"状态码：{context.Response.StatusCode}");
            exception.Log(log);

            /*        context.Response.ContentType = "application/json";
                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    _logger.LogError(exception, "");*/

            return context.Response.WriteAsync(result.ToJson());
        }
    }
}
