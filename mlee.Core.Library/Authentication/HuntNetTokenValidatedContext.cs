using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace mlee.Core.Library.Authentication
{
    public class HuntNetTokenValidatedContext : ResultContext<HuntNetAuthOption>
    {
        public HuntNetTokenValidatedContext(HttpContext context, AuthenticationScheme scheme, HuntNetAuthOption options)
            : base(context, scheme, options)
        {
        }

    }
}