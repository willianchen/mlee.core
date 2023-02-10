using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using mlee.Core.Services.User.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mlee.Core.Services.User
{
    /// <summary>
    /// 用户接口
    /// </summary>
    public interface IUserService
    {
        Task<UserGetOutput> GetAsync(long id);

        
    }
}
