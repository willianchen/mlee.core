using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace mlee.Core.Library
{
    /// <summary>
    /// Json拓展
    /// </summary>
    public static partial class Extensions
    {
        /// <summary>
        /// 将对象转换为Json字符串
        /// </summary>
        /// <param name="obj">目标对象</param>
        /// <param name="isConvertToSingleQuotes">是否将双引号转成单引号</param>
        public static string ToJson(this object obj, bool isConvertToSingleQuotes = false)
        {
            if (obj == null)
                return string.Empty;
            if (obj is string)
                return obj.ToString();
            return Helpers.Json.ToJson(obj, isConvertToSingleQuotes);
        }

      //  string[] props = { "operationRemark", "managerCode", "checkRemark" }; //排除掉，不能让前端看到的字段。为true的话就是只保留这些字段
        //jSetting.ContractResolver = new LimitPropsContractResolver(props, false);

        /// <summary>
        /// 将Json字符串转换为对象
        /// </summary>
        /// <param name="json">Json字符串</param>
        public static T JsonToObject<T>(this string obj)
        {
            if (string.IsNullOrWhiteSpace(obj))
                return default(T);
            return Helpers.Json.ToObject<T>(obj);
        }

        /// <summary>
        /// 深复制
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="obj">对象</param>
        /// <returns></returns>
        public static T DeepClone<T>(this T obj) where T : class
        {
            if (obj == null)
                return null;

            return obj.ToJson().ToObject<T>();
        }

        /// <summary>
        /// 初始化时间默认值
        /// </summary>
        /// <param name="obj">对象</param>
        /// <returns></returns>
        public static T InitDateTime<T>(this T obj) where T : class
        {
            obj.GetType().GetProperties().ForEach(aProperty =>
            {
                if (aProperty.PropertyType == typeof(DateTime))
                {
                    aProperty.SetValue(obj, DateTime.Now);
                }
            });

            return obj;
        }

        /// <summary>
        /// 将对象序列化为XML字符串
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="obj">对象</param>
        /// <returns></returns>
        public static string ToXmlStr<T>(this T obj)
        {
            var jsonStr = obj.ToJson();
            var xmlDoc = JsonConvert.DeserializeXmlNode(jsonStr, "root");
            string xmlDocStr = xmlDoc.InnerXml;

            return xmlDocStr;
        }

        /// <summary>
        /// 将对象序列化为XML字符串
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="obj">对象</param>
        /// <param name="rootNodeName">根节点名(建议设为xml)</param>
        /// <returns></returns>
        public static string ToXmlStr<T>(this T obj, string rootNodeName)
        {
            var jsonStr = obj.ToJson();
            var xmlDoc = JsonConvert.DeserializeXmlNode(jsonStr, rootNodeName);
            string xmlDocStr = xmlDoc.InnerXml;

            return xmlDocStr;
        }

        /// <summary>
        /// 获取某属性值
        /// </summary>
        /// <param name="obj">对象</param>
        /// <param name="propertyName">属性名</param>
        /// <returns></returns>
        public static object GetPropertyValue(this object obj, string propertyName)
        {
            return obj.GetType().GetProperty(propertyName, BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public | BindingFlags.Static).GetValue(obj);
        }

        /// <summary>
        /// 获取某属性值
        /// </summary>
        /// <param name="obj">对象</param>
        /// <param name="propertyName">属性名</param>
        /// <param name="value">值</param>
        /// <returns></returns>
        public static void SetPropertyValue(this object obj, string propertyName, object value)
        {
            obj.GetType().GetProperty(propertyName, BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public | BindingFlags.Static).SetValue(obj, value);
        }

        /// <summary>
        /// jsGetTime转为DateTime
        /// </summary>
        /// <param name="jsGetTime">js中Date.getTime()</param>
        /// <returns></returns>
        public static DateTime ToDateTime_From_JsGetTime(this long jsGetTime)
        {
            DateTime dtStart = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
            long lTime = long.Parse(jsGetTime + "0000");  //说明下，时间格式为13位后面补加4个"0"，如果时间格式为10位则后面补加7个"0",至于为什么我也不太清楚，也是仿照人家写的代码转换的
            TimeSpan toNow = new TimeSpan(lTime);
            DateTime dtResult = dtStart.Add(toNow); //得到转换后的时间

            return dtResult;
        }

        public static string ReplaceLayuiTree(this string Str)
        {
            return Str.Replace("\"Id\"", "\"id\"")
                .Replace("\"Title\"", "\"title\"")
                .Replace("\"Field\"", "\"field\"")
                .Replace("\"Href\"", "\"href\"")
                .Replace("\"Spread\"", "\"spread\"")
                .Replace("\"IsChecked\"", "\"checked\"")
                .Replace("\"Disabled\"", "\"disabled\"")
                .Replace("\"Children\"", "\"children\"")
                ;
        }
    }
}
