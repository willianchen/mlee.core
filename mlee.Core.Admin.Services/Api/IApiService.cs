using System.Threading.Tasks;
using mlee.Core.Services.Api.Dto;
using System.Collections.Generic;
using mlee.Core.Infrastructure.Entities.Api.Dto;
using mlee.Core.Library.Dto;
using mlee.Core.Infrastructure.Entities.Api;

namespace mlee.Core.Services.Api;

/// <summary>
/// api接口
/// </summary>
public interface IApiService
{
    Task<ApiGetOutput> GetAsync(long id);

    Task<List<ApiListOutput>> GetListAsync(string key);

    Task<PageOutput<ApiEntity>> GetPageAsync(Pager<ApiGetPageDto> input);

    Task<long> AddAsync(ApiAddInput input);

    Task UpdateAsync(ApiUpdateInput input);

    Task DeleteAsync(long id);

    Task BatchDeleteAsync(long[] ids);

    Task SoftDeleteAsync(long id);

    Task BatchSoftDeleteAsync(long[] ids);

    Task SyncAsync(ApiSyncInput input);
}