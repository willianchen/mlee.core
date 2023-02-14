using FreeSql.DataAnnotations;
using mlee.Core.Infrastructure.Entities.Org;
using mlee.Core.Infrastructure.Entities.Role;
using mlee.Core.Library.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mlee.Core.Infrastructure.Entities.RoleOrg
{

    /// <summary>
    /// 角色部门
    /// </summary>
    [Table(Name = "ad_role_org")]
    [Index("idx_{tablename}_01", nameof(RoleId) + "," + nameof(OrgId), true)]
    public partial class RoleOrgEntity : EntityAdd
    {
        /// <summary>
        /// 角色Id
        /// </summary>
        public long RoleId { get; set; }

        /// <summary>
        /// 角色
        /// </summary>
        public RoleEntity Role { get; set; }

        /// <summary>
        /// 部门Id
        /// </summary>
        public long OrgId { get; set; }

        /// <summary>
        /// 部门
        /// </summary>
        public OrgEntity Org { get; set; }
    }
}
