using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace mlee.Core.Library.Helpers
{
    public  class Convert36
    {
        private const string BASE_CHAR = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        /// <summary>
        /// 数字转换为36位字符字符
        /// </summary>
        /// <param name="num"></param>
        /// <param name="length">编号最短长度，不足补0，默认长度0，不补0</param>
        /// <returns></returns>
        public static string NumToStrNo (long num,int length=0)
        {
            string str = "";

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
        /// 解析段字符
        /// </summary>
        /// <param name="strNo"></param>
        /// <returns></returns>
        public static long StrNoToNum(string strNo)
        {
            long num = 0;
            for (int i = 0; i < strNo.Length; i++)
            {
                num += BASE_CHAR.IndexOf(strNo[i]) * (int)Math.Pow(BASE_CHAR.Length, strNo.Length - i - 1);
            }

            return num;
        }
    }
}
