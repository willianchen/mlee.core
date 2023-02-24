using FreeScheduler;
using mlee.Core.Admin.Repository;
using  mlee.Core.DB.Transaction;

namespace mlee.Core.Repository;

public class TaskRepository : CommonRepositoryBase<TaskInfo>, ITaskRepository
{
    public TaskRepository(UnitOfWorkManagerCloud uowm) : base(uowm)
    {
    }
}