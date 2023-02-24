using mlee.Core.Library.Dto;
using System.Threading.Tasks;
using mlee.Core.Services.DictionaryType.Dto;
using mlee.Core.Infrastructure.Entities.DictionaryType.Dto;

namespace mlee.Core.Services.DictionaryType;

/// <summary>
/// 数据字典类型接口
/// </summary>
public partial interface IDictionaryTypeService
{
    Task<DictionaryTypeGetOutput> GetAsync(long id);

    Task<PageOutput<DictionaryTypeListOutput>> GetPageAsync(Pager<DictionaryTypeGetPageDto> input);

    Task<long> AddAsync(DictionaryTypeAddInput input);

    Task UpdateAsync(DictionaryTypeUpdateInput input);

    Task DeleteAsync(long id);

    Task BatchDeleteAsync(long[] ids);

    Task SoftDeleteAsync(long id);

    Task BatchSoftDeleteAsync(long[] ids);
}