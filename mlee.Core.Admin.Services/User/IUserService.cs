using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using mlee.Core.Library.Dto;
using mlee.Core.Services.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using mlee.Core.Services.User.Dto;
using mlee.Core.Services.Auth.Dto;
using mlee.Core.Infrastructure.Entities.User.Dto;

namespace mlee.Core.Services.User
{
    /// <summary>
    /// 用户接口
    /// </summary>
    public interface IUserService
    {
        Task<UserGetOutput> GetAsync(long id);

        Task<PageOutput<UserGetPageOutput>> GetPageAsync(Pager<UserGetPageDto> input);

        Task<AuthLoginOutput> GetLoginUserAsync(long id);

        Task<DataPermissionDto> GetDataPermissionAsync();

        Task<long> AddAsync(UserAddInput input);

        Task<long> AddMemberAsync(UserAddMemberInput input);

        Task UpdateAsync(UserUpdateInput input);

        Task DeleteAsync(long id);

        Task BatchDeleteAsync(long[] ids);

        Task SoftDeleteAsync(long id);

        Task BatchSoftDeleteAsync(long[] ids);

        Task ChangePasswordAsync(UserChangePasswordInput input);

        Task<string> ResetPasswordAsync(UserResetPasswordInput input);

        Task SetManagerAsync(UserSetManagerInput input);

        Task UpdateBasicAsync(UserUpdateBasicInput input);

        Task<UserGetBasicOutput> GetBasicAsync();

        Task<IList<UserPermissionsOutput>> GetPermissionsAsync();

        Task<string> AvatarUpload([FromForm] IFormFile file, bool autoUpdate = false);
    }
}
