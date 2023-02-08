using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace mlee.Core.Library.Helpers
{
    public class ConvertNum
    {
        //private const string BASE_26CHAR = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        private const string BASE_36CHAR = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        //private const string BASE_52CHAR = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";
        private const string BASE_62CHAR = "0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";
        /// <summary>
        /// 数字转换为36位字符字符
        /// </summary>
        /// <param name="num"></param>
        /// <param name="length">编号最短长度，不足补0，默认长度0，不补0</param>
        /// <returns></returns>
        public static string NumToStrNo(long num, int length = 0,int radix = 36)
        {
            string str = "";
            string BASE_CHAR = GetBase_Char(radix);
            while (num > 0)
            {
                int cur = (int)(num % BASE_CHAR.Length);
                str = BASE_CHAR[cur] + str;
                num = num / BASE_CHAR.Length;
            }
            if (str.Length < length)
            {
                str = str.PadLeft(length, '0');
            }
            return str;
        }

        /// <summary>
        /// 解析段字符,26,52进制补0转换没有处理
        /// </summary>
        /// <param name="strNo"></param>
        /// <returns></returns>
        public static long StrNoToNum(string strNo, int radix = 36)
        {
            long num = 0;
            string BASE_CHAR = GetBase_Char(radix);
            for (int i = 0; i < strNo.Length; i++)
            {
                num += BASE_CHAR.IndexOf(strNo[i]) * (int)Math.Pow(BASE_CHAR.Length, strNo.Length - i - 1);
            }

            return num;
        }

        private static string GetBase_Char(int radix = 36)
        {
            string BASE_CHAR = string.Empty;

            switch (radix)
            {
                //case 26:
                //    BASE_CHAR = BASE_26CHAR;
                //    break;
                case 36:
                    BASE_CHAR = BASE_36CHAR;
                    break;
                //case 52:
                //    BASE_CHAR = BASE_52CHAR;
                //    break;
                case 62:
                    BASE_CHAR = BASE_62CHAR;
                    break;
                default:
                    BASE_CHAR = BASE_36CHAR;
                    break;
            }
            return BASE_CHAR;
        }
    }
}
