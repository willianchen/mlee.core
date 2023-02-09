using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using mlee.Core.Library;
using mlee.Core.Library.Logs;
using mlee.Core.Library.Logs.Extensions;
using mlee.Core.Loggers;
using mlee.Core.Loggers.Extensions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mlee.Core.Logs
{

    /// <summary>
    /// 操作日志处理
    /// </summary>
    public class LogHandler : ILogHandler
    {
        /// <summary>
        /// 日志名
        /// </summary>
        public const string TraceLogName = "ApiTraceLog";

        private readonly ILog _logger;

        public LogHandler(
            ILog logger
        )
        {
            _logger = logger;
        }

        public async Task LogAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var sw = new Stopwatch();
            sw.Start();
            var actionExecutedContext = await next();
            sw.Stop();

         
        }

    }
}
