using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mlee.Core.Infrastructure.Entities.Permission
{

    /// <summary>
    /// 权限类型
    /// </summary>
    public enum PermissionType
    {
        /// <summary>
        /// 分组
        /// </summary>
        Group = 1,

        /// <summary>
        /// 菜单
        /// </summary>
        Menu = 2,

        /// <summary>
        /// 权限点
        /// </summary>
        Dot = 3
    }
}
