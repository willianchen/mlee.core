using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.IO;
using System.Xml;
using System.Linq;
using System.Reflection;

namespace mlee.Core.Library
{
    public static class CustomHelper
    {

        #region 验证手机号
        /// <summary>
        /// 验证手机号是否正确
        /// </summary>
        /// <param name="phoneNumber">待验证的手机号</param>
        /// <returns></returns>
        public static bool IsPhoneNumber(string phoneNumber)
        {
            #region 正则验证区间说明
            /*
            手机号码前三位列表：
            13(老)号段：130、131、132、133、134、135、136、137、138、139
            14(新)号段：1400、1410、1440、145、146、147、148、149
            15(新)号段：150、151、152、153、155、156、157、158、159
            16(新)号段：165、166
            17(新)号段：170、171、172、173、175、176、177、178、1740（0-5）、1740（6-9）、1740（10-12）
            18(3G)号段：180、181、182、183、184、185、186、187、188、189
            19(新)号段：198、199
            13(老)号段
            130：中国联通，GSM
            131：中国联通，GSM
            132：中国联通，GSM
            133：中国联通，后转给中国电信，CDMA
            134：中国移动，GSM
            135：中国移动，GSM
            136：中国移动，GSM
            137：中国移动，GSM
            138：中国移动，GSM
            139：中国移动，GSM
            14号段
            1400（0-9）号段：中国联通，物联网网号
            1410（0-9）号段：中国电信，物联网网号
            1440（0-9）号段：中国移动，物联网网号
            145：中国联通，上网卡
            146：中国联通，公众移动通信网号（物联网业务专用号段）
            147：中国移动，上网卡
            148：中国移动，公众移动通信网号（物联网业务专用号段）
            149：中国电信
            15(新)号段
            150：中国移动，GSM
            151：中国移动，GSM
            152：中国联通，网友反映实际是中国移动的
            153：中国联通，后转给中国电信，CDMA
            155：中国联通，GSM
            156：中国联通，GSM
            157：中国移动，GSM
            158：中国移动，GSM
            159：中国移动，GSM
            16(新)号段
            165：中国移动，公众移动通信网号（移动通信转售业务专用号段）
            166：中国联通，公众移动通信网号
            17(4G)号段
            170：虚拟运营商
            171：联通
            172：移动
            173：电信
            1740（0-5）：中国电信，卫星移动通信业务号
            1740（6-9）：工业和信息化部应急通讯保障中心，卫星移动通信业务号（用于国际应急通讯需求）
            1740（10-12）：工业和信息化部应急通讯保障中心，卫星移动通信业务号（用于国际应急通讯需求）
            175：联通
            176：联通
            177：电信
            178：移动
            18(3G)号段
            180：中国电信，3G
            181：中国电信，3G
            182：中国移动，3G
            183：中国移动，3G
            184：中国移动，3G
            185：中国联通，3G
            186：中国联通，3G
            187：中国移动，3G
            188：中国移动，3G，TD-CDMA
            189：中国电信，3G，CDMA，天翼189，2008年底开始对外放号
            19新号段
            198：中国移动，公众移动通信网号
            199：中国电信，公众移动通信网号
             */
            #endregion
            Regex rx = new Regex(@"^0{0,1}(13[0-9]|14[0-1]|14[4-9]|15[0-3]|15[5-9]|16[5-6]|17[0-8]|18[0-9]|19[8-9])[0-9]{8}$");
            bool isTrue;
            if (!string.IsNullOrWhiteSpace(phoneNumber))
            {
                if (rx.IsMatch(phoneNumber)) //匹配
                {
                    isTrue = true;
                }
                else
                {
                    isTrue = false;
                }
            }
            else
            {
                isTrue = false;
            }

            return isTrue;
        }
        #endregion

        public static Dictionary<string, object> ToDic(this object obj)
        {
            string tempStr = "";
            if (obj is string) tempStr = obj as string;
            else tempStr = JsonConvert.SerializeObject(obj);

            Dictionary<string, object> dic = JsonConvert.DeserializeObject<Dictionary<string, object>>(tempStr);
            return dic;
        }
        #region Base64加密解密
        /// <summary>
        /// Base64加密，采用utf8编码方式加密
        /// </summary>
        /// <param name="source">待加密的明文</param>
        /// <returns>加密后的字符串</returns>
        public static string Base64Encode(string source)
        {
            return Base64Encode(Encoding.UTF8, source);
        }

        /// <summary>
        /// Base64加密
        /// </summary>
        /// <param name="encodeType">加密采用的编码方式</param>
        /// <param name="source">待加密的明文</param>
        /// <returns></returns>
        public static string Base64Encode(Encoding encodeType, string source)
        {
            byte[] bytes = encodeType.GetBytes(source);
            string encode;
            try
            {
                encode = Convert.ToBase64String(bytes);
            }
            catch
            {
                encode = source;
            }
            return encode;
        }

        /// <summary>
        /// Base64解密，采用utf8编码方式解密
        /// </summary>
        /// <param name="result">待解密的密文</param>
        /// <returns>解密后的字符串</returns>
        public static string Base64Decode(string result)
        {
            return Base64Decode(Encoding.UTF8, result);
        }

        /// <summary>
        /// Base64解密
        /// </summary>
        /// <param name="encodeType">解密采用的编码方式，注意和加密时采用的方式一致</param>
        /// <param name="result">待解密的密文</param>
        /// <returns>解密后的字符串</returns>
        public static string Base64Decode(Encoding encodeType, string result)
        {
            byte[] bytes = Convert.FromBase64String(result);
            string decode;
            try
            {
                decode = encodeType.GetString(bytes);
            }
            catch
            {
                decode = result;
            }
            return decode;
        }

        public static string CreateBase64DecodeUrl(Dictionary<string, string> dicArray, Encoding code)
        {
            StringBuilder prestr = new StringBuilder();
            foreach (KeyValuePair<string, string> temp in dicArray)
            {
                prestr.Append(temp.Key + "=" + Base64Encode(code, temp.Value) + "&");
            }

            //去掉最後一個&字符
            int nLen = prestr.Length;
            prestr.Remove(nLen - 1, 1);

            return prestr.ToString();
        }
        #endregion

        #region 时间戳转换
        /// <summary>
        /// 日期时间转秒级时间戳
        /// </summary>
        /// <param name="dateTime">日期时间</param>
        /// <param name="isUtc">是否格林威治时间</param>
        /// <returns></returns>
        public static long GetTimeStamp(DateTime dateTime, bool isUtc = false)
        {
            TimeSpan ts = dateTime - new DateTime(1970, 1, 1, (isUtc ? 0 : 8), 0, 0, 0);
            return ts.TotalSeconds.ToLong();
        }
        /// <summary>
        /// 日期时间转毫秒级时间戳
        /// </summary>
        /// <param name="dateTime">时间</param>
        /// <param name="isUtc">是否格林威治时间</param>
        /// <returns></returns>
        public static long GetMillisecondTimeStamp(DateTime dateTime, bool isUtc = false)
        {
            TimeSpan ts = dateTime - new DateTime(1970, 1, 1, (isUtc ? 0 : 8), 0, 0, 0);
            return ts.TotalMilliseconds.ToLong();
        }
        /// <summary>
        /// 秒级时间戳转日期时间
        /// </summary>
        /// <param name="timeSpan">秒级时间戳</param>
        /// <param name="isUtc">是否格林威治时间</param>
        /// <returns></returns>
        public static DateTime GetDateTimeByTimeStamp(long timeSpan, bool isUtc = false)
        {
            DateTime dateTime = new DateTime(1970, 1, 1, (isUtc ? 0 : 8), 0, 0, 0).AddSeconds(timeSpan);
            return dateTime;
        }
        /// <summary>
        /// 毫秒级时间戳转日期时间
        /// </summary>
        /// <param name="timeSpan"></param>
        /// <param name="isUtc">是否格林威治时间</param>
        /// <returns></returns>
        public static DateTime GetDateTimeByMillisecondsTimeStamp(long timeSpan, bool isUtc = false)
        {
            DateTime dateTime = new DateTime(1970, 1, 1, (isUtc ? 0 : 8), 0, 0, 0).AddMilliseconds(timeSpan);
            return dateTime;
        }
        #endregion

        #region 32位MD5加密
        /// <summary>
        /// 32位MD5加密
        /// </summary>
        /// <param name="password"></param>
        /// <returns></returns>
        public static string MD5Encrypt32(string password,string x="x")
        {
            string cl = password;
            string pwd = "";
            //实例化一个md5对像
            using (MD5 md5 = MD5.Create())
            {
                // 加密后是一个字节类型的数组，这里要注意编码UTF8/Unicode等的选择　
                byte[] s = md5.ComputeHash(Encoding.UTF8.GetBytes(cl));
                // 通过使用循环，将字节类型的数组转换为字符串，此字符串是常规字符格式化所得
                for (int i = 0; i < s.Length; i++)
                {
                    // 将得到的字符串使用十六进制类型格式。格式后的字符是小写的字母，如果使用大写（X）则格式后的字符是大写字符 
                    pwd += s[i].ToString(x);
                }
                return pwd;
            }
        }
        #endregion

        #region 对象通过反射进行赋值

        /// <summary>
        /// 属性同步
        /// </summary>
        /// <typeparam name="T">同步目标的类型</typeparam>
        /// <param name="sourceObj">同步源对象</param>
        /// <param name="targetObj">同步目标对象</param>
        /// <param name="ignoreFileds">不需要同步的键值</param>
        public static void AttributeSync<T>(Object sourceObj, ref T targetObj, params string[] ignoreFileds) where T : class
        {
            Type sourceType = sourceObj.GetType();
            Type targetType = targetObj.GetType();
            PropertyInfo[] sourceProperties = sourceType.GetProperties();
            PropertyInfo[] targetProperties = targetType.GetProperties();

            if (ignoreFileds == null)
            {
                ignoreFileds = new string[0];
            }
            foreach (PropertyInfo sourceProperty in sourceProperties)
            {
                //if(ignoreFileds.Contains(sourceProperty.Name))
                if (ignoreFileds.Any(x => x.Equals(sourceProperty.Name, StringComparison.CurrentCultureIgnoreCase)))
                {
                    continue;
                }
                foreach (PropertyInfo targetProperty in targetProperties)
                {
                    if (targetProperty.Name.Equals(sourceProperty.Name, StringComparison.CurrentCultureIgnoreCase) && sourceProperty.GetValue(sourceObj, null) != null)
                    {
                        targetProperty.SetValue(targetObj, sourceProperty.GetValue(sourceObj, null), null);
                        break;
                    }
                }
            }
        }

        #endregion

        /// <summary>
        /// 获取客户Ip
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public static string GetClientUserIp(this Microsoft.AspNetCore.Http.HttpContext context)
        {
            var ip = context.Request.Headers["X-Forwarded-For"].FirstOrDefault();
            if (string.IsNullOrEmpty(ip))
            {
                ip = context.Connection.RemoteIpAddress.ToString();
            }
            return ip;
        }
    }
}
