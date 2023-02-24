using  mlee.Core.DB.Transaction;
using  mlee.Core.Infrastructure.Entities.PermissionApi;
using mlee.Core.Admin.Repository;

namespace mlee.Core.Repository;

public class PermissionApiRepository : CommonRepositoryBase<PermissionApiEntity>, IPermissionApiRepository
{
    public PermissionApiRepository(UnitOfWorkManagerCloud uowm) : base(uowm)
    {

    }
}