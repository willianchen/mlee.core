using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace mlee.Core.Library.Cache
{
    /// <summary>
    /// 缓存接口类
    /// </summary>
    public interface ICache
    {
        /// <summary>
        /// 唯一缓存名称
        /// </summary>
        string Name { get; }

        /// <summary>
        /// 默认相对缓存失效时间
        /// </summary>
        TimeSpan DefaultSlidingExpireTime { get; }


        /// <summary>
        /// 获取缓存信息
        /// </summary>
        /// <param name="key">关键字</param>
        /// <param name="func"></param>
        /// <returns></returns>

        T Get<T>(string key, Func<string, T> func, TimeSpan? slidingExpireTime = null);


        /// <summary>
        /// 获取缓存信息（异步）
        /// </summary>
        /// <param name="key"></param>
        /// <param name="func"></param>
        /// <returns></returns>
        Task<T> GetAsync<T>(string key, Func<string, Task<T>> func, TimeSpan? slidingExpireTime = null);

        /// <summary>
        /// 获取缓存集合信息
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="func"></param>
        /// <param name="slidingExpireTime"></param>
        /// <returns></returns>
        Task<List<T>> ListGetAsync<T>(string key, Func<string, Task<List<T>>> func, TimeSpan? slidingExpireTime = null);

        /// <summary>
        /// 获取集合缓存
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        Task<List<T>>  ListGetOrDefaultAsync<T>(string key);

        /// <summary>
        /// 获取集合缓存
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        List<T> ListGetOrDefault<T>(string key);


        /// <summary>
        /// 获取缓存信息，未找到返回空
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        T GetOrDefault<T>(string key);

        /// <summary>
        /// 获取缓存信息，未找到返回空（异步）
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        Task<T> GetOrDefaultAsync<T>(string key);


        /// <summary>
        /// 获取缓存信息，未找到返回空
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        string GetOrDefaultString(string key);

        /// <summary>
        /// 获取缓存信息，未找到返回空（异步）
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        Task<string> GetOrDefaultStringAsync(string key);

        /// <summary>
        /// 设置相对缓存
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="slidingExpireTime"></param>
        void SetSlidingCache<T>(string key, T value, TimeSpan? slidingExpireTime = null);

        /// <summary>
        /// 设置相对缓存（异步）
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="slidingExpireTime"></param>
        Task<bool> SetSlidingCacheAsync<T>(string key, T value, TimeSpan? slidingExpireTime = null);

        /// <summary>
        /// 集合设置相对缓存（异步）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="slidingExpireTime"></param>
        /// <returns></returns>
        Task<bool> ListSetSlidingCacheAsync<T>(string key, List<T> value, TimeSpan? slidingExpireTime = null);

        /// <summary>
        /// 设置绝对缓存
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="absoluteExpireTime"></param>
        void SetAbsoluteCache<T>(string key, T value, DateTimeOffset? absoluteExpireTime = null);

        /// <summary>
        /// 设置绝对缓存（异步）
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="absoluteExpireTime"></param>
        Task<bool> SetAbsoluteCacheAsync<T>(string key, T value, DateTimeOffset? absoluteExpireTime = null);

        /// <summary>
        /// 集合设置绝对缓存
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="absoluteExpireTime"></param>
        /// <returns></returns>
        Task<bool> ListSetAbsoluteCacheAsync<T>(string key, List<T> value, DateTimeOffset? absoluteExpireTime = null);

        /// <summary>
        /// 移除缓存
        /// </summary>
        /// <param name="key">Key</param>
        void Remove(string key);

        /// <summary>
        /// 移除缓存（异步）
        /// </summary>
        /// <param name="key">Key</param>
        Task<bool> RemoveAsync(string key);

    }
}
