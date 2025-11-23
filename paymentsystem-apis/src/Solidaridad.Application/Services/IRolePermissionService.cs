using Solidaridad.Application.Models;
using Solidaridad.Application.Models.RolePermission;
using Solidaridad.Core.Entities.Base;

namespace Solidaridad.Application.Services;

public interface IRolePermissionService
{
    Task<CreateRolePermissionResponseModel> CreateAsync(RolePermissionResponseModel project);

    Task<BaseResponseModel> DeleteAsync(Guid id);

    Task<IEnumerable<RolePermissionResponseModel>> GetAllAsync(RolePermissionSearchParams searchParams);

    Task<UpdateRolePermissionResponseModel> UpdateAsync(Guid id, RolePermissionResponseModel projectModel);
    Task<IEnumerable<RolePermissionResponseModel>> GetAllByListIdAsync(Guid id, CancellationToken cancellationToken = default);
}
