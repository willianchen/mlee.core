using mlee.Core.Admin.Repository;
using mlee.Core.DB.Transaction;
using mlee.Core.Infrastructure.Entities.Api;

namespace mlee.Core.Repository;

public class ApiRepository : CommonRepositoryBase<ApiEntity>, IApiRepository
{
    public ApiRepository(UnitOfWorkManagerCloud uowm) : base(uowm)
    {
    }
}