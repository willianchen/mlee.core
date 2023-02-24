using  mlee.Core.DB.Transaction;
using  mlee.Core.Infrastructure.Entities.UserRole;
using mlee.Core.Admin.Repository;

namespace mlee.Core.Repository;

public class UserRoleRepository : CommonRepositoryBase<UserRoleEntity>, IUserRoleRepository
{
    public UserRoleRepository(UnitOfWorkManagerCloud uowm) : base(uowm)
    {

    }
}