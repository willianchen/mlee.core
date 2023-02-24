using System.ComponentModel.DataAnnotations;
using mlee.Core.Infrastructure.Entities.User;
using mlee.Core.Services.User;

namespace mlee.Core.Services.User.Dto;

/// <summary>
/// 添加
/// </summary>
public class UserAddInput: UserFormInput
{
    /// <summary>
    /// 密码
    /// </summary>
    [Required(ErrorMessage = "请输入密码")]
    public virtual string Password { get; set; }

    /// <summary>
    /// 状态
    /// </summary>
    public UserStatus Status { get; set; } = UserStatus.Enabled;
}