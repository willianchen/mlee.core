using System.Collections.Generic;
using System.Threading.Tasks;
using  mlee.Core.DB.Transaction;
using  mlee.Core.Infrastructure.Entities.Org;
using mlee.Core.Admin.Repository;

namespace mlee.Core.Repository;

public class OrgRepository : CommonRepositoryBase<OrgEntity>, IOrgRepository
{
    public OrgRepository(UnitOfWorkManagerCloud uowm) : base(uowm)
    {
    }

    /// <summary>
    /// 获得本部门和下级部门Id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task<List<long>> GetChildIdListAsync(long id)
    {
        return await Select
        .Where(a => a.Id == id)
        .AsTreeCte()
        .ToListAsync(a => a.Id);
    }
}