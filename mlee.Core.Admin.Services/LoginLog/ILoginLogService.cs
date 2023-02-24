using mlee.Core.Library.Dto;
using System.Threading.Tasks;
using mlee.Core.Services.LoginLog.Dto;
using ZhonTai.Admin.Domain;

namespace mlee.Core.Services.LoginLog;

/// <summary>
/// 登录日志接口
/// </summary>
public interface ILoginLogService
{
    Task<PageOutput<LoginLogListOutput>> GetPageAsync(Pager<LogGetPageDto> input);

    Task<long> AddAsync(LoginLogAddInput input);
}