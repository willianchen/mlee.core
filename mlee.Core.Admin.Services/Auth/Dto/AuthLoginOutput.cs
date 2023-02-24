﻿using mlee.Core.Infrastructure.Entities.Tenant;
using mlee.Core.Infrastructure.Entities.User;

namespace mlee.Core.Services.Auth.Dto;

public class AuthLoginOutput
{
    /// <summary>
    /// 主键Id
    /// </summary>
    public long Id { get; set; }

    /// <summary>
    /// 账号
    /// </summary>
    public string UserName { get; set; }

    /// <summary>
    /// 姓名
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// 用户类型
    /// </summary>
    public UserType Type { get; set; }

    /// <summary>
    /// 租户Id
    /// </summary>
    public long? TenantId { get; set; }

    /// <summary>
    /// 部门Id
    /// </summary>
    public long? OrgId { get; set; }

    /// <summary>
    /// 租户类型
    /// </summary>
    public TenantType? TenantType { get; set; }

    /// <summary>
    /// 数据库注册键
    /// </summary>
    public string DbKey { get; set; }
}