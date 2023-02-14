﻿using System;
using System.Collections.Generic;
using FreeSql.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using FreeSql;
using mlee.Core.Library.Entities;
using mlee.Core.Infrastructure.Entities.Tenant;

namespace mlee.Core.Infrastructure.Entities.User
{

    /// <summary>
    /// 用户
    /// </summary>
    [Table(Name = "ad_user")]
    [Index("idx_{tablename}_01", nameof(UserName) + "," + nameof(TenantId), true)]
    public partial class UserEntity : EntityTenant
    {
        public TenantEntity Tenant { get; set; }

        /// <summary>
        /// 账号
        /// </summary>
        [Column(StringLength = 60)]
        public string UserName { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        [Column(StringLength = 60)]
        public string Password { get; set; }

        /// <summary>
        /// 姓名
        /// </summary>
        [Column(StringLength = 20)]
        public string Name { get; set; }

        /// <summary>
        /// 手机号
        /// </summary>
        [Column(StringLength = 20)]
        public string Mobile { get; set; }

        /// <summary>
        /// 邮箱
        /// </summary>
        [Column(StringLength = 100)]
        public string Email { get; set; }

        /// <summary>
        /// 主属部门Id
        /// </summary>
        public long OrgId { get; set; }
        /*
                /// <summary>
                /// 部门
                /// </summary>
                public OrgEntity Org { get; set; }
        */
        /// <summary>
        /// 直属主管Id
        /// </summary>
        public long? ManagerUserId { get; set; }

        /// <summary>
        /// 直属主管
        /// </summary>
        public UserEntity ManagerUser { get; set; }

        /// <summary>
        /// 昵称
        /// </summary>
        [Column(StringLength = 20)]
        public string NickName { get; set; }

        /// <summary>
        /// 头像
        /// </summary>
        [Column(StringLength = 100)]
        public string Avatar { get; set; }

        /*    /// <summary>
            /// 用户状态
            /// </summary>
            [Column(MapType = typeof(int))]
            public UserStatus Status { get; set; }

            /// <summary>
            /// 用户类型
            /// </summary>
            [Column(MapType = typeof(int))]
            public UserType Type { get; set; } = UserType.DefaultUser;

            /// <summary>
            /// 角色列表
            /// </summary>
            [Navigate(ManyToMany = typeof(UserRoleEntity))]
            public ICollection<RoleEntity> Roles { get; set; }

            /// <summary>
            /// 部门列表
            /// </summary>
            [Navigate(ManyToMany = typeof(UserOrgEntity))]
            public ICollection<OrgEntity> Orgs { get; set; }

            /// <summary>
            /// 员工
            /// </summary>
            [Navigate(nameof(Id))]
            public UserStaffEntity Staff { get; set; }*/
    }
}
