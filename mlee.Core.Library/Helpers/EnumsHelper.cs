using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using System.Text;

namespace HuntNet.Library.Helpers
{
     public class EnumsHelper
    {
        #region 枚举操作方法
        /// <summary>
        /// 获取所有枚举名称 
        /// </summary>
        /// <param name="enumType"></param>
        /// <returns></returns>
        public static string[] GetEnumNames(Type enumType)
        {
            return System.Enum.GetNames(enumType);
        }
        /// <summary>
        /// 获取每句的描述，获取相关值,用于页面,上绑定,要继承byte
        /// </summary>
        /// <param name="enumType"></param>
        /// <returns></returns>
        public static List<KeyValuePair<byte, string>> GetEnum(Type enumType)
        {
            var names = System.Enum.GetNames(enumType);
            if (names != null && names.Length > 0)
            {
                List<KeyValuePair<byte, string>> kvList = new List<KeyValuePair<byte, string>>(names.Length);
                foreach (var item in names)
                {
                    System.Reflection.FieldInfo finfo = enumType.GetField(item);
                    object[] enumAttr = finfo.GetCustomAttributes(typeof(System.ComponentModel.DescriptionAttribute), true);
                    if (enumAttr != null && enumAttr.Length > 0)
                    {
                        string description = string.Empty;

                        System.ComponentModel.DescriptionAttribute desc = enumAttr[0] as System.ComponentModel.DescriptionAttribute;
                        if (desc != null)
                        {
                            description = desc.Description;
                        }
                        kvList.Add(new KeyValuePair<byte, string>((byte)finfo.GetValue(null), description));
                    }

                }
                return kvList;

            }
            return null;
        }
        /// <summary>
        /// 获取每句的描述，获取相关值,用于页面,上绑定,要继承byte,参数indexNum获取枚举的条件
        /// </summary>
        /// <param name="enumType"></param>
        /// <param name="indexNum"></param>
        /// <returns></returns>
        public static List<KeyValuePair<byte, string>> GetEnum(Type enumType, int indexFirst, int indexLast)
        {
            var names = System.Enum.GetNames(enumType);
            if (names != null && names.Length > 0)
            {
                List<KeyValuePair<byte, string>> kvList = new List<KeyValuePair<byte, string>>(names.Length);
                foreach (var item in names)
                {
                    System.Reflection.FieldInfo finfo = enumType.GetField(item);
                    if ((byte)finfo.GetValue(null) > indexFirst && (byte)finfo.GetValue(null) < indexLast)
                    {
                        object[] enumAttr = finfo.GetCustomAttributes(typeof(System.ComponentModel.DescriptionAttribute), true);
                        if (enumAttr != null && enumAttr.Length > 0)
                        {
                            string description = string.Empty;

                            System.ComponentModel.DescriptionAttribute desc = enumAttr[0] as System.ComponentModel.DescriptionAttribute;
                            if (desc != null)
                            {
                                description = desc.Description;
                            }
                            kvList.Add(new KeyValuePair<byte, string>((byte)finfo.GetValue(null), description));
                        }
                    }
                }
                return kvList;

            }
            return null;
        }

        /// <summary>
        /// 获取枚举的描述 Description
        /// </summary>
        /// <param name="enumType"></param>
        /// <param name="val"></param>
        /// <returns></returns>
        public static string GetEnumDescription(Type enumType, object val)
        {

            string enumvalue = System.Enum.GetName(enumType, val);
            if (string.IsNullOrEmpty(enumvalue))
            {
                return "";
            }
            System.Reflection.FieldInfo finfo = enumType.GetField(enumvalue);
            object[] enumAttr = finfo.GetCustomAttributes(typeof(System.ComponentModel.DescriptionAttribute), true);
            if (enumAttr.Length > 0)
            {
                System.ComponentModel.DescriptionAttribute desc = enumAttr[0] as System.ComponentModel.DescriptionAttribute;
                if (desc != null)
                {
                    return desc.Description;
                }
            }
            return enumvalue;
        }

        /// <summary>
        /// 获取描述信息
        /// </summary>
        /// <param name="en">枚举</param>
        /// <returns></returns>
        public static string GetEnumDes(Enum en)
        {
            Type type = en.GetType();
            MemberInfo[] memInfo = type.GetMember(en.ToString());
            if (memInfo != null && memInfo.Length > 0)
            {
                object[] attrs = memInfo[0].GetCustomAttributes(typeof(System.ComponentModel.DescriptionAttribute), false);
                if (attrs != null && attrs.Length > 0)
                    return ((DescriptionAttribute)attrs[0]).Description;
            }
            return en.ToString();
        }
        #endregion
    }
}
