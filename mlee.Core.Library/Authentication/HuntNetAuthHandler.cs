using mlee.Core.Library;
using mlee.Core.Library.Common;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace mlee.Core.Library.Authentication
{
    public class HuntNetAuthHandler : AuthenticationHandler<HuntNetAuthOption>
    {
        private const string TICKES = "Tickes";
        private const string KEY_AUTHORIZATION = "Token";
        private const string KEY_SPLIT = ":";

        protected new HuntNetEvents Events
        {
            get => (HuntNetEvents)base.Events;
            set => base.Events = value;
        }

        public HuntNetAuthHandler(IOptionsMonitor<HuntNetAuthOption> options, ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock)
            : base(options, logger, encoder, clock)
        {
        }

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            var headers = Request.Headers;
            string authorization = headers[KEY_AUTHORIZATION];
            // var tickes = headers[TICKES].ToLong();

            if (string.IsNullOrEmpty(authorization))
            {
                Logger.LogInformation("请求头authorization为空，目标路径{0}", Request.Path);
                return await Task.FromResult(AuthenticateResult.Fail("请求头authorization为空"));
                //return AuthenticateResult.NoResult();
            }
            string token = string.Empty;
            if (authorization.StartsWith(HuntNetAuthDefault.AuthenticationScheme + " ", StringComparison.CurrentCultureIgnoreCase))
            {
                token = authorization.Substring(HuntNetAuthDefault.AuthenticationScheme.Length).Trim();
            }
            if (string.IsNullOrEmpty(token))
            {
                Logger.LogInformation("无效的请求头authorization，目标路径{0}", Request.Path);
                return await Task.FromResult(AuthenticateResult.Fail("无效的请求头authorization"));
                //return AuthenticateResult.NoResult();
            }

            try
            {
                string key = Options.AppSecret;
                string name = Options.AppName;

                var data = token;
                // if (string.IsNullOrEmpty(data)) throw new Exception(" token 格式错误");

                string[] array = data.Split(KEY_SPLIT.ToCharArray());
                if (array.Length != 2) throw new Exception(" token 格式错误");

                var encryptToken = array[0];
                var tickes = array[1];
                if (string.IsNullOrEmpty(encryptToken) || string.IsNullOrEmpty(tickes)) throw new Exception(" token 格式错误");

                var secretEncrypt = Md5Encrypt.Encrypt($"{name}{key}{tickes}");
                if (!secretEncrypt.Equals(encryptToken))
                {
                    Logger.LogInformation("token 验证失败");
                    return await Task.FromResult(AuthenticateResult.Fail("token 验证失败"));
                }
                var tickesLong = tickes.ToLong();
                //过期时间30分钟
                if (tickesLong < DateTime.Now.AddMinutes(-30).Ticks)
                {
                    Logger.LogInformation("token 已过期");
                    return await Task.FromResult(AuthenticateResult.Fail("token 已过期"));
                }
                var claims = new List<Claim>()
                {
                    new Claim(ClaimTypes.Name, name)
                };

                var principer = new ClaimsPrincipal(new ClaimsIdentity(claims, HuntNetAuthDefault.AuthenticationScheme));
                var validatedContext = new HuntNetTokenValidatedContext(Context, Scheme, Options)
                {
                    Principal = principer
                };

                //  await Events.TokenValidated(validatedContext);

                validatedContext.Success();

                return await Task.FromResult(validatedContext.Result);
            }
            catch (Exception ex)
            {
                Logger.LogDebug(token + " validate failed: " + ex.Message);
                return await Task.FromResult(AuthenticateResult.Fail(ex.Message));
            }

        }

        protected override async Task HandleChallengeAsync(AuthenticationProperties properties)
        {
            var authResult = await HandleAuthenticateOnceSafeAsync();
            Response.StatusCode = 401;
            // Response.Headers.Add(HeaderNames.WWWAuthenticate, BasicDefault.AuthenticationScheme);
            if (authResult.Failure != null && !string.IsNullOrEmpty(authResult.Failure.Message))
            {
                var byteMsg = System.Text.Encoding.Default.GetBytes(authResult.Failure.Message);
                //  Response.Body.Write(byteMsg, 0, byteMsg.Length);
            }

            await base.HandleChallengeAsync(properties);
        }
    }
}
