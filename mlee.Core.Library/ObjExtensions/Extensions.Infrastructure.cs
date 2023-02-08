using AspectCore.Configuration;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using mlee.Core.Library.Dependency;
using mlee.Core.Library.Sessions;
using System;
using System.Collections.Generic;
using System.Text;
using Autofac;

namespace mlee.Core.Library
{
    /// <summary>
    /// 系统扩展 - 基础设施
    /// </summary>
    public static partial class Extensions
    {
        /// <summary>
        /// 注册基础设施服务
        /// </summary>
        /// <param name="services">服务集合</param>
        /// <param name="configs">依赖配置</param>
        public static void InitService(this IServiceCollection services)
        {
            services.AddHttpContextAccessor();
            services.AddHttpClient();
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            //  services.AddSingleton<Sessions.ISession, Session>();
        }


        public static void AddInfrasturcture(this ContainerBuilder builder, Action<IAspectConfiguration> aopConfigAction, params IConfig[] configs)
        {
            Bootstrapper.Run(builder, configs, aopConfigAction);
        }
    }
}
