using System.Threading.Tasks;
using mlee.Core.Library.Dto;
using mlee.Core.Infrastructure.Entities.Tenant.Dto;
using mlee.Core.Services.Tenant.Dto;

namespace mlee.Core.Services.Tenant;

/// <summary>
/// 租户接口
/// </summary>
public interface ITenantService
{
    Task<TenantGetOutput> GetAsync(long id);

    Task<PageOutput<TenantListOutput>> GetPageAsync(Pager<TenantGetPageDto> input);

    Task<long> AddAsync(TenantAddInput input);

    Task UpdateAsync(TenantUpdateInput input);

    Task DeleteAsync(long id);

    Task SoftDeleteAsync(long id);

    Task BatchSoftDeleteAsync(long[] ids);
}