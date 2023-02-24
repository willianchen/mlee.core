using System.Threading.Tasks;
using mlee.Core.Library.Dto;
using mlee.Core.Services.OprationLog.Dto;
using ZhonTai.Admin.Domain;

namespace mlee.Core.Services.OprationLog;

/// <summary>
/// 操作日志接口
/// </summary>
public interface IOprationLogService
{
    Task<PageOutput<OprationLogListOutput>> GetPageAsync(Pager<LogGetPageDto> input);

    Task<long> AddAsync(OprationLogAddInput input);
}