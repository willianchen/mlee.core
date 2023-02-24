﻿using System.Threading.Tasks;
using mlee.Core.Library.Dto;
using mlee.Core.Infrastructure.Entities.Task.Dto;
using ZhonTai.DynamicApi;
using ZhonTai.DynamicApi.Attributes;
using Microsoft.AspNetCore.Mvc;
using ZhonTai.Admin.Core.Consts;
using FreeScheduler;
using ZhonTai.Admin.Repositories;

namespace mlee.Core.Services.TaskScheduler;

/// <summary>
/// 任务日志服务
/// </summary>
[Order(71)]
[DynamicApi(Area = AdminConsts.AreaName)]
public class TaskLogService : BaseService, ITaskLogService, IDynamicApi
{
    private ITaskLogRepository _taskLogRepository => LazyGetRequiredService<ITaskLogRepository>();

    public TaskLogService()
    {

    }

    /// <summary>
    /// 查询分页
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<PageOutput<TaskLog>> GetPageAsync(Pager<TaskLogGetPageDto> input)
    {
        if (!(input.Filter != null && input.Filter.TaskId.NotNull()))
        {
            throw ApiResult.ToException("请选择任务");
        }

        var list = await _taskLogRepository.Select
        .WhereDynamicFilter(input.DynamicFilter)
        .Where(a => a.TaskId == input.Filter.TaskId)
        .Count(out var total)
        .OrderBy(c => c.Round)
        .Page(input.CurrentPage, input.PageSize)
        .ToListAsync();

        var data = new PageOutput<TaskLog>()
        {
            List = list,
            Total = total
        };

        return data;
    }
}