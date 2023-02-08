using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;

namespace mlee.Core.Library.Helpers
{
    /// <summary>
    /// Web操作
    /// </summary>
    public static partial class Web
    {
        #region 静态构造方法

        /// <summary>
        /// 初始化Web操作
        /// </summary>
        static Web()
        {
            try
            {
                HttpContextAccessor = Ioc.Create<IHttpContextAccessor>();
                Environment = Ioc.Create<IWebHostEnvironment>();
            }
            catch
            {

            }
        }

        #endregion

        #region 属性

        /// <summary>
        /// Http上下文访问器
        /// </summary>
        public static IHttpContextAccessor HttpContextAccessor { get; set; }

        /// <summary>
        /// 当前Http上下文
        /// </summary>
        public static HttpContext HttpContext => HttpContextAccessor?.HttpContext;

        /// <summary>
        /// 当前Http请求
        /// </summary>
        public static HttpRequest Request => HttpContext?.Request;

        /// <summary>
        /// 当前Http响应
        /// </summary>
        public static HttpResponse Response => HttpContext?.Response;

        /// <summary>
        /// 宿主环境
        /// </summary>
        public static IWebHostEnvironment Environment { get; set; }

        #endregion

        #region 认证相关

        /// <summary>
        /// Token键
        /// </summary>
        public const string TokenKey = "token";
        /// <summary>
        /// 获取访问令牌
        /// 优先级 Url>Token>Header
        /// </summary>
        public static string AccessToken
        {
            get
            {

                string token = Request?.Query[TokenKey].ToString().Trim();
                if (!string.IsNullOrWhiteSpace(token))
                {
                    return token;
                }

                var authorization = Request?.Headers["Authorization"].ToSafeString();
                if (!string.IsNullOrWhiteSpace(authorization))
                {
                    if (authorization.Contains("Bearer"))
                    {
                        token = authorization.Replace("Bearer ", "");
                        return token;
                    }
                }
                token = AccessTokenFromCookie;
                if (!string.IsNullOrWhiteSpace(token))
                {
                    return token;
                }
                return string.Empty;
            }
        }

        /// <summary>
        /// 从Cookie获取Token
        /// </summary>
        public static string AccessTokenFromCookie
        {
            get
            {
                var authorization = Request?.HttpContext.GetCookiesValue(TokenKey);
                if (string.IsNullOrWhiteSpace(authorization))
                    return string.Empty;
                return authorization;
            }
        }
        /// <summary>
        /// 获取JwtPayload
        /// </summary>
        private static JwtPayload JwtPayLoad
        {
            get
            {
                if (string.IsNullOrWhiteSpace(AccessToken))
                    return null;
                var tokenHandler = new JwtSecurityTokenHandler();
                var payLoad = tokenHandler.ReadJwtToken(AccessToken).Payload;
                return payLoad;
            }
        }

        /// <summary>
        /// 获取用户登录ID
        /// </summary>
        public static string UserID
        {
            get
            {
                return GetJwtPayLoadValue("UserId");
            }
        }


        /// <summary>
        /// 获取用户登录ID
        /// </summary>
        public static string UserName
        {
            get
            {
                return GetJwtPayLoadValue("UserName");
            }
        }

        /// <summary>
        /// 获取用户手机号码
        /// </summary>
        public static string Mobile
        {
            get
            {
                return GetJwtPayLoadValue("Mobile");
            }
        }

        /// <summary>
        /// 过期时间戳
        /// </summary>
        public static string ExpireTime
        {
            get
            {
                return GetJwtPayLoadValue("exp");
            }
        }

        /// <summary>
        /// 附加数据
        /// </summary>
        public static string ExtData
        {
            get
            {
                return GetJwtPayLoadValue("ExtData");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string GetJwtPayLoadValue(string key)
        {
            if (JwtPayLoad == null)
                return string.Empty;
            var result = JwtPayLoad.FirstOrDefault(x => x.Key == key).Value?.ToString();
            return result.ToSafeString();
        }

        /// <summary>
        /// 设置token
        /// </summary>
        public static void SetTokenCookie(string token, int expires = 60 * 24)
        {
            Request?.HttpContext.SetCookies(TokenKey, $"{token}", expires);
        }

        /// <summary>
        /// 删除Token
        /// </summary>
        public static void RemoveTokenCookie()
        {
            Request?.HttpContext.DeleteCookies(TokenKey);
        }
        #endregion


        #region Browser(浏览器)

        /// <summary>
        /// 浏览器
        /// </summary>
        public static string Browser => Request?.Headers["User-Agent"];

        #endregion

        #region RootPath(根路径)

        /// <summary>
        /// 根路径
        /// </summary>
        public static string RootPath => Environment?.ContentRootPath;

        #endregion

        #region WebRootPath(Web根路径)

        /// <summary>
        /// Web根路径，即wwwroot
        /// </summary>
        public static string WebRootPath => Environment?.WebRootPath;

        #endregion


        #region Ip(客户端Ip地址)

        /// <summary>
        /// Ip地址
        /// </summary>
        private static string _ip;

        /// <summary>
        /// 设置Ip地址
        /// </summary>
        /// <param name="ip">Ip地址</param>
        public static void SetIp(string ip)
        {
            _ip = ip;
        }

        /// <summary>
        /// 重置Ip地址
        /// </summary>
        public static void ResetIp()
        {
            _ip = null;
        }

        /// <summary>
        /// 客户端Ip地址
        /// </summary>
        public static string Ip
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_ip) == false)
                    return _ip;
                var list = new[] { "127.0.0.1", "::1" };
                var result = HttpContext?.Connection?.RemoteIpAddress.ToSafeString();
                if (string.IsNullOrWhiteSpace(result) || list.Contains(result))
                    result = Common.IsWindows ? GetLanIp() : GetLanIp(NetworkInterfaceType.Ethernet);
                return result;
            }
        }

        /// <summary>
        /// 获取局域网IP
        /// </summary>
        private static string GetLanIp()
        {
            foreach (var hostAddress in Dns.GetHostAddresses(Dns.GetHostName()))
            {
                if (hostAddress.AddressFamily == AddressFamily.InterNetwork)
                    return hostAddress.ToString();
            }
            return string.Empty;
        }

        /// <summary>
        /// 获取局域网IP
        /// </summary>
        /// <param name="type">网络接口类型</param>
        private static string GetLanIp(NetworkInterfaceType type)
        {
            try
            {
                foreach (var item in NetworkInterface.GetAllNetworkInterfaces())
                {
                    if (item.NetworkInterfaceType != type || item.OperationalStatus != OperationalStatus.Up)
                        continue;
                    var ipProperties = item.GetIPProperties();
                    if (ipProperties.GatewayAddresses.FirstOrDefault() == null)
                        continue;
                    foreach (var ip in ipProperties.UnicastAddresses)
                    {
                        if (ip.Address.AddressFamily == AddressFamily.InterNetwork)
                            return ip.Address.ToString();
                    }
                }
            }
            catch
            {
                return string.Empty;
            }

            return string.Empty;
        }

        #endregion

        #region Host(主机)

        /// <summary>
        /// 主机
        /// </summary>
        public static string Host => HttpContext == null ? Dns.GetHostName() : GetClientHostName();

        /// <summary>
        /// 获取Web客户端主机名
        /// </summary>
        private static string GetClientHostName()
        {
            var address = GetRemoteAddress();
            if (string.IsNullOrWhiteSpace(address))
                return Dns.GetHostName();
            var result = Dns.GetHostEntry(IPAddress.Parse(address)).HostName;
            if (result == "localhost.localdomain")
                result = Dns.GetHostName();
            return result;
        }

        /// <summary>
        /// 获取远程地址
        /// </summary>
        private static string GetRemoteAddress()
        {
            return Request?.Headers["HTTP_X_FORWARDED_FOR"] ?? Request?.Headers["REMOTE_ADDR"];
        }

        #endregion

        #region Url(请求地址)

        /// <summary>
        /// 请求地址
        /// </summary>
        public static string Url => Request?.GetDisplayUrl();

        #endregion

    }
}
