using AutoMapper;
using mlee.Core.Admin.Services;
using mlee.Core.Library.Configs;
using mlee.Core.Library.Maps;
using mlee.Core.Repository.User;
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

        public UserService()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<UserGetOutput> GetAsync(long id)
        {
            var userEntity = await _userRepository.Select
      .WhereDynamic(id)
      //  .IncludeMany(a => a.Roles.Select(b => new RoleEntity { Id = b.Id, Name = b.Name }))
      // .IncludeMany(a => a.Orgs.Select(b => new OrgEntity { Id = b.Id, Name = b.Name }))
      .ToOneAsync(a => new
      {
          a.Id,
          a.UserName,
          a.Name,
          a.Mobile,
          a.Email,
          a.OrgId,
          a.ManagerUserId,
          ManagerUserName = a.ManagerUser.Name,
      });

            var output = userEntity.MapTo<UserGetOutput>();
            //Mapper.Map<>(userEntity);

            return output;
        }
    }
}
