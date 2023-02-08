using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace mlee.Core.Library.Enums
{
    public enum Sex : byte
    {
        [Description("保密")]
        UnKnow = 0,
        [Description("男")]
        Male = 1,
        [Description("女")]
        Female = 2,
        [Description("其他")]
        Else = 3,
    }
}
