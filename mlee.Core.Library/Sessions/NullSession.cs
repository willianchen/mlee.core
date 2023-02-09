﻿

namespace mlee.Core.Library.Sessions
{
    /// <summary>
    /// 空用户会话
    /// </summary>
    public class NullSession : ISession
    {
        /// <summary>
        /// 是否认证
        /// </summary>
        public bool IsAuthenticated => false;

        /// <summary>
        /// 用户编号
        /// </summary>
     //   public string UserId => string.Empty;

        /// <summary>
        /// 登录用户名
        /// </summary>
        public string UserName => string.Empty;

        public string Mobile => string.Empty;

        /// <summary>
        /// 角色名称
        /// </summary>
        public string RoleName => string.Empty;

        public string ExtData => string.Empty;

        public long UserId { get; set; } = 0;
        public long TenantId { get; set; } = 0;
        public bool PlatformAdmin { get; set; } = false;

        /// <summary>
        /// 空用户会话实例
        /// </summary>
        public static readonly ISession Instance = new NullSession();
    }
}