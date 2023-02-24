﻿using Newtonsoft.Json;
using System.Collections.Generic;
using mlee.Core.Infrastructure.Entities.Org;
using mlee.Core.Infrastructure.Entities.Role;

namespace mlee.Core.Services.Role.Dto;

/// <summary>
/// 添加
/// </summary>
public class RoleAddInput
{
    /// <summary>
    /// 父级Id
    /// </summary>
    public long ParentId { get; set; }

    /// <summary>
    /// 名称
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// 编码
    /// </summary>
    public string Code { get; set; }

    /// <summary>
    /// 角色类型
    /// </summary>
    public RoleType Type { get; set; }

    /// <summary>
    /// 数据范围
    /// </summary>
    public DataScope DataScope { get; set; }

    /// <summary>
    /// 指定部门
    /// </summary>
    public long[] OrgIds { get; set; }

    /// <summary>
    /// 部门列表
    /// </summary>
    [JsonIgnore]
    public ICollection<OrgEntity> Orgs { get; set; }

    /// <summary>
    /// 说明
    /// </summary>
    public string Description { get; set; }

    /// <summary>
    /// 排序
    /// </summary>
    public int Sort { get; set; }
}