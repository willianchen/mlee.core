using FreeSql.DataAnnotations;
using mlee.Core.Infrastructure.Entities.Api;
using mlee.Core.Infrastructure.Entities.Permission;
using mlee.Core.Library.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mlee.Core.Infrastructure.Entities.PermissionApi
{

    /// <summary>
    /// 权限接口
    /// </summary>
    [Table(Name = "ad_permission_api")]
    [Index("idx_{tablename}_01", nameof(PermissionId) + "," + nameof(ApiId), true)]
    public class PermissionApiEntity : EntityAdd
    {
        /// <summary>
        /// 权限Id
        /// </summary>
        public long PermissionId { get; set; }

        /// <summary>
        /// 权限
        /// </summary>
        public PermissionEntity Permission { get; set; }

        /// <summary>
        /// 接口Id
        /// </summary>
        public long ApiId { get; set; }

        /// <summary>
        /// 接口
        /// </summary>
        public ApiEntity Api { get; set; }
    }
}
