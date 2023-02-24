using Autofac;
using Autofac.Core.Registration;
using Microsoft.Extensions.DependencyModel;
using mlee.Core.DB.Transaction;
using mlee.Core.Library.Configs;
using mlee.Core.Library.Dependency;
using mlee.Core.Library.Reflections;
using mlee.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Autofac.Extras.DynamicProxy;

namespace mlee.Core.RegisterModules
{

    public class RegisterModule : ConfigBase
    {
        private readonly AppConfig _appConfig;

        /// <summary>
        /// 模块注入
        /// </summary>
        /// <param name="appConfig">AppConfig</param>
        public RegisterModule(AppConfig appConfig)
        {
            _appConfig = appConfig;
        }


        protected override void Load(ContainerBuilder builder)
        {
            //事务拦截
            var interceptorServiceTypes = new List<Type>();
            if (_appConfig.Aop.Transaction)
            {
                builder.RegisterType<TransactionInterceptor>();
                builder.RegisterType<TransactionAsyncInterceptor>();
                interceptorServiceTypes.Add(typeof(TransactionInterceptor));
            }

            /* if (_appConfig.AssemblyNames?.Length > 0)
             {
                 */
            //程序集
            Assembly[] assemblies = new Finder().GetAssemblies().ToArray();
            /*DependencyContext.Default.RuntimeLibraries
                .Where(a => _appConfig.AssemblyNames.Contains(a.Name))
                .Select(o => Assembly.Load(new AssemblyName(o.Name))).ToArray();
*/
            /* var nonRegisterIOCAttribute = typeof(NonRegisterIOCAttribute);
             var iRegisterIOCType = typeof(Idep);*/

            bool Predicate(Type a) => (a.Name.EndsWith("Service") || a.Name.EndsWith("Repository"))
                && !a.IsAbstract && !a.IsInterface && a.IsPublic;

            //有接口实例
            builder.RegisterAssemblyTypes(assemblies)
            .Where(Predicate)
            .AsImplementedInterfaces()
            .InstancePerLifetimeScope()
            .PropertiesAutowired()// 属性注入
            .InterceptedBy(interceptorServiceTypes.ToArray())
            .EnableInterfaceInterceptors();

            //无接口实例
            builder.RegisterAssemblyTypes(assemblies)
            .Where(Predicate)
            .InstancePerLifetimeScope()
            .PropertiesAutowired()// 属性注入
            .InterceptedBy(interceptorServiceTypes.ToArray())
            .EnableClassInterceptors();

            //仓储泛型注入
            /*   builder.RegisterGeneric(typeof(RepositoryBase<>)).As(typeof(IRepositoryBase<>)).InstancePerLifetimeScope().PropertiesAutowired();
               builder.RegisterGeneric(typeof(RepositoryBase<,>)).As(typeof(IRepositoryBase<,>)).InstancePerLifetimeScope().PropertiesAutowired();*/
            //  }
        }
    }
}
