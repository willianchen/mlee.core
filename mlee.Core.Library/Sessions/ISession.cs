
namespace mlee.Core.Library.Sessions
{
    /// <summary>
    /// 用户会话
    /// </summary>
    public interface ISession
    {
        /// <summary>
        /// 是否认证
        /// </summary>
        bool IsAuthenticated { get; }
        /// <summary>
        /// 用户标识
        /// </summary>
        long UserId { get; }

        /// <summary>
        /// 用户名
        /// </summary>
        string UserName { get; }

        /// <summary>
        /// 手机号码
        /// </summary>
        string Mobile { get; }

        /// <summary>
        /// 角色名称
        /// </summary>
        string RoleName { get; }

        /// <summary>
        /// 拓展参数
        /// </summary>
        string ExtData { get; }

        long TenantId { get; set; }
        /// <summary>
        /// 平台管理员
        /// </summary>
        public bool PlatformAdmin { get; set; }
    }
}