using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using mlee.Core.DB;
using mlee.Core.DB.Transaction;
using mlee.Core.Library.Configs;
using mlee.Core.Library.Helpers;
using mlee.Core.Library.Sessions;
using mlee.Core.StartUp;
using Org.BouncyCastle.Utilities.Collections;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace mlee.Core.Db
{

    public static class DBServiceCollectionExtensions
    {
        /// <summary>
        /// 添加数据库
        /// </summary>
        /// <param name="services"></param>
        /// <param name="env"></param>
        /// <param name="hostAppOptions"></param>
        /// <returns></returns>
        public static void AddDb(this IServiceCollection services, IHostEnvironment env, HostAppOptions hostAppOptions)
        {
            var dbConfig = ConfigHelper.Get<DbConfig>("dbconfig", env.EnvironmentName);
            var appConfig = ConfigHelper.Get<AppConfig>("appconfig", env.EnvironmentName);
            var user = services.BuildServiceProvider().GetService<ISession>();
            var freeSqlCloud = string.IsNullOrWhiteSpace(appConfig.DistributeKey) ? new FreeSqlCloud() : new FreeSqlCloud(appConfig.DistributeKey);
            DbHelper.RegisterDb(freeSqlCloud, user, dbConfig, appConfig, hostAppOptions);

            //注册多数据库
            if (dbConfig.Dbs?.Length > 0)
            {
                foreach (var db in dbConfig.Dbs)
                {
                    DbHelper.RegisterDb(freeSqlCloud, user, db, appConfig, null);
                }
            }

            services.AddSingleton<IFreeSql>(freeSqlCloud);
            services.AddSingleton(freeSqlCloud);
            services.AddScoped<UnitOfWorkManagerCloud>();
            //定义基础仓储主库
            var fsql = freeSqlCloud.Use(dbConfig.Key);
            services.AddSingleton(provider => fsql);
        }

        /// <summary>
        /// 添加TiDb数据库
        /// </summary>
        /// <param name="_"></param>
        /// <param name="context"></param>
        /// <param name="version">版本</param>
        public static void AddTiDb(this IServiceCollection _, HostAppContext context, string version = "8.0")
        {
            var dbConfig = ConfigHelper.Get<DbConfig>("dbconfig", context.Environment.EnvironmentName);
            var _dicMySqlVersion = typeof(FreeSqlGlobalExtensions).GetField("_dicMySqlVersion", BindingFlags.NonPublic | BindingFlags.Static);
            var dicMySqlVersion = new ConcurrentDictionary<string, string>();
            dicMySqlVersion[dbConfig.ConnectionString] = version;
            _dicMySqlVersion.SetValue(new ConcurrentDictionary<string, string>(), dicMySqlVersion);
        }
    }
}