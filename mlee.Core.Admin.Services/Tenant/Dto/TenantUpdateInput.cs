using System.ComponentModel.DataAnnotations;
using mlee.Core.Library.Validations.Validators;

namespace mlee.Core.Services.Tenant.Dto;

/// <summary>
/// 修改
/// </summary>
public partial class TenantUpdateInput : TenantAddInput
{
    /// <summary>
    /// 接口Id
    /// </summary>
    [Required]
    [ValidateRequired("请选择租户")]
    public long Id { get; set; }
}