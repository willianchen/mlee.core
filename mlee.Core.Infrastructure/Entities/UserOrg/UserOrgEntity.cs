using FreeSql.DataAnnotations;
using mlee.Core.Infrastructure.Entities.Org;
using mlee.Core.Infrastructure.Entities.User;
using mlee.Core.Library.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mlee.Core.Infrastructure.Entities.UserOrg
{

    /// <summary>
    /// 用户所属部门
    /// </summary>
    [Table(Name = "ad_user_org")]
    [Index("idx_{tablename}_01", nameof(UserId) + "," + nameof(OrgId), true)]
    public partial class UserOrgEntity : EntityUpdate
    {
        /// <summary>
        /// 用户Id
        /// </summary>
        public long UserId { get; set; }

        /// <summary>
        /// 用户
        /// </summary>
        public UserEntity User { get; set; }

        /// <summary>
        /// 部门Id
        /// </summary>
        public long OrgId { get; set; }

        /// <summary>
        /// 部门
        /// </summary>
        public OrgEntity Org { get; set; }

        /// <summary>
        /// 是否主管
        /// </summary>
        public bool IsManager { get; set; } = false;
    }
}
