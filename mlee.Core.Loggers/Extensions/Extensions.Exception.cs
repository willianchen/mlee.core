using mlee.Core.Library;
using mlee.Core.Library.Exceptions;
using mlee.Core.Library.Logs;
using System;
using System.Collections.Generic;
using System.Text;

namespace mlee.Core.Loggers.Extensions
{
    /// <summary>
    /// 异常扩展
    /// </summary>
    public static partial class Extensions
    {
        /// <summary>
        /// 写日志
        /// </summary>
        /// <param name="exception">异常</param>
        /// <param name="log">日志</param>
        public static void Log(this Exception exception, ILog log)
        {
            exception = exception.GetRawException();
            if (exception is Warning warning)
            {
                log.Exception(exception, warning.Code).Warn();
                return;
            }
            log.Exception(exception).Error();
        }
    }
}
