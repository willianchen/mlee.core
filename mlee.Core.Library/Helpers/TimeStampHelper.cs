using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace mlee.Core.Library.Helpers
{
    public class TimeStampHelper
    {
        public readonly static DateTime BaseTime = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        public readonly static long BaseTimeTicks = BaseTime.Ticks;
        
        public static DateTime TimeStampToDateTime(Int64 timeStamp, TimeStampType timeStampType = TimeStampType.Unix)
        {
            long  timeTicks=0;
            switch (timeStampType)
            {
                case TimeStampType.CSharp:
                    timeTicks = timeStamp;
                    break;
                case TimeStampType.JavaScript:
                    timeTicks =timeStamp * 10000 + BaseTimeTicks;
                    break;
                case TimeStampType.Unix:
                    timeTicks = timeStamp * 10000000 + BaseTimeTicks;
                    break;
                default:
                    break;
            }
            return new DateTime(timeTicks);
        }

        //return (long)(dateTime.ToUniversalTime() - BaseTime).TotalSeconds;
        public static long DateTimeToTimeStamp(System.DateTime time, TimeStampType timeSpanType = TimeStampType.Unix)
        {
            long timeTicks = 0;
            long inTimeTicks=time.ToUniversalTime().Ticks;
            switch (timeSpanType)
            {
                case TimeStampType.CSharp:
                    timeTicks = inTimeTicks;
                    break;
                case TimeStampType.JavaScript:
                    timeTicks = (inTimeTicks - BaseTimeTicks) / 10000;
                    break;
                case TimeStampType.Unix:
                    timeTicks = (inTimeTicks - BaseTimeTicks) / 10000000;
                    break;
                default:
                    break;
            }

            return timeTicks;
        }

        public enum TimeStampType : byte
        {
            [Description("C#")]
            CSharp = 1,
            [Description("JavaScript")]
            JavaScript = 2,
            [Description("Unix")]
            Unix = 3,
        }
    }
}
