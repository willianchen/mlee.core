using Microsoft.AspNetCore.Authentication;
using System;
using System.Collections.Generic;
using System.Text;

namespace mlee.Core.Library.Authentication
{
    public static partial class Extensions
    {
        public static AuthenticationBuilder AddHuntNetAuth(this AuthenticationBuilder builder)
           => builder.AddHuntNetAuth(HuntNetAuthDefault.AuthenticationScheme, _ => { });

        public static AuthenticationBuilder AddHuntNetAuth(this AuthenticationBuilder builder, Action<HuntNetAuthOption> configureOptions)
            => builder.AddHuntNetAuth(HuntNetAuthDefault.AuthenticationScheme, configureOptions);

        public static AuthenticationBuilder AddHuntNetAuth(this AuthenticationBuilder builder, string authenticationScheme, Action<HuntNetAuthOption> configureOptions)
            => builder.AddHuntNetAuth(authenticationScheme, displayName: HuntNetAuthDefault.DisplayName, configureOptions: configureOptions);

        public static AuthenticationBuilder AddHuntNetAuth(this AuthenticationBuilder builder, string authenticationScheme, string displayName, Action<HuntNetAuthOption> configureOptions)
        {
            return builder.AddScheme<HuntNetAuthOption, HuntNetAuthHandler>(authenticationScheme, displayName, configureOptions);
        }
    }
}
