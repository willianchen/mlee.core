using System.Threading.Tasks;
using mlee.Core.Library.Dto;
using mlee.Core.Infrastructure.Entities.Task.Dto;
using mlee.Core.Services.TaskScheduler.Dto;

namespace mlee.Core.Services.TaskScheduler;

/// <summary>
/// 任务接口
/// </summary>
public interface ITaskService
{
    Task<TaskGetOutput> GetAsync(long id);

    Task<PageOutput<TaskListOutput>> GetPageAsync(Pager<TaskGetPageDto> input);

    string Add(TaskAddInput input);

    Task UpdateAsync(TaskUpdateInput input);

    void Pause(string id);

    void Resume(string id);

    void Run(string id);

    void Delete(string id);
}