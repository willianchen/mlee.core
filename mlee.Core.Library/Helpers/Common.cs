using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;
using Microsoft.AspNetCore.Http;

namespace mlee.Core.Library.Helpers
{
    /// <summary>
    /// 常用公共操作
    /// </summary>
    public static partial class Common
    {
        /// <summary>
        /// 获取类型
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        public static Type GetType<T>()
        {
            return GetType(typeof(T));
        }

        /// <summary>
        /// 获取类型
        /// </summary>
        /// <param name="type">类型</param>
        public static Type GetType(Type type)
        {
            return Nullable.GetUnderlyingType(type) ?? type;
        }

        /// <summary>
        /// 换行符
        /// </summary>
        public static string Line => Environment.NewLine;

        /// <summary>
        /// 获取物理路径
        /// </summary>
        /// <param name="relativePath">相对路径</param>
        public static string GetPhysicalPath(string relativePath)
        {
            if (string.IsNullOrWhiteSpace(relativePath))
                return string.Empty;
            var rootPath = Web.RootPath;
            if (string.IsNullOrWhiteSpace(rootPath))
                return Path.GetFullPath(relativePath);
            return $"{Web.RootPath}\\{relativePath.Replace("/", "\\").TrimStart('\\')}";
        }

        /// <summary>
        /// 获取wwwroot路径
        /// </summary>
        /// <param name="relativePath">相对路径</param>
        public static string GetWebRootPath(string relativePath)
        {
            if (string.IsNullOrWhiteSpace(relativePath))
                return string.Empty;
            var rootPath = Web.WebRootPath;
            if (string.IsNullOrWhiteSpace(rootPath))
                return Path.GetFullPath(relativePath);
            return $"{Web.WebRootPath}\\{relativePath.Replace("/", "\\").TrimStart('\\')}";
        }

        /// <summary>
        /// 是否Linux操作系统
        /// </summary>
        public static bool IsLinux => RuntimeInformation.IsOSPlatform(OSPlatform.Linux);

        /// <summary>
        /// 是否Windows操作系统
        /// </summary>
        public static bool IsWindows => RuntimeInformation.IsOSPlatform(OSPlatform.Windows);

        /// <summary>
        /// 是否苹果操作系统
        /// </summary>
        public static bool IsOsx => RuntimeInformation.IsOSPlatform(OSPlatform.OSX);

        /// <summary>
        /// 当前操作系统
        /// </summary>
        public static string System => IsWindows ? "Windows" : IsLinux ? "Linux" : IsOsx ? "OSX" : string.Empty;

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
                if (ignoreFileds.Contains(sourceProperty.Name))
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

        #region 通过反射比对对象是否相等
        /// <summary>
        /// 对象属性值比对
        /// </summary>
        /// <param name="sourceObj">需要比对对象</param>
        /// <param name="targetObj">需要比对对象</param>
        /// <param name="ignoreFileds">不需要比对的键值</param>
        public static bool ComparisonObject(Object sourceObj, Object targetObj, params string[] ignoreFileds)
        {
            Type sourceType = sourceObj.GetType();
            Type targetType = targetObj.GetType();
            PropertyInfo[] sourceProperties = sourceType.GetProperties();
            PropertyInfo[] targetProperties = targetType.GetProperties();

            if (ignoreFileds == null)
            {
                ignoreFileds = new string[0];
            }
            if (sourceProperties.Count() != targetProperties.Count())
            {
                return false;
            }
            foreach (PropertyInfo sourceProperty in sourceProperties)
            {
                bool isIn = false;
                if (ignoreFileds.Contains(sourceProperty.Name))
                {
                    continue;
                }
                foreach (PropertyInfo targetProperty in targetProperties)
                {
                    if (targetProperty.Name.Equals(sourceProperty.Name, StringComparison.CurrentCulture))
                    {
                        if (sourceProperty.GetValue(sourceObj, null) == targetProperty.GetValue(targetObj, null))
                        {
                            isIn = true;
                            break;
                        }
                        else
                        {
                            return false;
                        }
                    }
                }
                if (!isIn)
                {
                    return isIn;
                }
            }
            return true;
        }
        #endregion

        #region 生成验证码
        /// <summary>
        /// 创建指定位数的随机数
        /// </summary>
        /// <param name="codeCount"></param>
        /// <returns></returns>
        public static string CreateRandomCode(int codeCount)
        {
            string allChar = "0,1,2,3,4,5,6,7,8,9,A,B,C,D,E,a,b,c,d,e,f,g,h,i,g,k,l,m,n,o,p,q,r,F,G,H,I,G,K,L,M,N,O,P,Q,R,S,T,U,V,W,X,Y,Z,s,t,u,v,w,x,y,z";
            string[] allCharArray = allChar.Split(',');
            string randomCode = "";
            int temp = -1;
            Random rand = new Random();
            for (int i = 0; i < codeCount; i++)
            {
                if (temp != -1)
                {
                    rand = new Random(i * temp * ((int)DateTime.Now.Ticks));
                }
                int t = rand.Next(35);
                if (temp == t)
                {
                    return CreateRandomCode(codeCount);
                }
                temp = t;
                randomCode += allCharArray[t];
            }
            return randomCode;
        }



        /// <summary>
        /// 创建指定位数的随机数
        /// 纯数字
        /// </summary>
        /// <param name="codeCount"></param>
        /// <returns></returns>
        public static string CreateRandomCode_Number(int codeCount)
        {
            string allChar = "0,1,2,3,4,5,6,7,8,9";
            string[] allCharArray = allChar.Split(',');
            string randomCode = "";
            int temp = -1;
            Random rand = new Random();
            for (int i = 0; i < codeCount; i++)
            {
                if (temp != -1)
                {
                    rand = new Random(i * temp * ((int)DateTime.Now.Ticks));
                }
                int t = rand.Next(10);
                if (temp == t)
                {
                    return CreateRandomCode_Number(codeCount);
                }
                temp = t;
                randomCode += allCharArray[t];
            }
            return randomCode;
        }

        #endregion
        #region 根据生成的验证码生成图片流
        public static byte[] CreateValidateGraphic(string validateCode)
        {
            Bitmap image = new Bitmap((int)Math.Ceiling(validateCode.Length * 49.0), 62);
            Graphics g = Graphics.FromImage(image);
            try
            {
                //生成随机生成器
                Random random = new Random();
                //清空图片背景色
                g.Clear(Color.White);
                //画图片的干扰线
                for (int i = 0; i < 60; i++)
                {
                    int x1 = random.Next(image.Width);
                    int x2 = random.Next(image.Width);
                    int y1 = random.Next(image.Height);
                    int y2 = random.Next(image.Height);
                    g.DrawLine(new Pen(Color.Silver), x1, x2, y1, y2);
                }
                Font font = new Font("Arial", 36, (FontStyle.Bold | FontStyle.Italic));

                LinearGradientBrush brush = new LinearGradientBrush(new Rectangle(0, 0, image.Width, image.Height), Color.Blue, Color.DarkRed, 1.2f, true);
                g.DrawString(validateCode, font, brush, 0, 0);

                //画图片的前景干扰线
                for (int i = 0; i < 220; i++)
                {
                    int x = random.Next(image.Width);
                    int y = random.Next(image.Height);
                    image.SetPixel(x, y, Color.FromArgb(random.Next()));
                }
                //画图片的边框线
                g.DrawRectangle(new Pen(Color.Silver), 0, 0, image.Width - 1, image.Height - 1);

                //保存图片数据
                MemoryStream stream = new MemoryStream();
                image.Save(stream, ImageFormat.Jpeg);

                //输出图片流
                return stream.ToArray();
            }
            finally
            {
                g.Dispose();
                image.Dispose();
            }
        }
        #endregion

        /// <summary>
        /// 保留前面的字符后面跟上cnt个*
        /// </summary>
        /// <param name="str"></param>
        /// <param name="cnt"></param>
        /// <returns></returns>
        public static string SetStrCut(this string str, int cnt, int startcount = 2)
        {
            if (!string.IsNullOrWhiteSpace(str))
            {
                var len = str.Length;
                var resut = str;
                resut = str.Substring(0, cnt);
                for (var i = 0; i < cnt; i++)
                {
                    resut += "*";
                }
                if (startcount > 0)
                {
                    if (len > 2)
                    {
                        resut += str.Substring(len - 1, 1);//取最后一个字
                    }
                }
                return resut;
            }
            return "";
        }

        #region 解析请求参数
        /// <summary>
        /// 解析get请求
        /// </summary>
        /// <param name="Query"></param>
        /// <returns></returns>
        public static Dictionary<string, string> GetRequestGet(IQueryCollection Query)
        {
            Dictionary<string, string> sArray = new Dictionary<string, string>();
            ICollection<string> requestItem = Query.Keys;
            foreach (var item in requestItem)
            {
                sArray.Add(item, Query[item]);

            }
            return sArray;
        }
        /// <summary>
        /// 解析post请求
        /// </summary>
        /// <param name="Form"></param>
        /// <returns></returns>
        public static Dictionary<string, string> GetRequestPost(IFormCollection Form)
        {
            Dictionary<string, string> sArray = new Dictionary<string, string>();

            ICollection<string> requestItem = Form.Keys;
            foreach (var item in requestItem)
            {
                sArray.Add(item, Form[item]);

            }
            return sArray;

        }
        #endregion

    }
}

