using FreeScheduler;
using System.Threading.Tasks;
using mlee.Core.Library.Dto;
using mlee.Core.Infrastructure.Entities.Task.Dto;

namespace mlee.Core.Services.TaskScheduler;

/// <summary>
/// 任务日志接口
/// </summary>
public interface ITaskLogService
{
    Task<PageOutput<TaskLog>> GetPageAsync(Pager<TaskLogGetPageDto> input);
}