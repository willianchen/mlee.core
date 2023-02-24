using System.Collections.Generic;
using System.Threading.Tasks;
using mlee.Core.Admin.Repository;
using  mlee.Core.DB.Transaction;
using  mlee.Core.Infrastructure.Entities.UserOrg;

namespace mlee.Core.Repository;

public class UserOrgRepository : CommonRepositoryBase<UserOrgEntity>, IUserOrgRepository
{
    public UserOrgRepository(UnitOfWorkManagerCloud uowm) : base(uowm)
    {
        
    }

    /// <summary>
    /// 本部门下是否有员工
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task<bool> HasUser(long id)
    {
        return await Select.Where(a => a.OrgId == id).AnyAsync();
    }

    /// <summary>
    /// 部门列表下是否有员工
    /// </summary>
    /// <param name="idList"></param>
    /// <returns></returns>
    public async Task<bool> HasUser(List<long> idList)
    {
        return await Select.Where(a => idList.Contains(a.OrgId)).AnyAsync();
    }
}