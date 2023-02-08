using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace mlee.Core.Library.Helpers
{
    public static class RegexHelper
    {
        #region 正则表达式

        #region 获取单一数据
        /// <summary>
        /// 获取整个正则表达式数据【第0组】
        /// </summary>
        /// <param name="SourceStr"></param>
        /// <param name="Reg"></param>
        /// <returns></returns>
        public static string GetORegStr(string SourceStr, string Reg)
        {
            return GetORegGroup(SourceStr, Reg, 0);
        }

        /// <summary>
        /// 获取正则表达式默认第一组数据
        /// </summary>
        /// <param name="SourceStr"></param>
        /// <param name="Reg"></param>
        /// <returns></returns>
        public static string GetORegGroup(string SourceStr, string Reg)
        {
            return GetORegGroup(SourceStr, Reg, 1);
        }

        /// <summary>
        /// 根据正则表达式获取组ID获取组数据
        /// </summary>
        /// <param name="SourceStr"></param>
        /// <param name="Reg"></param>
        /// <param name="GroupId"></param>
        /// <returns></returns>
        public static string GetORegGroup(string SourceStr, string Reg, int GroupId)
        {
            string tmpValue = string.Empty;
            Regex r = new Regex(Reg, RegexOptions.IgnoreCase);
            if (r.IsMatch(SourceStr))
            {
                tmpValue = r.Match(SourceStr).Groups[GroupId].Value;
            }
            return tmpValue;
        }

        /// <summary>
        /// 获取get组的数据【?﹤get﹥】
        /// </summary>
        /// <param name="SourceStr"></param>
        /// <param name="Reg"></param>
        /// <returns></returns>
        public static string GetORegGroupName(string SourceStr, string Reg)
        {
            return GetORegGroupName(SourceStr, Reg, "get");
        }
        /// <summary>
        /// 根据正则表达式组名获取数据【?﹤groupname﹥】
        /// </summary>
        /// <param name="SourceStr"></param>
        /// <param name="Reg"></param>
        /// <param name="GroupName"></param>
        /// <returns></returns>
        public static string GetORegGroupName(string SourceStr, string Reg, string GroupName)
        {
            string tmpValue = string.Empty;
            Regex r = new Regex(Reg, RegexOptions.IgnoreCase);
            if (r.IsMatch(SourceStr))
            {
                tmpValue = r.Match(SourceStr).Groups[GroupName].Value;
            }
            return tmpValue;
        }
        #endregion

        #region 获取批量数据
        #region 根据组ID获取数据
        /// <summary>
        /// 获取整个正则表达式数据【第0组】数组
        /// </summary>
        /// <param name="SourceStr">来源字符串</param>
        /// <param name="Reg">正则表达式</param>
        /// <returns>返回单组数组，默认0组</returns>
        public static string[] GetRegStr(string SourceStr, string Reg)
        {
            return GetRegGroup(SourceStr, Reg, "0");
        }
        /// <summary>
        /// 返回单组数组，默认1组
        /// </summary>
        /// <param name="SourceStr">来源字符串</param>
        /// <param name="Reg">正则表达式</param>
        /// <returns>返回单组数组，默认1组</returns>
        public static string[] GetRegGroup(string SourceStr, string Reg)
        {
            return GetRegGroup(SourceStr, Reg, "1");
        }

        /// <summary>
        /// 获取多组数据数组
        /// <param name="SourceStr">来源字符串</param>
        /// <param name="Reg">正则表达式</param>
        /// <param name="GroupIdC">获取组的id集合，eg:1,2,3</param>
        /// <returns>返回值，字符串数组，每个字符串里面多个组可有分割字符串"|"分割</returns>
        public static string[] GetRegGroup(string SourceStr, string Reg, string GroupIdC)
        {
            return GetRegGroup(SourceStr, Reg, GroupIdC, "|");
        }

        /// <summary>
        /// 获取多组数据数组
        /// </summary>
        /// <param name="SourceStr">来源字符串</param>
        /// <param name="Reg">正则表达式</param> 
        /// <param name="GroupIdC">获取组的id集合，eg:1,2,3</param>
        /// <param name="SplitStr">数组中获取多组的字符串分割</param>
        /// <returns>返回值，字符串数组，每个字符串里面多个组可有分割字符串分割</returns>
        public static string[] GetRegGroup(string SourceStr, string Reg, string GroupIdC, string SplitStr)
        {
            string[] GropuId = GroupIdC.Split(',');
            Regex r = new Regex(Reg, RegexOptions.IgnoreCase);
            if (r.IsMatch(SourceStr))
            {
                MatchCollection mc = r.Matches(SourceStr);
                int mcCount = mc.Count;
                if (mcCount > 0)
                {
                    string[] tmpStrArr = new string[mcCount];
                    for (int i = 0; i < mcCount; i++)
                    {
                        string tmpStr = string.Empty;
                        for (int ii = 0; ii < GropuId.Length; ii++)
                        {
                            tmpStr += mc[i].Groups[int.Parse(GropuId[ii])].Value + SplitStr;
                        }
                        tmpStrArr[i] = tmpStr.Trim(SplitStr.ToCharArray()).Trim();
                    }

                    return tmpStrArr;
                }
                else
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
        }
        #endregion
        #region 根据组名获取数据
        /// <summary>
        /// 获取组名单组数据数组，默认get组
        /// </summary>
        /// <param name="SourceStr">来源字符串</param>
        /// <param name="Reg">正则表达式</param>
        /// <param name="GroupIdC">获取组的组名集合，eg:get1,get2,get3</param>
        /// <param name="SplitStr">数组中获取多组的字符串分割</param>
        /// <returns>返回值，字符串数组，每个字符串里面多个组可有分割字符串分割</returns>
        public static string[] GetRegGroupName(string SourceStr, string Reg)
        {
            return GetRegGroupName(SourceStr, Reg, "get");
        }

        /// <summary>
        /// 获取组名多组数据数组
        /// </summary>
        /// <param name="SourceStr">来源字符串</param>
        /// <param name="Reg">正则表达式</param>
        /// <param name="GroupIdC">获取组的组名集合，eg:get1,get2,get3</param>
        /// <param name="SplitStr">数组中获取多组的字符串分割</param>
        /// <returns>返回值，字符串数组，每个字符串里面多个组可有分割字符串分割</returns>
        public static string[] GetRegGroupName(string SourceStr, string Reg, string GroupNameC)
        {
            return GetRegGroupName(SourceStr, Reg, GroupNameC, "|");
        }
        /// <summary>
        /// 获取组名多组数据数组
        /// </summary>
        /// <param name="SourceStr">来源字符串</param>
        /// <param name="Reg">正则表达式</param>
        /// <param name="GroupIdC">获取组的组名集合，eg:get1,get2,get3</param>
        /// <param name="SplitStr">数组中获取多组的字符串分割</param>
        /// <returns>返回值，字符串数组，每个字符串里面多个组可有分割字符串分割</returns>
        public static string[] GetRegGroupName(string SourceStr, string Reg, string GroupNameC, string SplitStr)
        {
            string[] GropuName = GroupNameC.Split(',');
            Regex r = new Regex(Reg, RegexOptions.IgnoreCase);
            if (r.IsMatch(SourceStr))
            {
                MatchCollection mc = r.Matches(SourceStr);
                int mcCount = mc.Count;
                if (mcCount > 0)
                {
                    string[] tmpStrArr = new string[mcCount];
                    for (int i = 0; i < mcCount; i++)
                    {
                        string tmpStr = string.Empty;
                        for (int ii = 0; ii < GropuName.Length; ii++)
                        {
                            tmpStr += mc[i].Groups[GropuName[ii].Trim()].Value + SplitStr;
                        }
                        tmpStrArr[i] = tmpStr.Trim(SplitStr.ToCharArray()).Trim();
                    }

                    return tmpStrArr;
                }
                else
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
        }
        #endregion
        #region 获取Capture数据
        public static string[] GetRegCaptureByGroupName(string SourceStr, string Reg, string GroupName, string SplitStr)
        {
            Regex r = new Regex(Reg, RegexOptions.IgnoreCase);
            if (r.IsMatch(SourceStr))
            {
                MatchCollection mc = r.Matches(SourceStr);
                int mcCount = mc.Count;
                if (mcCount > 0)
                {
                    string[] tmpStrArr = new string[mcCount];
                    for (int i = 0; i < mcCount; i++)
                    {
                        string tmpStr = string.Empty;
                        foreach (Capture item in mc[i].Groups[GroupName].Captures)
                        {
                            tmpStr += item.Value + SplitStr;
                        }
                        tmpStrArr[i] = tmpStr.Trim(SplitStr.ToCharArray()).Trim();
                    }
                    return tmpStrArr;
                }
                else
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
        }
        #endregion
        #endregion

        #region 替换&IsMatch
        /// <summary>
        /// 根据正则替换数据
        /// </summary>
        /// <param name="Str">数据源</param>
        /// <param name="Reg">替换正则</param>
        /// <param name="repStr">替换数据</param>
        /// <returns>替换后的字符串</returns>
        public static string Repalce(string Str, string Reg, string repStr)
        {
            Regex reg = new Regex(Reg, RegexOptions.IgnoreCase);
            return reg.Replace(Str, repStr);
        }

        /// <summary>
        /// 是否匹配成功
        /// </summary>
        /// <param name="Str">数据源</param>
        /// <param name="Reg">正则</param>
        /// <returns>true or false</returns>
        public static bool IsMatch(string Str, string Reg)
        {
            Regex reg = new Regex(Reg, RegexOptions.IgnoreCase);
            return reg.IsMatch(Str);
            //return reg.Match(Str).Success;
        }
        #endregion

        #region 去除Html标签
        /// <summary>
        /// 去除html标签
        /// </summary>
        /// <param name="html"></param>
        /// <returns></returns>
        public static string ToHtmlRemove(this string html)
        {
            if (!string.IsNullOrEmpty(html))
            {
                html = html.Trim();
                string text1 = "<.*?>";
                Regex regex1 = new Regex(text1);
                html = regex1.Replace(html, "");
                html = html.Replace("&nbsp;", " ");
            }
            else
            {
                html = "";
            }
            return html;
        }



        /// <summary>
        /// 去除字符串中的html标签
        /// </summary>
        /// <param name="requestString">要处理的字符串</param>
        /// <returns>去除html标签后的字符串</returns>
        public static string RemoveHtml(this string requestString)
        {
            //删除脚本   
            requestString = Regex.Replace(requestString, @"<script[^>]*?>.*?</script>", "", RegexOptions.IgnoreCase);
            //删除Html 标签  
            requestString = Regex.Replace(requestString, @"<(.[^>]*)>", "", RegexOptions.IgnoreCase);
            requestString = Regex.Replace(requestString, @"([\r\n])[\s]+", "", RegexOptions.IgnoreCase);
            requestString = Regex.Replace(requestString, @"-->", "", RegexOptions.IgnoreCase);
            requestString = Regex.Replace(requestString, @"<!--.*", "", RegexOptions.IgnoreCase);
            requestString = Regex.Replace(requestString, @"&(quot|#34);", "\"", RegexOptions.IgnoreCase);
            requestString = Regex.Replace(requestString, @"&(amp|#38);", "&", RegexOptions.IgnoreCase);
            requestString = Regex.Replace(requestString, @"&(lt|#60);", "<", RegexOptions.IgnoreCase);
            requestString = Regex.Replace(requestString, @"&(gt|#62);", ">", RegexOptions.IgnoreCase);
            requestString = Regex.Replace(requestString, @"&(nbsp|#160);", "   ", RegexOptions.IgnoreCase);
            requestString = Regex.Replace(requestString, @"&(iexcl|#161);", "\xa1", RegexOptions.IgnoreCase);
            requestString = Regex.Replace(requestString, @"&(cent|#162);", "\xa2", RegexOptions.IgnoreCase);
            requestString = Regex.Replace(requestString, @"&(pound|#163);", "\xa3", RegexOptions.IgnoreCase);
            requestString = Regex.Replace(requestString, @"&(copy|#169);", "\xa9", RegexOptions.IgnoreCase);
            requestString = Regex.Replace(requestString, @"&#(\d+);", "", RegexOptions.IgnoreCase);
            //将字符串转换为html编码的字符串。
            requestString = System.Web.HttpUtility.HtmlEncode(requestString).Trim();
            //返回结果
            return requestString;
        }

        #endregion

        #endregion
    }
}
