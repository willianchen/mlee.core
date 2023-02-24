using mlee.Core.Library.Entities;
using FreeSql.DataAnnotations;
using mlee.Core.Infrastructure.Entities.Tenant;
using mlee.Core.Infrastructure.Entities.Permission;

namespace mlee.Core.Infrastructure.Entities.TenantPermission;

/// <summary>
/// 租户权限
/// </summary>
[Table(Name = "ad_tenant_permission")]
[Index("idx_{tablename}_01", nameof(TenantId) + "," + nameof(PermissionId), true)]
public class TenantPermissionEntity : EntityAdd
{
    /// <summary>
    /// 租户Id
    /// </summary>
	public long TenantId { get; set; }

    /// <summary>
    /// 权限Id
    /// </summary>
		public long PermissionId { get; set; }

    /// <summary>
    /// 租户
    /// </summary>
    public TenantEntity Tenant { get; set; }

    /// <summary>
    /// 权限
    /// </summary>
    public PermissionEntity Permission { get; set; }
}