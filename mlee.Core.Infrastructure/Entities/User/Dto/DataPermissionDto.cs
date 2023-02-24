﻿using mlee.Core.Infrastructure.Entities.Role;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mlee.Core.Infrastructure.Entities.User.Dto
{

    public class DataPermissionDto
    {
        /// <summary>
        /// 部门Id
        /// </summary>
        public long OrgId { get; set; }

        /// <summary>
        /// 部门列表
        /// </summary>
        public List<long> OrgIds { get; set; }

        /// <summary>
        /// 数据范围
        /// </summary>
        public DataScope DataScope { get; set; } = DataScope.Self;
    }
}
