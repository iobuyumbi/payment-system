using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Solidaridad.Application.Models;
using Solidaridad.Application.Models.Permission;
using Solidaridad.Application.Models.RolePermission;
using Solidaridad.Application.Services;
using Solidaridad.Shared.Services;

namespace Solidaridad.API.Controllers;

[Authorize]
public class RolePermissionsController : ApiController
{
    #region DI
    private readonly IPermissionService _permissionService;
    private readonly IClaimService _claimService;

    public RolePermissionsController(IPermissionService permissionService, IClaimService claimService)
    {
        _permissionService = permissionService;
        _claimService = claimService;
    }
    #endregion

    #region Methods

    [HttpGet("permissions")]
    public async Task<ActionResult> GetPermissionsByRoleId(string roleId)
    {
        //var currentOrgId = Guid.Parse(_claimService.GetClaim("orgid"));
        var permissions = await _permissionService.GetAllAsync(roleId, new Guid());

        return Ok(ApiResult<IEnumerable<PermissionResponseModel>>.Success(permissions));
    }

    [HttpPut("save-permissions/{roleId}")]
    public async Task<IActionResult> Update(Guid roleId, IEnumerable<UpdateRolePermissionModel> model)
    {
        var updated = await _permissionService.UpdateRolePermissionAsync(roleId, model);
        return Ok(ApiResult<UpdateRolePermissionResponseModel>.Success(updated));
    }

    #endregion
}
