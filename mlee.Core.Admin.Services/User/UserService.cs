using AutoMapper;
using Microsoft.AspNetCore.Http;
using mlee.Core.Admin.Services;
using mlee.Core.Consts;
using mlee.Core.Infrastructure.Attributes;
using mlee.Core.Infrastructure.Consts;
using mlee.Core.Infrastructure.Entities.Org;
using mlee.Core.Infrastructure.Entities.Role;
using mlee.Core.Infrastructure.Entities.Tenant;
using mlee.Core.Infrastructure.Entities.User;
using mlee.Core.Infrastructure.Entities.User.Dto;
using mlee.Core.Infrastructure.Entities.UserOrg;
using mlee.Core.Infrastructure.Entities.UserRole;
using mlee.Core.Infrastructure.Entities.UserStaff;
using mlee.Core.Library;
using mlee.Core.Library.Common;
using mlee.Core.Library.Configs;
using mlee.Core.Library.Dto;
using mlee.Core.Library.Helpers;
using mlee.Core.Library.Maps;
using mlee.Core.Repository.User;
using mlee.Core.Services.Auth.Dto;
using mlee.Core.Services.User;
using mlee.Core.Services.User.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mlee.Core.Services.User
{
    public partial class UserService : BaseService, IUserService//, IDynamicApi
    {
        private AppConfig _appConfig => LazyGetRequiredService<AppConfig>();
        private IUserRepository _userRepository => LazyGetRequiredService<IUserRepository>();
        private IUserOrgRepository _userOrgRepository => LazyGetRequiredService<IUserOrgRepository>();
        private ITenantRepository _tenantRepository => LazyGetRequiredService<ITenantRepository>();
        private IUserRoleRepository _userRoleRepository => LazyGetRequiredService<IUserRoleRepository>();
        private IUserStaffRepository _staffRepository => LazyGetRequiredService<IUserStaffRepository>();
        public UserService()
        {
        }


        /// <summary>
        /// 查询用户
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<UserGetOutput> GetAsync(long id)
        {
            var userEntity = await _userRepository.Select
            .WhereDynamic(id)
            .IncludeMany(a => a.Roles.Select(b => new RoleEntity { Id = b.Id, Name = b.Name }))
            .IncludeMany(a => a.Orgs.Select(b => new OrgEntity { Id = b.Id, Name = b.Name }))
            .ToOneAsync(a => new
            {
                a.Id,
                a.UserName,
                a.Name,
                a.Mobile,
                a.Email,
                a.Roles,
                a.Orgs,
                a.OrgId,
                a.ManagerUserId,
                ManagerUserName = a.ManagerUser.Name,
                Staff = new
                {
                    a.Staff.JobNumber,
                    a.Staff.Sex,
                    a.Staff.Position,
                    a.Staff.Introduce
                }
            });

            var output = Mapper.Map<UserGetOutput>(userEntity);

            return output;
        }

        /// <summary>
        /// 查询分页
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<PageOutput<UserGetPageOutput>> GetPageAsync(Pager<UserGetPageDto> input)
        {
            var orgId = input.Filter?.OrgId;
            var list = await _userRepository.Select
            .WhereIf(orgId.HasValue && orgId > 0, a => _userOrgRepository.Where(b => b.UserId == a.Id && b.OrgId == orgId).Any())
            .Where(a => a.Type != UserType.Member)
            .WhereDynamicFilter(input.DynamicFilter)
            .Count(out var total)
            .OrderByDescending(true, a => a.Id)
            .IncludeMany(a => a.Roles.Select(b => new RoleEntity { Name = b.Name }))
            .Page(input.Page, input.PageSize)
            .ToListAsync(a => new UserGetPageOutput { Roles = a.Roles });

            if (orgId.HasValue && orgId > 0)
            {
                var managerUserIds = await _userOrgRepository.Select.Where(a => a.OrgId == orgId && a.IsManager == true).ToListAsync(a => a.UserId);
                if (managerUserIds.Any())
                {
                    var managerUsers = list.Where(a => managerUserIds.Contains(a.Id));
                    foreach (var managerUser in managerUsers)
                    {
                        managerUser.IsManager = true;
                    }
                }
            }

            var data = new PageOutput<UserGetPageOutput>()
            {
                List = Mapper.Map<List<UserGetPageOutput>>(list),
                Total = total
            };

            return data;
        }

        /// <summary>
        /// 查询登录用户信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<AuthLoginOutput> GetLoginUserAsync(long id)
        {
            var output = new ApiResult<AuthLoginOutput>();
            var entityDto = await _userRepository.Select.DisableGlobalFilter(FilterNames.Tenant)
                .WhereDynamic(id).ToOneAsync<AuthLoginOutput>();

            if (_appConfig.Tenant && entityDto?.TenantId.Value > 0)
            {
                var tenant = await _tenantRepository.Select.DisableGlobalFilter(FilterNames.Tenant)
                    .WhereDynamic(entityDto.TenantId).ToOneAsync(a => new { a.TenantType, a.DbKey });
                entityDto.TenantType = tenant.TenantType;
                entityDto.DbKey = tenant.DbKey;
            }
            return entityDto;
        }

        /// <summary>
        /// 获得数据权限
        /// </summary>
        /// <returns></returns>
        public async Task<DataPermissionDto> GetDataPermissionAsync()
        {
            if (!(User?.Id > 0))
            {
                return null;
            }

            var key = CacheKeys.DataPermission + User.Id;
            return await Cache.GetOrSetAsync(key, async () =>
            {
                using (_userRepository.DataFilter.Disable(FilterNames.Self, FilterNames.Data))
                {
                    var user = await _userRepository.Select
                    .IncludeMany(a => a.Roles.Select(b => new RoleEntity
                    {
                        Id = b.Id,
                        DataScope = b.DataScope
                    }))
                    .WhereDynamic(User.Id)
                    .ToOneAsync(a => new
                    {
                        a.OrgId,
                        a.Roles
                    });

                    if (user == null)
                        return null;

                    //数据范围
                    DataScope dataScope = DataScope.Self;
                    var customRoleIds = new List<long>();
                    user.Roles?.ToList().ForEach(role =>
                    {
                        if (role.DataScope == DataScope.Custom)
                        {
                            customRoleIds.Add(role.Id);
                        }
                        else if (role.DataScope <= dataScope)
                        {
                            dataScope = role.DataScope;
                        }
                    });

                    //部门列表
                    var orgIds = new List<long>();
                    if (dataScope != DataScope.All)
                    {
                        //本部门
                        if (dataScope == DataScope.Dept)
                        {
                            orgIds.Add(user.OrgId);
                        }
                        //本部门和下级部门
                        else if (dataScope == DataScope.DeptWithChild)
                        {
                            orgIds = await _orgRepository
                            .Where(a => a.Id == user.OrgId)
                            .AsTreeCte()
                            .ToListAsync(a => a.Id);
                        }

                        //指定部门
                        if (customRoleIds.Count > 0)
                        {
                            var customRoleOrgIds = await _roleOrgRepository.Select.Where(a => customRoleIds.Contains(a.RoleId)).ToListAsync(a => a.OrgId);
                            orgIds = orgIds.Concat(customRoleOrgIds).ToList();
                        }
                    }

                    return new DataPermissionDto
                    {
                        OrgId = user.OrgId,
                        OrgIds = orgIds.Distinct().ToList(),
                        DataScope = dataScope
                    };
                }
            });
        }

        /// <summary>
        /// 查询用户基本信息
        /// </summary>
        /// <returns></returns>
        public async Task<UserGetBasicOutput> GetBasicAsync()
        {
            if (!(User?.Id > 0))
            {
                throw ApiResult.ToException("未登录！");
            }

            var data = await _userRepository.GetAsync<UserGetBasicOutput>(User.Id);
            data.Mobile = DataMaskHelper.PhoneMask(data.Mobile);
            data.Email = DataMaskHelper.EmailMask(data.Email);
            return data;
        }

        /// <summary>
        /// 查询用户权限信息
        /// </summary>
        /// <returns></returns>
        public async Task<IList<UserPermissionsOutput>> GetPermissionsAsync()
        {
            var key = CacheKeys.UserPermissions + User.Id;
            var result = await Cache.GetOrSetAsync(key, async () =>
            {
                if (User.TenantAdmin)
                {
                    var cloud = LazyGetRequiredService<FreeSqlCloud>();
                    var db = cloud.Use(DbKeys.AppDb);

                    return await db.Select<ApiEntity>()
                    .Where(a => db.Select<TenantPermissionEntity, PermissionApiEntity>()
                    .InnerJoin((b, c) => b.PermissionId == c.PermissionId && b.TenantId == User.TenantId)
                    .Where((b, c) => c.ApiId == a.Id).Any())
                    .ToListAsync<UserPermissionsOutput>();
                }

                return await _apiRepository
                .Where(a => _apiRepository.Orm.Select<UserRoleEntity, RolePermissionEntity, PermissionApiEntity>()
                .InnerJoin((b, c, d) => b.RoleId == c.RoleId && b.UserId == User.Id)
                .InnerJoin((b, c, d) => c.PermissionId == d.PermissionId)
                .Where((b, c, d) => d.ApiId == a.Id).Any())
                .ToListAsync<UserPermissionsOutput>();
            });
            return result;
        }

        /// <summary>
        /// 新增用户
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
/*        [AdminTransaction]*/
        public virtual async Task<long> AddAsync(UserAddInput input)
        {
            if (await _userRepository.Select.AnyAsync(a => a.UserName == input.UserName))
            {
                throw ApiResult.ToException($"账号已存在");
            }

            if (input.Mobile.NotNull() && await _userRepository.Select.AnyAsync(a => a.Mobile == input.Mobile))
            {
                throw ApiResult.ToException($"手机号已存在");
            }

            if (input.Email.NotNull() && await _userRepository.Select.AnyAsync(a => a.Email == input.Email))
            {
                throw ApiResult.ToException($"邮箱已存在");
            }

            // 用户信息
            if (input.Password.IsNull())
            {
                input.Password = _appConfig.DefaultPassword;
            }

            input.Password = Md5Encrypt.Encrypt(input.Password);

            var entity = Mapper.Map<UserEntity>(input);
            entity.Type = UserType.DefaultUser;
            var user = await _userRepository.InsertAsync(entity);
            var userId = user.Id;

            //用户角色
            if (input.RoleIds != null && input.RoleIds.Any())
            {
                var roles = input.RoleIds.Select(roleId => new UserRoleEntity
                {
                    UserId = userId,
                    RoleId = roleId
                }).ToList();
                await _userRoleRepository.InsertAsync(roles);
            }

            // 员工信息
            var staff = input.Staff == null ? new UserStaffEntity() : Mapper.Map<UserStaffEntity>(input.Staff);
            staff.Id = userId;
            await _staffRepository.InsertAsync(staff);

            //所属部门
            if (input.OrgIds != null && input.OrgIds.Any())
            {
                var orgs = input.OrgIds.Select(orgId => new UserOrgEntity
                {
                    UserId = userId,
                    OrgId = orgId
                }).ToList();
                await _userOrgRepository.InsertAsync(orgs);
            }

            return userId;
        }

        /// <summary>
        /// 新增会员
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public virtual async Task<long> AddMemberAsync(UserAddMemberInput input)
        {
            using (_userRepository.DataFilter.DisableAll())
            {
                if (await _userRepository.Select.AnyAsync(a => a.UserName == input.UserName))
                {
                    throw ApiResult.ToException($"账号已存在");
                }

                if (input.Mobile.NotNull() && await _userRepository.Select.AnyAsync(a => a.Mobile == input.Mobile))
                {
                    throw ApiResult.ToException($"手机号已存在");
                }

                if (input.Email.NotNull() && await _userRepository.Select.AnyAsync(a => a.Email == input.Email))
                {
                    throw ApiResult.ToException($"邮箱已存在");
                }

                // 用户信息
                if (input.Password.IsNull())
                {
                    input.Password = _appConfig.DefaultPassword;
                }

                input.Password = Md5Encrypt.Encrypt(input.Password);

                var entity = Mapper.Map<UserEntity>(input);
                entity.Type = UserType.Member;
                var user = await _userRepository.InsertAsync(entity);

                return user.Id;
            }
        }

        /// <summary>
        /// 修改会员
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [AdminTransaction]
        public virtual async Task UpdateMemberAsync(UserUpdateMemberInput input)
        {
            var user = await _userRepository.GetAsync(input.Id);
            if (!(user?.Id > 0))
            {
                throw ApiResult.ToException("用户不存在");
            }

            if (await _userRepository.Select.AnyAsync(a => a.Id != input.Id && a.UserName == input.UserName))
            {
                throw ApiResult.ToException($"账号已存在");
            }

            if (input.Mobile.NotNull() && await _userRepository.Select.AnyAsync(a => a.Id != input.Id && a.Mobile == input.Mobile))
            {
                throw ApiResult.ToException($"手机号已存在");
            }

            if (input.Email.NotNull() && await _userRepository.Select.AnyAsync(a => a.Id != input.Id && a.Email == input.Email))
            {
                throw ApiResult.ToException($"邮箱已存在");
            }

            Mapper.Map(input, user);
            await _userRepository.UpdateAsync(user);
        }

        /// <summary>
        /// 修改用户
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [AdminTransaction]
        public virtual async Task UpdateAsync(UserUpdateInput input)
        {
            var user = await _userRepository.GetAsync(input.Id);
            if (!(user?.Id > 0))
            {
                throw ApiResult.ToException("用户不存在");
            }

            if (input.Id == input.ManagerUserId)
            {
                throw ApiResult.ToException("直属主管不能是自己");
            }

            if (await _userRepository.Select.AnyAsync(a => a.Id != input.Id && a.UserName == input.UserName))
            {
                throw ApiResult.ToException($"账号已存在");
            }

            if (input.Mobile.NotNull() && await _userRepository.Select.AnyAsync(a => a.Id != input.Id && a.Mobile == input.Mobile))
            {
                throw ApiResult.ToException($"手机号已存在");
            }

            if (input.Email.NotNull() && await _userRepository.Select.AnyAsync(a => a.Id != input.Id && a.Email == input.Email))
            {
                throw ApiResult.ToException($"邮箱已存在");
            }

            Mapper.Map(input, user);
            await _userRepository.UpdateAsync(user);

            var userId = user.Id;

            // 用户角色
            await _userRoleRepository.DeleteAsync(a => a.UserId == userId);
            if (input.RoleIds != null && input.RoleIds.Any())
            {
                var roles = input.RoleIds.Select(roleId => new UserRoleEntity
                {
                    UserId = userId,
                    RoleId = roleId
                }).ToList();
                await _userRoleRepository.InsertAsync(roles);
            }

            // 员工信息
            var staff = await _staffRepository.GetAsync(userId);
            staff ??= new UserStaffEntity();
            Mapper.Map(input.Staff, staff);
            staff.Id = userId;
            await _staffRepository.InsertOrUpdateAsync(staff);

            //所属部门
            await _userOrgRepository.DeleteAsync(a => a.UserId == userId);
            if (input.OrgIds != null && input.OrgIds.Any())
            {
                var orgs = input.OrgIds.Select(orgId => new UserOrgEntity
                {
                    UserId = userId,
                    OrgId = orgId
                }).ToList();
                await _userOrgRepository.InsertAsync(orgs);
            }

            await Cache.DelAsync(CacheKeys.DataPermission + user.Id);
        }

        /// <summary>
        /// 更新用户基本信息
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
  /*      [Login]*/
        public async Task UpdateBasicAsync(UserUpdateBasicInput input)
        {
            var entity = await _userRepository.GetAsync(User.Id);
            entity = Mapper.Map(input, entity);
            await _userRepository.UpdateAsync(entity);
        }

        /// <summary>
        /// 修改用户密码
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
      /*  [Login]*/
        public async Task ChangePasswordAsync(UserChangePasswordInput input)
        {
            if (input.ConfirmPassword != input.NewPassword)
            {
                throw ApiResult.ToException("新密码和确认密码不一致");
            }

            var entity = await _userRepository.GetAsync(User.Id);
            var oldPassword = Md5Encrypt.Encrypt(input.OldPassword);
            if (oldPassword != entity.Password)
            {
                throw ApiResult.ToException("旧密码不正确");
            }

            entity.Password = Md5Encrypt.Encrypt(input.NewPassword);
            await _userRepository.UpdateAsync(entity);
        }

        /// <summary>
        /// 重置密码
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<string> ResetPasswordAsync(UserResetPasswordInput input)
        {
            var entity = await _userRepository.GetAsync(input.Id);
            var password = input.Password;
            if (password.IsNull())
            {
                password = _appConfig.DefaultPassword;
            }
            if (password.IsNull())
            {
                password = "111111";
            }
            entity.Password = Md5Encrypt.Encrypt(password);
            await _userRepository.UpdateAsync(entity);
            return password;
        }

        /// <summary>
        /// 设置主管
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task SetManagerAsync(UserSetManagerInput input)
        {
            var entity = await _userOrgRepository.Where(a => a.UserId == input.UserId && a.OrgId == input.OrgId).FirstAsync();
            entity.IsManager = input.IsManager;
            await _userOrgRepository.UpdateAsync(entity);
        }

        /// <summary>
        /// 彻底删除用户
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [AdminTransaction]
        public virtual async Task DeleteAsync(long id)
        {
            var user = await _userRepository.Select.WhereDynamic(id).ToOneAsync(a => new { a.Type });
            if (user == null)
            {
                throw ApiResult.ToException("用户不存在");
            }

            if (user.Type == UserType.PlatformAdmin || user.Type == UserType.TenantAdmin)
            {
                throw ApiResult.ToException("平台管理员禁止删除");
            }

            //删除用户角色
            await _userRoleRepository.DeleteAsync(a => a.UserId == id);
            //删除用户所属部门
            await _userOrgRepository.DeleteAsync(a => a.UserId == id);
            //删除员工
            await _staffRepository.DeleteAsync(a => a.Id == id);
            //删除用户
            await _userRepository.DeleteAsync(a => a.Id == id);

            await Cache.DelAsync(CacheKeys.DataPermission + id);
        }

        /// <summary>
        /// 批量彻底删除用户
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [AdminTransaction]
        public virtual async Task BatchDeleteAsync(long[] ids)
        {
            var admin = await _userRepository.Select.Where(a => ids.Contains(a.Id) &&
            (a.Type == UserType.PlatformAdmin || a.Type == UserType.TenantAdmin)).AnyAsync();

            if (admin)
            {
                throw ApiResult.ToException("平台管理员禁止删除");
            }

            //删除用户角色
            await _userRoleRepository.DeleteAsync(a => ids.Contains(a.UserId));
            //删除用户所属部门
            await _userOrgRepository.DeleteAsync(a => ids.Contains(a.UserId));
            //删除员工
            await _staffRepository.DeleteAsync(a => ids.Contains(a.Id));
            //删除用户
            await _userRepository.DeleteAsync(a => ids.Contains(a.Id));

            foreach (var userId in ids)
            {
                await Cache.DelAsync(CacheKeys.DataPermission + userId);
            }
        }

        /// <summary>
        /// 删除用户
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [AdminTransaction]
        public virtual async Task SoftDeleteAsync(long id)
        {
            var user = await _userRepository.Select.WhereDynamic(id).ToOneAsync(a => new { a.Type });
            if (user == null)
            {
                throw ApiResult.ToException("用户不存在");
            }

            if (user.Type == UserType.PlatformAdmin || user.Type == UserType.TenantAdmin)
            {
                throw ApiResult.ToException("平台管理员禁止删除");
            }

            await _userRoleRepository.DeleteAsync(a => a.UserId == id);
            await _userOrgRepository.DeleteAsync(a => a.UserId == id);
            await _staffRepository.SoftDeleteAsync(a => a.Id == id);
            await _userRepository.SoftDeleteAsync(id);

            await Cache.DelAsync(CacheKeys.DataPermission + id);
        }

        /// <summary>
        /// 批量删除用户
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [AdminTransaction]
        public virtual async Task BatchSoftDeleteAsync(long[] ids)
        {
            var admin = await _userRepository.Select.Where(a => ids.Contains(a.Id) &&
            (a.Type == UserType.PlatformAdmin || a.Type == UserType.TenantAdmin)).AnyAsync();

            if (admin)
            {
                throw ApiResult.ToException("平台管理员禁止删除");
            }

            await _userRoleRepository.DeleteAsync(a => ids.Contains(a.UserId));
            await _userOrgRepository.DeleteAsync(a => ids.Contains(a.UserId));
            await _staffRepository.SoftDeleteAsync(a => ids.Contains(a.Id));
            await _userRepository.SoftDeleteAsync(ids);

            foreach (var userId in ids)
            {
                await Cache.DelAsync(CacheKeys.DataPermission + userId);
            }
        }

        /// <summary>
        /// 上传头像
        /// </summary>
        /// <param name="file"></param>
        /// <param name="autoUpdate"></param>
        /// <returns></returns>
        public async Task<string> AvatarUpload(IFormFile file, bool autoUpdate = false)
        {
            /*var uploadConfig = LazyGetRequiredService<IOptionsMonitor<UploadConfig>>().CurrentValue;
            var uploadHelper = LazyGetRequiredService<UploadHelper>();
            var config = uploadConfig.Avatar;
            var fileInfo = await uploadHelper.UploadAsync(file, config, new { User.Id });
            if (autoUpdate)
            {
                var entity = await _userRepository.GetAsync(User.Id);
                entity.Avatar = fileInfo.FileRelativePath;
                await _userRepository.UpdateAsync(entity);
            }*/
            //return fileInfo.FileRelativePath;
            return await Task.FromResult("111");
        }
    }
}
