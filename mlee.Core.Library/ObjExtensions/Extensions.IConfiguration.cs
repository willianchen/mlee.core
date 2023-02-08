using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace mlee.Core.Library
{
    /// <summary>
    /// 配置项拓展
    /// </summary>
    public static partial class Extensions
    {
        /// <summary>
        /// 获取配置项Value
        /// 支持层级
        /// eg: Configuration.GetSetting(new string[] { "ConnectionStrings", "WeMedia_V2Connection"})
        /// eg: Configuration.GetSetting("ConnectionStrings", "WeMedia_V2Connection")
        /// </summary>
        /// <param name="Configuration"></param>
        /// <param name="sections"></param>
        /// <returns></returns>
        public static string GetSetting(this IConfiguration Configuration, params string[] sections)
        {
            try
            {
                var val = string.Empty;
                for (int i = 0; i < sections.Length; i++)
                {
                    val += sections[i] + ":";
                }
                return Configuration[val.TrimEnd(':')];
            }
            catch (Exception)
            {
                return "";
            }
        }
        /// <summary>
        /// 获取配置节对象实例
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="Configuration"></param>
        /// <param name="sections"></param>
        /// <returns></returns>
        public static T GetSettingValue<T>(this IConfiguration Configuration, params string[] sections)
        {
            try
            {
                var val = string.Empty;
                for (int i = 0; i < sections.Length; i++)
                {
                    val += sections[i] + ":";
                }
                return Configuration.GetSection(val.TrimEnd(':')).Get<T>();
            }
            catch (Exception)
            {
                return default(T);
            }
        }
    }
}
