using mlee.Core.Library.Helpers;
using System;
using System.Collections.Generic;
using System.Text;

namespace mlee.Core.Library.Sessions
{
    /// <summary>
    /// 用户会话
    /// </summary>
    public class Session : ISession
    {
        /// <summary>
        /// 空用户会话
        /// </summary>
        public static readonly ISession Null = NullSession.Instance;

        /// <summary>
        /// 用户会话
        /// </summary>
        public static readonly ISession Instance = new Session();

        /// <summary>
        /// 是否认证
        /// </summary>
        public bool IsAuthenticated { get; } = false;// Web.Identity.IsAuthenticated;

        public string UserName { get; set; }

        /// <summary>
        /// 手机号码
        /// </summary>
        public string Mobile { get; set; }

        public string RoleName => string.Empty;

        public string ExtData => string.Empty;

        public long UserId { get; set; } = 0;
        public long TenantId { get; set; } = 0;
        public bool PlatformAdmin { get; set; } = false;

        long ISession.UserId => throw new NotImplementedException();
    }
}