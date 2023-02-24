using mlee.Core.Admin.Repository;
using  mlee.Core.DB.Transaction;
using  mlee.Core.Infrastructure.Entities.UserStaff;

namespace mlee.Core.Repository;

public class UserStaffRepository : CommonRepositoryBase<UserStaffEntity>, IUserStaffRepository
{
    public UserStaffRepository(UnitOfWorkManagerCloud uowm) : base(uowm)
    {

    }
}