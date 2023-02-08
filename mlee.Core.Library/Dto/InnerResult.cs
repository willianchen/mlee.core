using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace mlee.Core.Library.Dto
{
    /// <summary>
    /// 内部结果集
    /// </summary>
    public class InnerResult
    {
        /// <summary>
        /// 结果
        /// </summary>
        public bool Code { get; set; }

        /// <summary>
        /// 错误消息
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="code"></param>
        /// <param name="message"></param>
        public InnerResult(bool code, string message)
        {
            Code = code;
            Message = message;
        }

        /// <summary>
        /// 成功返回值
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public static InnerResult ToSuccess(string message = "成功消息")
        {
            return new InnerResult(true, message);
        }

        /// <summary>
        /// 失败结果
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public static InnerResult ToFailed(string message)
        {
            return new InnerResult(false, message);
        }
    }

    /// <summary>
    /// 返回结果
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class InnerResult<T> : InnerResult where T : class
    {
        public InnerResult(bool code, string message, T data) : base(code, message)
        {
            Data = data;
        }

        /// <summary>
        /// 返回数据
        /// </summary>
        public T Data { get; set; }

        /// <summary>
        /// 成功返回
        /// </summary>
        /// <param name="data"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public static InnerResult<T> ToSuccess(T data, string message = "成功消息")
        {
            return new InnerResult<T>(true, message, data);
        }

        /// <summary>
        /// 失败结果
        /// </summary>
        /// <param name="data"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public new static InnerResult<T> ToFailed(string message = "成功消息")
        {
            return new InnerResult<T>(false, message, null);
        }
    }
}
