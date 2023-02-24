using mlee.Core.DB.Transaction;
using mlee.Core.Infrastructure.Entities.RoleOrg;
using mlee.Core.Admin.Repository;

namespace mlee.Core.Repository;

public class RoleOrgRepository : CommonRepositoryBase<RoleOrgEntity>, IRoleOrgRepository
{
    public RoleOrgRepository(UnitOfWorkManagerCloud uowm) : base(uowm)
    {

    }
}