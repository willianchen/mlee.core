using FreeSql.DataAnnotations;

namespace mlee.Core.Infrastructure.Entities.LoginLog;

/// <summary>
/// 登录日志
/// </summary>
[Table(Name = "ad_login_log")]
public partial class LoginLogEntity : LogAbstract
{
}