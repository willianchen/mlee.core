using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System;
using System.Linq;
using Microsoft.AspNetCore.Http;
using mlee.Core.Library.Helpers;
using mlee.Core.Auth;

namespace mlee.Core.Attributes
{

    /// <summary>
    /// 启用权限验证
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true)]
    public class ValidatePermissionAttribute : AuthorizeAttribute, IAuthorizationFilter, IAsyncAuthorizationFilter
    {
        private async Task PermissionAuthorization(AuthorizationFilterContext context)
        {
            //排除匿名访问
            if (context.ActionDescriptor.EndpointMetadata.Any(m => m.GetType() == typeof(AllowAnonymousAttribute)))
                return;

            //登录验证
            var user = Ioc.Create<mlee.Core.Library.Sessions.ISession>();
            //context.HttpContext.RequestServices.GetService<mlee.Core.Library.Sessions.ISession>();
            if (user == null || !(user?.UserId > 0))
            {
                context.Result = new ChallengeResult();
                return;
            }

            //排除登录接口
            /*   if (context.ActionDescriptor.EndpointMetadata.Any(m => m.GetType() == typeof(LoginAttribute)))
                   return;*/

            //权限验证
            var httpMethod = context.HttpContext.Request.Method;
            var api = context.ActionDescriptor.AttributeRouteInfo.Template;
            var permissionHandler = Ioc.Create<IPermissionHandler>();
            //context.HttpContext.RequestServices.GetService<IPermissionHandler>();
            var isValid = await permissionHandler.ValidateAsync(api, httpMethod);
            if (!isValid)
            {
                context.Result = new ForbidResult();
            }
        }

        public async void OnAuthorization(AuthorizationFilterContext context)
        {
            await PermissionAuthorization(context);
        }

        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            await PermissionAuthorization(context);
        }
    }
}
