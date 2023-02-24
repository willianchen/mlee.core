﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using mlee.Core.Library.Dto;
using mlee.Core.Services.Permission.Dto;

namespace mlee.Core.Services.Permission;

/// <summary>
/// 权限接口
/// </summary>
public partial interface IPermissionService
{
    Task<PermissionGetGroupOutput> GetGroupAsync(long id);

    Task<PermissionGetMenuOutput> GetMenuAsync(long id);

    Task<PermissionGetApiOutput> GetApiAsync(long id);

    Task<PermissionGetDotOutput> GetDotAsync(long id);

    Task<IEnumerable<dynamic>> GetPermissionList();

    Task<List<long>> GetRolePermissionList(long roleId);

    Task<List<long>> GetTenantPermissionList(long tenantId);

    Task<List<PermissionListOutput>> GetListAsync(string key, DateTime? start, DateTime? end);

    Task<long> AddGroupAsync(PermissionAddGroupInput input);

    Task<long> AddMenuAsync(PermissionAddMenuInput input);

    Task<long> AddApiAsync(PermissionAddApiInput input);

    Task<long> AddDotAsync(PermissionAddDotInput input);

    Task UpdateGroupAsync(PermissionUpdateGroupInput input);

    Task UpdateMenuAsync(PermissionUpdateMenuInput input);

    Task UpdateApiAsync(PermissionUpdateApiInput input);

    Task UpdateDotAsync(PermissionUpdateDotInput input);

    Task DeleteAsync(long id);

    Task SoftDeleteAsync(long id);

    Task AssignAsync(PermissionAssignInput input);

    Task SaveTenantPermissionsAsync(PermissionSaveTenantPermissionsInput input);
}