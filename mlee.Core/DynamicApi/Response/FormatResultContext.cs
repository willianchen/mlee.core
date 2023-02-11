using mlee.Core.Library.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mlee.Core.DynamicApi.Response
{

    public static class FormatResultContext
    {
        internal static Type FormatResultType = typeof(ApiResult<>);
    }
}
