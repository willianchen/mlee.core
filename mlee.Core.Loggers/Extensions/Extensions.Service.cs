using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using NLog.LayoutRenderers;
using mlee.Core.Library.Logs;
using mlee.Core.Library.Logs.Abstractions;
using mlee.Core.Library.Logs.Core;
using mlee.Core.Library.Sessions;
using mlee.Core.Loggers.Format;
using mlee.Core.Loggers.NLogger;
using System;

namespace mlee.Core.Loggers.Extensions
{
    /// <summary>
    /// 日志扩展
    /// </summary>
    public static partial class Extensions
    {
        /// <summary>
        /// 注册NLog日志操作
        /// </summary>
        /// <param name="services">服务集合</param>
        public static void UseNLog(this IServiceCollection services)
        {
            LayoutRenderer.Register<NLogLayoutRenderer>("log");
            services.TryAddScoped<ILogProviderFactory, NLogger.LogProviderFactory>();
            services.TryAddSingleton<ILogFormat, ContentFormat>();
            services.TryAddScoped<ILogContext, LogContext>();
            services.TryAddScoped<ILog, Log>();
            services.TryAddScoped<ISession, NullSession>();
        }
    }
}
