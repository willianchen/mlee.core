﻿using FreeSql.DataAnnotations;
using mlee.Core.Infrastructure.Entities.Org;
using mlee.Core.Infrastructure.Entities.Permission;
using mlee.Core.Infrastructure.Entities.RoleOrg;
using mlee.Core.Infrastructure.Entities.RolePermission;
using mlee.Core.Infrastructure.Entities.User;
using mlee.Core.Infrastructure.Entities.UserRole;
using mlee.Core.Library.Configs;
using mlee.Core.Library.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Metadata.Edm;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mlee.Core.Infrastructure.Entities.Role
{

    /// <summary>
    /// 角色
    /// </summary>
    [Table(Name = "ad_role")]
    [Index("idx_{tablename}_01", $"{nameof(TenantId)},{nameof(ParentId)},{nameof(Name)}", true)]
    public partial class RoleEntity : EntityTenant
    {
        /// <summary>
        /// 父级Id
        /// </summary>
        public long ParentId { get; set; }

        /// <summary>
        /// 子级列表
        /// </summary>
        [Navigate(nameof(ParentId))]
        public List<RoleEntity> Childs { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        [Column(StringLength = 50)]
        public string Name { get; set; }

        /// <summary>
        /// 编码
        /// </summary>
        [Column(StringLength = 50)]
        public string Code { get; set; }

        /// <summary>
        /// 角色类型
        /// </summary>
        [Column(MapType = typeof(int), CanUpdate = false)]
        public RoleType Type { get; set; }

        /// <summary>
        /// 数据范围
        /// </summary>
        [Column(MapType = typeof(int))]
        public DataScope DataScope { get; set; } = DataScope.All;

        /// <summary>
        /// 说明
        /// </summary>
        [Column(StringLength = 200)]
        public string Description { get; set; }

        /// <summary>
        /// 隐藏
        /// </summary>
        public bool Hidden { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        public int Sort { get; set; }

        /// <summary>
        /// 用户列表
        /// </summary>
        [Navigate(ManyToMany = typeof(UserRoleEntity))]
        public ICollection<UserEntity> Users { get; set; }

        /// <summary>
        /// 部门列表
        /// </summary>
        [Navigate(ManyToMany = typeof(RoleOrgEntity))]
        public ICollection<OrgEntity> Orgs { get; set; }

        /// <summary>
        /// 权限列表
        /// </summary>
        [Navigate(ManyToMany = typeof(RolePermissionEntity))]
        public ICollection<PermissionEntity> Permissions { get; set; }
    }
}
