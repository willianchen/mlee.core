using Mapster;
using System.Linq;
using mlee.Core.Services.Role.Dto;

namespace mlee.Core.Services.Role;

/// <summary>
/// 映射配置
/// </summary>
public class MapConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config
        .NewConfig<RoleGetOutput, RoleGetOutput>()
        .Map(dest => dest.OrgIds, src => src.Orgs.Select(a => a.Id));
    }
}