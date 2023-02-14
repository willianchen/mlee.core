﻿using FreeSql.DataAnnotations;
using mlee.Core.Infrastructure.Entities.Role;
using mlee.Core.Infrastructure.Entities.User;
using mlee.Core.Library.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mlee.Core.Infrastructure.Entities.UserRole
{

    /// <summary>
    /// 用户角色
    /// </summary>
    [Table(Name = "ad_user_role")]
    [Index("idx_{tablename}_01", nameof(UserId) + "," + nameof(RoleId), true)]
    public class UserRoleEntity : EntityAdd
    {
        /// <summary>
        /// 用户Id
        /// </summary>
        public long UserId { get; set; }

        public UserEntity User { get; set; }

        /// <summary>
        /// 角色Id
        /// </summary>
        public long RoleId { get; set; }

        public RoleEntity Role { get; set; }
    }
}
