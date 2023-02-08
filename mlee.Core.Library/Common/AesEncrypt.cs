using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace mlee.Core.Library.Common
{
    public class AesEncrypt
    {

        /// <summary>
        /// 多平台通用AES加密
        /// 创建人：于德 创建时间：2018-07-04
        /// AES/CBC/ZeroPadding 128位模式，key和iv一样，编码统一用utf-8。不支持ZeroPadding的就用NoPadding.
        /// </summary>
        /// <param name="toEncrypt">加密字符串</param>
        /// <param name="key">16位key</param>
        /// <param name="iv">16位iv</param>
        /// <returns></returns>
        public static string Encrypt(string toEncrypt, string key, string iv)
        {
            byte[] keyArray = Encoding.UTF8.GetBytes(key);
            byte[] ivArray = Encoding.UTF8.GetBytes(iv);
            byte[] toEncryptArray = Encoding.UTF8.GetBytes(toEncrypt);
            RijndaelManaged rDel = new RijndaelManaged
            {
                Key = keyArray, IV = ivArray, Mode = CipherMode.CBC, Padding = PaddingMode.Zeros
            };
            ICryptoTransform cTransform = rDel.CreateEncryptor();
            byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);
            return Convert.ToBase64String(resultArray, 0, resultArray.Length);
        }

        /// <summary>
        /// 多平台通用AES解密
        /// 创建人：于德 创建时间：2018-07-04
        /// AES/CBC/ZeroPadding 128位模式，key和iv一样，编码统一用utf-8。不支持ZeroPadding的就用NoPadding.
        /// </summary>
        /// <param name="toDecrypt">解密字符串</param>
        /// <param name="key">16位key</param>
        /// <param name="iv">16位iv</param>
        /// <returns></returns>
        public static string Decrypt(string toDecrypt, string key, string iv)
        {
            byte[] keyArray = Encoding.UTF8.GetBytes(key);
            byte[] ivArray = Encoding.UTF8.GetBytes(iv);
            byte[] toEncryptArray = Convert.FromBase64String(toDecrypt);
            RijndaelManaged rDel = new RijndaelManaged
            {
                Key = keyArray, IV = ivArray, Mode = CipherMode.CBC, Padding = PaddingMode.Zeros
            };
            ICryptoTransform cTransform = rDel.CreateDecryptor();
            byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);
            return Encoding.UTF8.GetString(resultArray);
        }
    }
}
