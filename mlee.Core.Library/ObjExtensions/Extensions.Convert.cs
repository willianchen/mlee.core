using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Text.RegularExpressions;
using mlee.Core.Library.Helpers;
using String = System.String;

namespace mlee.Core.Library
{
    /// <summary>
    /// 系统扩展 - 类型转换
    /// </summary>
    public static partial class Extensions
    {
        /// <summary>
        /// 安全转换为字符串，去除两端空格，当值为null时返回""
        /// </summary>
        /// <param name="input">输入值</param>
        public static string ToSafeString(this object input)
        {
            return input?.ToString().Trim() ?? string.Empty;
        }

        /// <summary>
        /// 字符串格式化
        /// </summary>
        /// <param name="input"></param>
        /// <param name="zeroNumber">小数点数量</param>
        /// <returns></returns>
        public static string ToFormatString(this decimal? input, int zeroNumber = 1)
        {
            return input.ToDecimal().ToFormatString(zeroNumber);
        }

        /// <summary>
        /// 字符串格式化
        /// </summary>
        /// <param name="input"></param>
        /// <param name="zeroNumber">小数点数量</param>
        /// <returns></returns>
        public static string ToFormatString(this decimal input, int zeroNumber = 1)
        {
            switch (zeroNumber)
            {
                case 0:
                    return input.ToDecimal().ToString("#0");
                case 1:
                    return input.ToDecimal().ToString("#0.0");
                case 2:
                    return input.ToDecimal().ToString("#0.00");
                case 3:
                    return input.ToDecimal().ToString("#0.000");
                case 4:
                    return input.ToDecimal().ToString("#0.0000");
                default:
                    return input.ToDecimal().ToString("#0.00");
            }
        }
        /// <summary>
        /// 转换为bool
        /// </summary>
        /// <param name="obj">数据</param>
        public static bool ToBool(this object obj, bool defautVal = false)
        {
            return Helpers.Convert.ToBool(obj, defautVal);
        }

        /// <summary>
        /// 转换为可空bool
        /// </summary>
        /// <param name="obj">数据</param>
        public static bool? ToBoolOrNull(this object obj)
        {
            return Helpers.Convert.ToBoolOrNull(obj);
        }

        /// <summary>
        /// 转换为可空int
        /// </summary>
        /// <param name="obj">数据</param>
        public static int ToInt(this object obj, int defaultVal = 0)
        {
            return Helpers.Convert.ToInt(obj, defaultVal);
        }

        /// <summary>
        /// 转换为可空int
        /// </summary>
        /// <param name="obj">数据</param>
        public static int? ToIntOrNull(this object obj)
        {
            return Helpers.Convert.ToIntOrNull(obj);
        }

        /// <summary>
        /// 转换为long
        /// </summary>
        /// <param name="obj">数据</param>
        public static long ToLong(this object obj, long defaultVal = 0)
        {
            return Helpers.Convert.ToLong(obj, defaultVal);
        }

        /// <summary>
        /// 转换为可空long
        /// </summary>
        /// <param name="obj">数据</param>
        public static long? ToLongOrNull(this object obj)
        {
            return Helpers.Convert.ToLongOrNull(obj);
        }

        public static string ToCommnStr(this string str)
        {
            //[^A-Za-z0-9_.]
            if (str == String.Empty)
                return String.Empty; str = str.Replace("'", "");
            //str = str.Replace(";", "");
            //str = str.Replace(",", "");
            //str = str.Replace("?", "");
            //str = str.Replace("<", "");
            //str = str.Replace(">", "");
            //str = str.Replace("(", "");
            //str = str.Replace(")", "");
            //str = str.Replace("）", "");
            //str = str.Replace("（", "");
            //str = str.Replace("@", "");
            //str = str.Replace("=", "");
            //str = str.Replace("+", "");
            //str = str.Replace("*", "");
            //str = str.Replace("&", "");
            //str = str.Replace("#", "");
            //str = str.Replace("%", "");
            //str = str.Replace("$", "");
            //str = str.Replace("/", "");
            //str = str.Replace("\\", "");
            Regex reg = new Regex(@"[a-zA-Z0-9\w.\u4e00-\u9fa5]");
            var m = reg.Matches(str);
            if (m.Count > 0)
            {
                var str2 = "";
                foreach (Match t in m)
                {
                    str2 += t.Value;

                }
                str = str2;
            }
            return str;
        }

        /// <summary>
        /// 转换为double
        /// </summary>
        /// <param name="obj">数据</param>
        public static double ToDouble(this object obj, double defaultVal = 0)
        {
            return Helpers.Convert.ToDouble(obj, defaultVal: defaultVal);
        }

        /// <summary>
        /// 转换为可空double
        /// </summary>
        /// <param name="obj">数据</param>
        public static double? ToDoubleOrNull(this object obj)
        {
            return Helpers.Convert.ToDoubleOrNull(obj);
        }

        /// <summary>
        /// 转换为decimal
        /// </summary>
        /// <param name="obj">数据</param>
        public static decimal ToDecimal(this object obj, decimal defaultVal = 0)
        {
            return Helpers.Convert.ToDecimal(obj, defaultVal: defaultVal);
        }

        /// <summary>
        /// 转换为可空decimal
        /// </summary>
        /// <param name="obj">数据</param>
        public static decimal? ToDecimalOrNull(this object obj)
        {
            return Helpers.Convert.ToDecimalOrNull(obj);
        }

        /// <summary>
        /// 转换为日期
        /// </summary>
        /// <param name="obj">数据</param>
        public static DateTime ToDate(this object obj)
        {
            return Helpers.Convert.ToDate(obj);
        }

        /// <summary>
        /// 转换为可空日期
        /// </summary>
        /// <param name="obj">数据</param>
        public static DateTime? ToDateOrNull(this object obj)
        {
            return Helpers.Convert.ToDateOrNull(obj);
        }

        /// <summary>
        /// 转换为Guid
        /// </summary>
        /// <param name="obj">数据</param>
        public static Guid ToGuid(this string obj)
        {
            return Helpers.Convert.ToGuid(obj);
        }

        /// <summary>
        /// 转换为可空Guid
        /// </summary>
        /// <param name="obj">数据</param>
        public static Guid? ToGuidOrNull(this string obj)
        {
            return Helpers.Convert.ToGuidOrNull(obj);
        }

        /// <summary>
        /// 转换为Guid集合
        /// </summary>
        /// <param name="obj">数据,范例: "83B0233C-A24F-49FD-8083-1337209EBC9A,EAB523C6-2FE7-47BE-89D5-C6D440C3033A"</param>
        public static List<Guid> ToGuidList(this string obj)
        {
            return Helpers.Convert.ToGuidList(obj);
        }

        /// <summary>
        /// 转换为Guid集合
        /// </summary>
        /// <param name="obj">字符串集合</param>
        public static List<Guid> ToGuidList(this IList<string> obj)
        {
            if (obj == null)
                return new List<Guid>();
            return obj.Select(t => t.ToGuid()).ToList();
        }

        /// <summary>
        /// 返回新文件名，格式：年月日时分秒3位毫秒+5位随机码
        /// </summary>
        /// <returns></returns>
        public static string GetNewFileName()
        {
            var r = new Random();
            return DateTime.Now.ToString("yyyyMMddHHmmssfff" + r.Next(10000, 99999));
        }

        /// <summary>
        /// 获取URL
        /// </summary>
        /// <param name="instance"></param>
        /// <returns></returns>
        public static string ToUrlString(this object instance)
        {
            var listProperties = Reflection.GetPublicPropertyList(instance);
            var result = new StringBuilder();
            foreach (var t in listProperties)
            {
                result.Append($"{t.Name}={t.GetValue(instance)}&");
            }
            return result.ToString().TrimEnd('&');
        }

    }
}
