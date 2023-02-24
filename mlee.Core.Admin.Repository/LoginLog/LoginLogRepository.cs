using mlee.Core.Admin.Repository;
using mlee.Core.DB.Transaction;
using mlee.Core.Infrastructure.Entities.LoginLog;

namespace mlee.Core.Repository;

public class LoginLogRepository : CommonRepositoryBase<LoginLogEntity>, ILoginLogRepository
{
    public LoginLogRepository(UnitOfWorkManagerCloud uowm) : base(uowm)
    {
    }
}