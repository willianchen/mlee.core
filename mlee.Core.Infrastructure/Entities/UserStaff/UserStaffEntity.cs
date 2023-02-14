using FreeSql.DataAnnotations;
using mlee.Core.Library.Configs;
using mlee.Core.Library.Entities;
using mlee.Core.Library.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mlee.Core.Infrastructure.Entities.UserStaff
{

    /// <summary>
    /// 用户员工
    /// </summary>
    [Table(Name = "ad_user_staff")]
    public partial class UserStaffEntity : EntityTenant
    {
        /// <summary>
        /// 职位
        /// </summary>
        public string Position { get; set; }

        /// <summary>
        /// 工号
        /// </summary>
        [Column(StringLength = 20)]
        public string JobNumber { get; set; }

        /// <summary>
        /// 性别
        /// </summary>
        [Column(MapType = typeof(int))]
        public Sex? Sex { get; set; }

        /// <summary>
        /// 入职时间
        /// </summary>
        public DateTime? EntryTime { get; set; }

        /// <summary>
        /// 个人简介
        /// </summary>
        [Column(StringLength = 500)]
        public string Introduce { get; set; }
    }
}
