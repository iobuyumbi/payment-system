using Microsoft.AspNetCore.Http;
using Solidaridad.Application.Models;
using Solidaridad.Application.Models.Permission;
using Solidaridad.Application.Models.RolePermission;

namespace Solidaridad.Application.Services;

public interface IPermissionService
{
    Task<CreatePermissionResponseModel> CreateAsync(CreatePermissionModel createPermissionModel, CancellationToken cancellationToken = default);
    
    Task<BaseResponseModel> DeleteAsync(Guid id, CancellationToken cancellationToken = default);
    
    Task<IEnumerable<PermissionResponseModel>> GetAllAsync(string keyword, Guid orgId, CancellationToken cancellationToken = default);
    Task ImportPermission(IFormFile file, Guid? id);
    Task<UpdatePermissionResponseModel> UpdateAsync(Guid id, UpdatePermissionModel updatePermissionModel, CancellationToken cancellationToken = default);
    
    Task<UpdateRolePermissionResponseModel> UpdateRolePermissionAsync(Guid roleId, IEnumerable<UpdateRolePermissionModel> updateRolePermissionModel, CancellationToken cancellationToken = default);
}
