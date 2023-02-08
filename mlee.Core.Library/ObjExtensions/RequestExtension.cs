﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace mlee.Core.Library
{
    /// <summary>
    /// RequestExtension
    /// </summary>
    public static partial class Extensions
    {
        private const string NullIpAddress = "::1";

        /// <summary>
        /// 是否是本地请求
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public static bool IsLocal(this HttpRequest req)
        {
            var connection = req.HttpContext.Connection;
            if (connection.RemoteIpAddress.IsSet())
            {
                //We have a remote address set up
                return connection.LocalIpAddress.IsSet()
                    //Is local is same as remote, then we are local
                    ? connection.RemoteIpAddress.Equals(connection.LocalIpAddress)
                    //else we are remote if the remote IP address is not a loopback address
                    : IPAddress.IsLoopback(connection.RemoteIpAddress);
            }

            return true;
        }

        private static bool IsSet(this IPAddress address)
        {
            return address != null && address.ToString() != NullIpAddress;
        }


        /// <summary>
        /// Determines whether the specified HTTP request is an AJAX request.
        /// </summary>
        /// 
        /// <returns>
        /// true if the specified HTTP request is an AJAX request; otherwise, false.
        /// </returns>
        /// <param name="request">The HTTP request.</param><exception cref="T:System.ArgumentNullException">The <paramref name="request"/> parameter is null (Nothing in Visual Basic).</exception>
        public static bool IsAjaxRequest(this HttpRequest request)
        {
            if (request == null)
                throw new ArgumentNullException("request");

            if (request.Headers != null)
                return request.Headers["X-Requested-With"] == "XMLHttpRequest";
            return false;
        }

        /// <summary>
        /// 通常是以/开头的完整路径
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public static string PathAndQuery(this HttpRequest request)
        {
            if (request == null)
                throw new ArgumentNullException("request");

            return request.Path + request.QueryString;
        }

        /// <summary>
        /// 获取来源页面
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public static string UrlReferrer(this HttpRequest request)
        {
            return request.Headers["Referer"].ToString();
        }

        /// <summary>
        /// 返回绝对地址
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public static string AbsoluteUri(this HttpRequest request)
        {
            string schemeUpper = request.Scheme.ToUpper();//协议（大写）
            var host = request.Host.Host;
            var port = request.Host.Port ?? -1;//端口（.NET Core 中有可能会出现null）
            string portSetting = null;//Url中的端口部分
            if (port == -1 || //这个条件只有在 .net core 中， Host.Port == null 的情况下才会发生
                (schemeUpper == "HTTP" && port == 80) ||
                (schemeUpper == "HTTPS" && port == 443))
            {
                portSetting = "";//使用默认值
            }
            else
            {
                portSetting = ":" + port;//添加端口
            }

            var absoluteUri = string.Concat(
                          request.Scheme,
                          "://",
                          host,//不包含端口号
                          portSetting,//端口号
                          request.PathBase.ToUriComponent(),
                          request.Path.ToUriComponent(),
                          request.QueryString.ToUriComponent());

            return absoluteUri;
        }

        /// <summary>
        /// 获取客户端信息
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public static string UserAgent(this HttpRequest request)
        {
            return request.Headers["User-Agent"].ToString();
        }

        /// <summary>
        /// 获取客户端地址（IP）
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public static IPAddress UserHostAddress(this HttpContext httpContext)
        {
            return httpContext.Features.Get<IHttpConnectionFeature>()?.RemoteIpAddress;
        }
    }
}
