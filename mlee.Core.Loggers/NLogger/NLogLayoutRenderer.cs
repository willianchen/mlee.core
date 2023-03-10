using NLog;
using NLog.LayoutRenderers;
using mlee.Core.Library;
using mlee.Core.Library.Exceptions;
using mlee.Core.Loggers.Format;
using System;
using System.Collections.Generic;
using System.Text;

namespace mlee.Core.Loggers.NLogger
{
    /// <summary>
    /// NLog布局渲染器
    /// </summary>
    public class NLogLayoutRenderer : LayoutRenderer
    {
        /// <summary>
        /// 日志属性名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 添加日志信息
        /// </summary>
        protected override void Append(StringBuilder builder, LogEventInfo logEvent)
        {
            if (logEvent.Parameters == null || logEvent.Parameters.Length == 0)
                return;
            if (!(logEvent.Parameters[0] is LogContent content))
                return;
            builder.Append(GetValue(content));
        }

        /// <summary>
        /// 获取值
        /// </summary>
        private object GetValue(LogContent content)
        {
            var name = Name.ToSafeString().ToLower();
            if (name == "errormessage")
                return Warning.GetMessage(content.Exception);
            if (name == "stacktrace")
                return content.Exception?.StackTrace;
            return content.GetType().GetProperty(Name)?.GetValue(content);
        }
    }
}
