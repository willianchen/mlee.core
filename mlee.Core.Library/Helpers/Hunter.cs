using mlee.Core.Library.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace mlee.Core.Library.Helpers
{
    public static class Hunter
    {
        /// <summary>
        /// 获取面积（平方米）
        /// </summary>
        /// <param name="width">宽度 单位：毫米(mm)</param>
        /// <param name="height">长度 单位：毫米(mm)</param>
        /// <param name="num">数量</param>
        /// <returns></returns>
        public static decimal AreaM2(decimal width, decimal height, int num)
        {
            return width * height * num / 1000000;
        }

        /// <summary>
        /// 获取拼版数量
        /// </summary>
        /// <param name="num"></param>
        /// <param name="panelWayX"></param>
        /// <param name="panelWayY"></param>
        /// <returns></returns>
        public static int GetSetNum(int num, int panelWayX, int panelWayY)
        {
            return (num / (panelWayX * panelWayY)).ToInt();
        }

        /// <summary>
        /// 总pcs数量
        /// </summary>
        /// <param name="num"></param>
        /// <param name="panelWayX"></param>
        /// <param name="panelWayY"></param>
        /// <returns></returns>
        public static int GetPcsNum(int num, int panelWayX, int panelWayY)
        {
            return (num * (panelWayX * panelWayY)).ToInt();
        }



        /// <summary>
        /// 设置用户编号
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <param name="adminCode">业务员编号</param>
        /// <returns></returns>
        public static string FormatUserCode(string userId, string adminCode)
        {
            //网站编号
            string siteCode = Appsettings.app(new string[] { "Site", "Code" });
            if (string.IsNullOrWhiteSpace(siteCode))
                siteCode = "A";

            string code = userId.Substring(0, 4);
            string code2 = userId.Substring(4);
            if (code2.ToInt() < 100)
            {
                code2 = code2.ToInt().ToString().PadLeft(3, '0');
            }
           
            //添加网站编号
            code = siteCode + code + code2;

            return code + adminCode.ToUpper();
        }
    }
}
