using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace mlee.Core.Library.Enums
{
    public enum Status : byte
    {
        [Description("待审核")]
        CheckPending = 0,
        [Description("审核通过")]
        CheckPass = 1,
        [Description("审核不过")]
        CheckNoPass = 2,
    }
}
