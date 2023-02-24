using Mapster;
using System.Linq;
using mlee.Core.Infrastructure.Entities.Permission;
using mlee.Core.Services.Permission.Dto;

namespace mlee.Core.Services.Admin.Permission;

/// <summary>
/// 映射配置
/// </summary>
public class MapConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config
        .NewConfig<PermissionEntity, PermissionGetDotOutput>()
        .Map(dest => dest.ApiIds, src => src.Apis.Select(a => a.Id));
    }
}