using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using StackExchange.Redis;
using mlee.Core.Redis.RedisConfig;
using System.Linq;

namespace mlee.Core.Redis
{
    public static class ExtentionsRedisCache
    {
        public static IServiceCollection AddDistributedRedisCache(this IServiceCollection service)
        {
            return service.AddStackExchangeRedisCache(options =>
            {
                //获取配置信息
                var redisConnectionHelp = service.BuildServiceProvider().GetService<IRedisConnectionHelp>();
                options.ConfigurationOptions = new ConfigurationOptions
                {
                    AllowAdmin = true,
                    Password = redisConnectionHelp.RedisPassword,
                    DefaultDatabase = redisConnectionHelp.RedisDefaultDataBase,
                    ConnectTimeout = 300
                };
                var redisConnectionConfigs = redisConnectionHelp.RedisConnectionConfig.Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries);
                if (redisConnectionConfigs == null && redisConnectionConfigs.Count() <= 0)
                {
                    throw new ArgumentException("Redis配置错误");
                }
                redisConnectionConfigs.ToList().ForEach(a =>
                {
                    options.ConfigurationOptions.EndPoints.Add(a);
                });
            });
        }
    }
}
