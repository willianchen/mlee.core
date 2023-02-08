using HuntNet.Library.Cache;
using mlee.Core.Redis.IRedisManage;
using Nito.AsyncEx;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace mlee.Core.Redis.Cache
{
    /// <summary>
    /// Redis缓存
    /// </summary>
    public class RedisCache : ICache
    {
        private readonly IRedisOperationHelp _redis;

        public RedisCache(IRedisOperationHelp redis)
        {
            _redis = redis;
            DefaultSlidingExpireTime = TimeSpan.FromDays(1);
        }

        /// <summary>
        /// Key前缀
        /// </summary>
        public string Name { get; } = "Cache";

        /// <summary>
        /// 默认缓存过期时间
        /// </summary>
        public TimeSpan DefaultSlidingExpireTime { get; }

        private readonly object SyncObj = new object();
        private readonly AsyncLock _asyncLock = new AsyncLock();


        /// <summary>
        /// 约束Key
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        private string GetKey(string key)
        {
            return $"{Name}:{key}";
        }
        /// <summary>
        /// 获取对象
        /// </summary>
        /// <param name="key"></param>
        /// <param name="func">获取对象方法</param>
        /// <returns></returns>
        public T Get<T>(string key, Func<string, T> func, TimeSpan? slidingExpireTime = null)
        {
            T item = default(T);

            item = GetOrDefault<T>(key);

            if (item == null)
            {
                lock (SyncObj)
                {
                    item = GetOrDefault<T>(key);

                    if (item == null)
                    {
                        item = func(key);

                        if (item == null)
                        {
                            return default(T);
                        }
                        SetSlidingCache(key, item, slidingExpireTime);
                    }
                }
            }

            return item;
        }

        /// <summary>
        /// 获取对象
        /// </summary>
        /// <param name="key"></param>
        /// <param name="func">获取对象方法</param>
        /// <returns></returns>
        public async Task<T> GetAsync<T>(string key, Func<string, Task<T>> func, TimeSpan? slidingExpireTime = null)
        {
            var item = default(T);
            item = await GetOrDefaultAsync<T>(key);


            if (item == null)
            {
                using (await _asyncLock.LockAsync())
                {
                    item = await GetOrDefaultAsync<T>(key);


                    if (item == null)
                    {
                        item = await func(key);

                        if (item == null)
                        {
                            return default(T);
                        }
                        await SetSlidingCacheAsync(key, item, slidingExpireTime);
                    }
                }
            }

            return item;
        }


        /// <summary>
        /// 获取缓存集合
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="func"></param>
        /// <param name="slidingExpireTime"></param>
        /// <returns></returns>
        public async Task<List<T>> ListGetAsync<T>(string key, Func<string, Task<List<T>>> func, TimeSpan? slidingExpireTime = null)
        {

            List<T> item = default(List<T>);
            item = ListGetOrDefault<T>(key);
            if (item == null)
            {
                using (await _asyncLock.LockAsync())
                {
                    item = await ListGetOrDefaultAsync<T>(key);
                    if (item == null)
                    {
                        item = await func(key);

                        if (item == null)
                        {
                            return default(List<T>);
                        }
                        await ListSetSlidingCacheAsync(key, item, slidingExpireTime);
                    }
                }
            }
            return item;
        }


        /// <summary>
        /// 获取集合缓存
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public Task<List<T>> ListGetOrDefaultAsync<T>(string key)
        {
            key = GetKey(key);
            return _redis.SetGetAsync<T>(key);
        }

        /// <summary>
        /// 获取缓存集合
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public List<T> ListGetOrDefault<T>(string key)
        {
            key = GetKey(key);
            return _redis.SetGet<T>(key);
        }
        /// <summary>
        /// 获取对象
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public T GetOrDefault<T>(string key)
        {
            key = GetKey(key);
            return _redis.StringGet<T>(key);
        }

        /// <summary>
        /// 异步获取对象
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public async Task<T> GetOrDefaultAsync<T>(string key)
        {
            key = GetKey(key);
            return await Task.FromResult(_redis.StringGet<T>(key));
        }

        /// <summary>
        /// 获取缓存信息，未找到返回空
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public string GetOrDefaultString(string key)
        {
            key = GetKey(key);
            return _redis.StringGet(key);
        }

        /// <summary>
        /// 获取缓存信息，未找到返回空（异步）
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public async Task<string> GetOrDefaultStringAsync(string key)
        {

            key = GetKey(key);
            return await Task.FromResult(_redis.StringGet(key));
        }

        /// <summary>
        /// 移除对象
        /// </summary>
        /// <param name="key"></param>
        public void Remove(string key)
        {
            key = GetKey(key);
            _redis.KeyRemove(key);
        }

        /// <summary>
        /// 异步移除对象
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public async Task<bool> RemoveAsync(string key)
        {
            key = GetKey(key);
            return await _redis.KeyRemoveAsync(key);
        }

        /// <summary>
        /// 设置缓存对象
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="absoluteExpireTime">过期时间</param>
        public void SetAbsoluteCache<T>(string key, T value, DateTimeOffset? absoluteExpireTime = null)
        {
            key = GetKey(key);
            var expireTime = absoluteExpireTime == null ? DefaultSlidingExpireTime : absoluteExpireTime.Value.Subtract(DateTimeOffset.Now);
            _redis.StringSet<T>(key, value, expireTime);
        }

        /// <summary>
        /// 异步设置缓存对象
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="absoluteExpireTime"></param>
        /// <returns></returns>
        public async Task<bool> SetAbsoluteCacheAsync<T>(string key, T value, DateTimeOffset? absoluteExpireTime = null)
        {
            key = GetKey(key);
            var expireTime = absoluteExpireTime == null ? DefaultSlidingExpireTime : absoluteExpireTime.Value.Subtract(DateTimeOffset.Now);
            return await _redis.StringSetAsync<T>(key, value, expireTime);
        }

        /// <summary>
        /// 设置绝对时间缓存
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="absoluteExpireTime"></param>
        /// <returns></returns>
        public async Task<bool> ListSetAbsoluteCacheAsync<T>(string key, List<T> value, DateTimeOffset? absoluteExpireTime = null)
        {

            key = GetKey(key);
            var expireTime = absoluteExpireTime == null ? DefaultSlidingExpireTime : absoluteExpireTime.Value.Subtract(DateTimeOffset.Now);
            var result = await _redis.SetAddAsync(key, value);
            if (!(result > 0))
                return false;
            return await _redis.KeyExpireAsync(key, expireTime, Redis.KeyOperatorEnum.Set);

        }


        /// <summary>
        /// 设置相对缓存
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="slidingExpireTime"></param>
        public void SetSlidingCache<T>(string key, T value, TimeSpan? slidingExpireTime = null)
        {
            slidingExpireTime = slidingExpireTime ?? DefaultSlidingExpireTime;
            key = GetKey(key);
            _redis.StringSet<T>(key, value, slidingExpireTime);
        }

        /// <summary>
        /// 设置相对缓存
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="slidingExpireTime"></param>
        /// <returns></returns>
        public Task<bool> SetSlidingCacheAsync<T>(string key, T value, TimeSpan? slidingExpireTime = null)
        {
            key = GetKey(key);
            slidingExpireTime = slidingExpireTime ?? DefaultSlidingExpireTime;
            return _redis.StringSetAsync<T>(key, value, slidingExpireTime);
        }


        /// <summary>
        /// 设置相对缓存
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="slidingExpireTime"></param>
        /// <returns></returns>
        public async Task<bool> ListSetSlidingCacheAsync<T>(string key, List<T> value, TimeSpan? slidingExpireTime = null)
        {
            key = GetKey(key);
            slidingExpireTime = slidingExpireTime ?? DefaultSlidingExpireTime;

            var result = await _redis.SetAddAsync(key, value);
            if (!(result > 0))
                return false;
            return await _redis.KeyExpireAsync(key, slidingExpireTime, Redis.KeyOperatorEnum.Set);
        }
    }
}
