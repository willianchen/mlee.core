using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace mlee.Core.Library.Common
{
    public class Md5Encrypt
    {
        #region MD5加密字符串处理
        /// <summary>
        /// MD5加密字符串处理-默认小写
        /// </summary>
        /// <param name="input">待加密码字符串</param>
        /// <param name="half">加密是16位还是32位；如果为true为16位</param>
        /// <param name="charset">编码</param>
        /// <returns></returns>
        public static string Encrypt(string input, bool half=false, string charset = "utf-8")
        {
            try
            {
                charset = charset ?? "utf-8";
                Encoding encoding;
                try
                {
                    encoding = Encoding.GetEncoding(charset);
                }
                catch (Exception)
                {
                    encoding = Encoding.UTF8;
                }

                using (var md5 = MD5.Create())
                {
                    var result = md5.ComputeHash(encoding.GetBytes(input));
                    var strResult = BitConverter.ToString(result);
                    strResult = strResult.Replace("-", "");
                    if (half)//16位MD5加密（取32位加密的9~25字符）
                    {
                        strResult = strResult?.Substring(8, 16);
                    }
                    return strResult.ToLower();
                }
            }
            catch (Exception)
            {

                return "00000000000000000000000000000000";
            }
                       
        }
        #endregion
    }
}
