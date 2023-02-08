using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace mlee.Core.Library.Datas
{
    /// <summary>
    /// 查询操作符
    /// </summary>
    public enum Operator
    {
        /// <summary>
        /// 等于
        /// </summary>
        [Description("等于")]
        Equal = 1,
        /// <summary>
        /// 不等于
        /// </summary>
        [Description("不等于")]
        NotEqual = 2,
        /// <summary>
        /// 大于
        /// </summary>
        [Description("大于")]
        Greater = 3,
        /// <summary>
        /// 大于等于
        /// </summary>
        [Description("大于等于")]
        GreaterEqual = 4,
        /// <summary>
        /// 小于
        /// </summary>
        [Description("小于")]
        Less = 5,
        /// <summary>
        /// 小于等于
        /// </summary>
        [Description("小于等于")]
        LessEqual = 6,
        /// <summary>
        /// 头匹配
        /// </summary>
        [Description("头匹配")]
        Starts = 7,
        /// <summary>
        /// 尾匹配
        /// </summary>
        [Description("尾匹配")]
        Ends = 8,
        /// <summary>
        /// 模糊匹配
        /// </summary>
        [Description("模糊匹配")]
        Contains = 9,
        /// <summary>
        /// In
        /// </summary>
        [Description("In")]
        In = 10,
        /// <summary>
        /// Not In
        /// </summary>
        [Description("Not In")]
        NotIn = 11,
    }
}
