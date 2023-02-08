using System;
using System.Collections.Generic;
using System.Text;

namespace mlee.Core.Library.Enums
{
    public enum  SmsSendStatus: byte
    {
        Fail = 0,
        Success = 1,
        WaitSend = 100,
    }
}
