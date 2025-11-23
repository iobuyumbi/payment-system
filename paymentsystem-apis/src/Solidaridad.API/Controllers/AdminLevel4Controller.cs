using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Solidaridad.Application.Models.SubCounty;
using Solidaridad.Application.Models;
using Solidaridad.Application.Services;
using System.Collections.ObjectModel;
using Solidaridad.Core.Entities;
using Solidaridad.Application.Models.Village;

namespace Solidaridad.API.Controllers;

[Authorize]
public class AdminLevel4Controller : ApiController
{
    #region DI
    private readonly IAdminLevel4Service _villageService;
    public AdminLevel4Controller(IAdminLevel4Service villageService)
    {
        _villageService = villageService;
        
    }
    #endregion

    #region Methods

    [HttpPost]
    [Route("Search")]
    public async Task<IActionResult> GetAll([FromBody]  Guid wardId)
    {
        var village = await _villageService.GetAllAsync();
        if (wardId != null)
        {
            return Ok(ApiResult<ReadOnlyCollection<AdminLevel4ResponseModel>>.Success(village.Where(c => c.WardId == wardId).ToList().AsReadOnly()));
        }

        return Ok(ApiResult<ReadOnlyCollection<AdminLevel4ResponseModel>>.Success(village));
    }
    [HttpPost]
    public async Task<IActionResult> CreateAsync(CreateAdminLevel4Model village)
    {
        return Ok(ApiResult<CreateAdminLevel4ResponseModel>.Success(
            await _villageService.CreateAsync(village)));
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> UpdateAsync(Guid id, UpdateAdminLevel4Model village)
    {
        return Ok(ApiResult<UpdateAdminLevel4ResponseModel>.Success(
            await _villageService.UpdateAsync(id, village)));
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteAsync(Guid id)
    {
        return Ok(ApiResult<BaseResponseModel>.Success(await _villageService.DeleteAsync(id)));
    }
    #endregion
}
