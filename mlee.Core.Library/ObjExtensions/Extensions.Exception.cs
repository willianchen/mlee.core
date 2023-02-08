﻿using mlee.Core.Library.Exceptions.Prompts;
using System;
using System.Collections.Generic;
using System.Text;

namespace mlee.Core.Library
{
    /// <summary>
    /// 异常扩展
    /// </summary>
    public static partial class Extensions
    {
        /// <summary>
        /// 获取原始异常
        /// </summary>
        /// <param name="exception">异常</param>
        public static Exception GetRawException(this Exception exception)
        {
            if (exception == null)
                return null;
            if (exception is AspectCore.DynamicProxy.AspectInvocationException aspectInvocationException)
            {
                if (aspectInvocationException.InnerException == null)
                    return aspectInvocationException;
                return GetRawException(aspectInvocationException.InnerException);
            }
            return exception;
        }

        /// <summary>
        /// 获取异常提示
        /// </summary>
        /// <param name="exception">异常</param>
        public static string GetPrompt(this Exception exception)
        {
            return ExceptionPrompt.GetPrompt(exception);
        }
    }
}