using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Description;
using System.Text;

namespace mlee.Core.Library.Dto
{
    public class ApiResult
    {

        /// <summary>
        /// 信息
        /// </summary>
        public string Message { get; set; } = "请求成功";

        /// <summary>
        /// 状态码
        /// </summary>
        public int Code { get; set; } = (int)ApiStatusEnum.Success;

        /// <summary>
        /// 扩展对象数据
        /// </summary>
        public dynamic extObj { get; set; }

        public ApiResult(int code = (int)ApiStatusEnum.Success, string message = "")
        {
            if (string.IsNullOrWhiteSpace(message) && code == (int)ApiStatusEnum.Success)
                message = "请求成功";
            Code = code;
            Message = message;
        }


        public static ApiResult ToSuccess(string message = "")
        {
            return new ApiResult(message: message);
        }

        public static ApiResult ToFailed(ApiStatusEnum code, string message)
        {
            return new ApiResult((int)code, message);
        }

    }

    /// <summary>
    /// API 返回JSON字符串
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ApiResult<T> : ApiResult where T : class
    {
        /// <summary>
        /// 数据集
        /// </summary>
        public T Data { get; set; }

        public ApiResult() : base((int)ApiStatusEnum.Success)
        {
        }
        public ApiResult(int code, T data, string message = "") : base(code, message)
        {
            Data = data;
        }

        /// <summary>
        /// 成功返参
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static ApiResult<T> ToSuccess(T data, string message = "")
        {
            return new ApiResult<T>((int)ApiStatusEnum.Success, data, message);
        }

        /// <summary>
        /// 失败返参
        /// </summary>
        /// <param name="code"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public new static ApiResult<T> ToFailed(ApiStatusEnum code, string message)
        {
            if (string.IsNullOrWhiteSpace(message))
            {
                throw new ArgumentNullException("请填写错误详细信息");
            }
            return new ApiResult<T>((int)code, null, message);
        }
    }


    public class PageListResult<T> : ApiResult where T : class
    {
        /// <summary>
        /// 数据集
        /// </summary>
        public IList<T> Data { get; set; }

        public Pager Pager { get; set; }
        public dynamic MapToList { get; set; }

        public PageListResult() : this((int)ApiStatusEnum.Success, null, null)
        {

        }

        public PageListResult(int code, IList<T> data, Pager page, string message = "") : base(code, message)
        {
            Data = data;
            Pager = page;
        }

        /// <summary>
        /// 成功返参
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static PageListResult<T> ToSuccess(IList<T> data, Pager page, string message = "")
        {
            return new PageListResult<T>((int)ApiStatusEnum.Success, data, page, message);
        }


        /// <summary>
        /// 失败返参
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static PageListResult<T> ToFailed(int code, string message)
        {
            if (string.IsNullOrWhiteSpace(message))
            {
                throw new ArgumentNullException("请填写错误详细信息");
            }
            return new PageListResult<T>(code, null, new Pager(), message);
        }

    }

}
public enum ApiStatusEnum
{
    /// <summary>
    /// 请求(或处理)成功
    /// </summary>
    [Description("请求(或处理)成功")]
    [Note("Success")]
    Success = 200, //请求(或处理)成功

    /// <summary>
    /// 内部请求出错
    /// </summary>
    [Description("内部请求出错")]
    [Note("System Error")]
    ServerError = 500, //内部请求出错

    /// <summary>
    /// 未授权标识
    /// </summary>
    [Description("未授权标识")]
    [Note("Unauthorized")]
    Unauthorized = 401,//未授权标识

    /// <summary>
    /// 请求参数不完整或不正确
    /// </summary>
    [Description("请求参数不完整或不正确")]
    [Note("Parameter Error")]
    ParameterError = 400,//请求参数不完整或不正确

    /// <summary>
    /// 请求TOKEN失效
    /// </summary>
    [Description("请求TOKEN失效")]
    [Note("Token Invalid")]
    TokenInvalid = 403,//请求TOKEN失效

    [Description("Url NotFound")]
    [Note("NotFound")]
    NotFound = 404,//NotFound

    /// <summary>
    /// HTTP请求类型不合法
    /// </summary>
    [Description("HTTP请求类型不合法")]
    [Note("Http Mehtod Error")]
    HttpMehtodError = 405,//HTTP请求类型不合法

    /// <summary>
    /// HTTP请求不合法,请求参数可能被篡改
    /// </summary>
    [Description("HTTP请求不合法,请求参数可能被篡改")]
    [Note("Http Request Error")]
    HttpRequestError = 406,//HTTP请求不合法

    /// <summary>
    /// 该URL已经失效
    /// </summary>
    [Description("该URL已经失效")]
    [Note("URL ExpireError")]
    URLExpireError = 407,//HTTP请求不合法
}
