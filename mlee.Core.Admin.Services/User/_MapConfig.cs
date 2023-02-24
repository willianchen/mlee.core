using Mapster;
using System.Linq;
using mlee.Core.Services.User.Dto;

namespace mlee.Core.Services.User;

/// <summary>
/// 映射配置
/// </summary>
public class MapConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config
        .NewConfig<UserGetPageOutput, UserGetPageOutput>()
        .Map(dest => dest.RoleNames, src => src.Roles.Select(a => a.Name));
    }
}