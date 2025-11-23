using Solidaridad.Application.Models;
using Solidaridad.Application.Models.Role;
using Solidaridad.Core.Entities.Base;

namespace Solidaridad.Application.Services;

public interface IRoleService
{
    Task<CreateRoleResponseModel> CreateAsync(RoleResponseModel project);

    Task<BaseResponseModel> DeleteAsync(Guid id);

    Task<IEnumerable<RoleResponseModel>> GetAllAsync(RoleSearchParams searchParams);

    Task<UpdateRoleResponseModel> UpdateAsync(Guid id, RoleResponseModel projectModel);
    Task<IEnumerable<RoleResponseModel>> GetAllByListIdAsync(Guid id, CancellationToken cancellationToken = default);
}
