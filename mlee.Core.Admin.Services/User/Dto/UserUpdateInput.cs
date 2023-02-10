using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mlee.Core.Services.User.Dto
{

    /// <summary>
    /// 修改
    /// </summary>
    public partial class UserUpdateInput : UserFormInput
    {
        /// <summary>
        /// 主键Id
        /// </summary>
        [Required]
     /*   [ValidateRequired("请选择用户")]*/
        public long Id { get; set; }
    }
}
