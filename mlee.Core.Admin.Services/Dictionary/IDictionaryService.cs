using mlee.Core.Library.Dto;
using System.Threading.Tasks;
using mlee.Core.Services.Dictionary.Dto;
using mlee.Core.Infrastructure.Entities.Dictionary.Dto;

namespace mlee.Core.Services.Dictionary;

/// <summary>
/// 数据字典接口
/// </summary>
public partial interface IDictionaryService
{
    Task<DictionaryGetOutput> GetAsync(long id);

    Task<PageOutput<DictionaryListOutput>> GetPageAsync(Pager<DictionaryGetPageDto> input);

    Task<long> AddAsync(DictionaryAddInput input);

    Task UpdateAsync(DictionaryUpdateInput input);

    Task DeleteAsync(long id);

    Task BatchDeleteAsync(long[] ids);

    Task SoftDeleteAsync(long id);

    Task BatchSoftDeleteAsync(long[] ids);
}