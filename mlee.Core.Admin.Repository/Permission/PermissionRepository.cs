using mlee.Core.DB.Transaction;
using mlee.Core.Infrastructure.Entities.Permission;
using mlee.Core.Admin.Repository;

namespace mlee.Core.Repository;

public class PermissionRepository : CommonRepositoryBase<PermissionEntity>, IPermissionRepository
{
    public PermissionRepository(UnitOfWorkManagerCloud uowm) : base(uowm)
    {
    }
}