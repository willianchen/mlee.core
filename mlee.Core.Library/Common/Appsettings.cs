using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace mlee.Core.Library.Common
{
    public class Appsettings
    {
        static IConfiguration Configuration { get; set; }

        /// <summary>
        /// 手动注入对象
        /// StartUp内调用
        /// </summary>
        /// <param name="configuration"></param>
        public static void SetConfiguration(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        /// <summary>
        /// 封装要操作的字符
        /// eg: Appsettings.app(new string[] { "ConnectionStrings", "WeMedia_V2Connection"})
        /// </summary>
        /// <param name="sections"></param>
        /// <returns></returns>
        public static string app(params string[] sections)
        {
            return Configuration.GetSetting(sections);
        }

        /// <summary>
        /// 获取配置节对象实例
        /// eg: Appsettings.app(new string[] { "ConnectionStrings", "WeMedia_V2Connection"})
        /// </summary>
        /// <param name="sections"></param>
        /// <returns></returns>
        public static T GetValue<T>(params string[] sections)
        {
            return Configuration.GetSettingValue<T>(sections);
        }
    }
}
