using AspectCore.DynamicProxy;
using mlee.Core.Library.Cache;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mlee.Core.Library.Aspects
{
    public class CacheInterceptAttribute : InterceptorBase
    {
        //通过注入的方式，把缓存操作接口通过构造函数注入
        private ICache _cache;
        public CacheInterceptAttribute(ICache cache)
        {
            _cache = cache;
        }

        public override Task Invoke(AspectContext context, AspectDelegate next)
        {
            //获取自定义缓存键
            var cacheKey = CustomCacheKey(context);
            //根据key获取相应的缓存值
            var cacheValue = _cache.GetOrDefault<object>(cacheKey);
            if (cacheValue != null)
            {
                //将当前获取到的缓存值，赋值给当前执行方法
                context.ReturnValue = cacheValue;
                return next(context);
            }

            //存入缓存
            if (!string.IsNullOrWhiteSpace(cacheKey))
            {
                _cache.SetSlidingCache(cacheKey, context.ReturnValue);
            }
            return next(context);
        }

        //自定义缓存键
        private string CustomCacheKey(AspectContext context)
        {
            var typeName = context.GetType().Name;
            var methodName = context.ServiceMethod.Name;
            //invocation.Method.Name;
            var methodArguments = context.Parameters.Take(3).ToList();
            //invocation.Arguments.Select(GetArgumentValue).Take(3).ToList();//获取参数列表，我最多需要三个即可

            string key = $"{typeName}:{methodName}:";
            foreach (var param in methodArguments)
            {
                key += $"{param}:";
            }

            return key.TrimEnd(':');
        }

        //object 转 string
        private string GetArgumentValue(object arg)
        {
            // PS：这里仅仅是很简单的数据类型，如果参数是表达式/类等，比较复杂的，请看我的在线代码吧，封装的比较多，当然也可以自己封装。
            if (arg is int || arg is long || arg is string)
                return arg.ToString();

            if (arg is DateTime)
                return ((DateTime)arg).ToString("yyyyMMddHHmmss");

            return "";
        }
    }
}
