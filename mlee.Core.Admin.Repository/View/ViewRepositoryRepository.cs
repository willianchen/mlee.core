using mlee.Core.Admin.Repository;
using mlee.Core.DB.Transaction;
using mlee.Core.Infrastructure.Entities.View;

namespace mlee.Core.Repository;

public class ViewRepository : CommonRepositoryBase<ViewEntity>, IViewRepository
{
    public ViewRepository(UnitOfWorkManagerCloud muowm) : base(muowm)
    {
    }
}