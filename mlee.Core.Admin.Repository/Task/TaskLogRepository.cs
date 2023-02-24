using FreeScheduler;
using mlee.Core.Admin.Repository;
using  mlee.Core.DB.Transaction;

namespace mlee.Core.Repository;

public class TaskLogRepository : CommonRepositoryBase<TaskLog>, ITaskLogRepository
{
    public TaskLogRepository(UnitOfWorkManagerCloud uowm) : base(uowm)
    {
    }
}