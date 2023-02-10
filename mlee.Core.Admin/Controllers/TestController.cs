﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using mlee.Core.Library.Dto;
using mlee.Core.Services.User;
using mlee.Core.Services.User.Dto;

namespace mlee.Core.Admin.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    [AllowAnonymous]
    public class TestController : ControllerBase
    {
        private readonly IUserService userService;
        public TestController(IUserService _userService)
        {
            userService = _userService;
        }

        [HttpGet]
        public async Task<ApiResult<UserGetOutput>> GetAsync(long id)
        {
            var result = await userService.GetAsync(id);
            return ApiResult<UserGetOutput>.ToSuccess(result);
        }
    }
}
