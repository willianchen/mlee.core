using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mlee.Core.Auth
{
    public class NullPermissionHandler : IPermissionHandler
    {
        private readonly mlee.Core.Library.Sessions.ISession _user;
        public NullPermissionHandler(mlee.Core.Library.Sessions.ISession user)
        {
            _user = user;
        }

        /// <summary>
        /// 权限验证
        /// </summary>
        /// <param name="api">接口路径</param>
        /// <param name="httpMethod">http请求方法</param>
        /// <returns></returns>
        public async Task<bool> ValidateAsync(string api, string httpMethod)
        {
            if (_user.PlatformAdmin)
            {
                return true;
            }

            /* var permissions = new List<string>();;
             //await _userService.GetPermissionsAsync();

             var valid = permissions.Any(m =>
                 m.Path.NotNull() && m.Path.EqualsIgnoreCase($"/{api}")
                 && m.HttpMethods.NotNull() && m.HttpMethods.Split(',').Any(n => n.NotNull() && n.EqualsIgnoreCase(httpMethod))
             );*/

            return true;
        }
    }
}