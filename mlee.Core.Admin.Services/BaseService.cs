using Microsoft.Extensions.DependencyInjection;
using mlee.Core.Library.Cache;
using mlee.Core.Library.Logs;
using mlee.Core.Library.ObjExtensions;
using mlee.Core.Library.Sessions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mlee.Core.Admin.Services
{
    public abstract class BaseService : IBaseService
    {
        protected readonly object ServiceProviderLock = new object();
        protected IDictionary<Type, object> CachedServices = new Dictionary<Type, object>();
        private ICache _cache;
        private ILog _logger;
        //private IMapper _mapper;
        private ISession _user;

        /// <summary>
        /// 缓存
        /// </summary>
        public ICache Cache => LazyGetRequiredService(ref _cache);

        /*  /// <summary>
          /// 日志工厂
          /// </summary>
          public ILoggerFactory LoggerFactory => LazyGetRequiredService(ref _loggerFactory);*/

        /// <summary>
        /// 映射
        /// </summary>
        //public IMapper Mapper => LazyGetRequiredService(ref _mapper);

        public IServiceProvider ServiceProvider { get; set; }

        /// <summary>
        /// 用户信息
        /// </summary>
        public ISession User => LazyGetRequiredService(ref _user);

        /// <summary>
        /// 日志
        /// </summary>
        protected ILog Logger => LazyGetRequiredService(ref _logger);

        //  private Lazy<ILog> _lazyLogger => new Lazy<ILog>(() => LoggerFactory?.CreateLogger(GetType().FullName) ?? NullLogger.Instance, true);

        protected TService LazyGetRequiredService<TService>(ref TService reference)
        {
            if (reference == null)
            {
                lock (ServiceProviderLock)
                {
                    if (reference == null)
                    {
                        reference = ServiceProvider.GetRequiredService<TService>();
                    }
                }
            }

            return reference;
        }

        /// <summary>
        /// 获得懒加载服务
        /// </summary>
        /// <typeparam name="TService">服务接口</typeparam>
        /// <returns></returns>
       // [NonAction]
        public virtual TService LazyGetRequiredService<TService>()
        {
            return (TService)LazyGetRequiredService(typeof(TService));
        }

        /// <summary>
        /// 根据服务类型获得懒加载服务
        /// </summary>
        /// <param name="serviceType">服务类型</param>
        /// <returns></returns>
       // [NonAction]
        public virtual object LazyGetRequiredService(Type serviceType)
        {
            return CachedServices.GetOrAdd(serviceType, () => ServiceProvider.GetRequiredService(serviceType));
        }
    }
}