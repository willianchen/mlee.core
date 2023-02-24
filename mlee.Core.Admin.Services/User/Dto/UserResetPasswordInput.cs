using mlee.Core.Library.Entities;

namespace mlee.Core.Services.User.Dto;

/// <summary>
/// 重置密码
/// </summary>
public class UserResetPasswordInput : Entity
{
    /// <summary>
    /// 密码
    /// </summary>
    public string Password { get; set; }
}