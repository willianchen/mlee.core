using  mlee.Core.DB.Transaction;
using  mlee.Core.Infrastructure.Entities.OprationLog;
using mlee.Core.Admin.Repository;

namespace mlee.Core.Repository;

public class OprationLogRepository : CommonRepositoryBase<OprationLogEntity>, IOprationLogRepository
{
    public OprationLogRepository(UnitOfWorkManagerCloud uowm) : base(uowm)
    {
    }
}