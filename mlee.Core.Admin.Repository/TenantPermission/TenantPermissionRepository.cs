using mlee.Core.Admin.Repository;
using  mlee.Core.DB.Transaction;
using  mlee.Core.Infrastructure.Entities.TenantPermission;

namespace mlee.Core.Repository;

public class TenantPermissionRepository : CommonRepositoryBase<TenantPermissionEntity>, ITenantPermissionRepository
{
    public TenantPermissionRepository(UnitOfWorkManagerCloud uowm) : base(uowm)
    {

    }
}