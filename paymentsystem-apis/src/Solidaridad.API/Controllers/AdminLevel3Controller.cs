using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NPOI.SS.Formula.Functions;
using Solidaridad.Application.Models;
using Solidaridad.Application.Models.SubCounty;
using Solidaridad.Application.Models.Village;
using Solidaridad.Application.Models.Ward;
using Solidaridad.Application.Services;
using Solidaridad.Application.Services.Impl;
using Solidaridad.Core.Entities.Base;
using Solidaridad.Core.Entities.Pagination;
using Solidaridad.DataAccess.Repositories;
using System.Collections.ObjectModel;

namespace Solidaridad.API.Controllers;

[Authorize]
public class AdminLevel3Controller : ApiController
{
    #region DI
   
    private readonly IAdminLevel3Service _wardService;
    public AdminLevel3Controller(IAdminLevel3Service wardService )
    {
        _wardService = wardService;
       
    }
    #endregion

    #region Methods
    [HttpPost]
    [Route("Search")]
    public async Task<IActionResult> GetAll(AdminLevel3SearchParams searchParams)
    {
        searchParams.CountryId = CountryId;
        var ward = await _wardService.GetAllAsync(searchParams);
        int totalRecords = ward.Count();
        Page pageInfo = new Page
        {
            PageNumber = searchParams.PageNumber,
            Size = searchParams.PageSize,
            TotalElements = totalRecords,
            TotalPages = totalRecords / searchParams.PageSize
        };
        var pagedData = new PagedData<List<AdminLevel3ResponseModel>>
        {
            Page = pageInfo,
            Result = ward.ToList()
        };

        return Ok(new ApiResponseModel<PagedData<List<AdminLevel3ResponseModel>>>
        {
            Success = true,
            Message = "success",
            Data = pagedData
        });
      
    }
    [HttpPost]
    public async Task<IActionResult> CreateAsync(CreateAdminLevel3Model ward)
    {
        return Ok(ApiResult<CreateAdminLevel3ResponseModel>.Success(
            await _wardService.CreateAsync(ward)));
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> UpdateAsync(Guid id, UpdateAdminLevel3Model ward)
    {
        return Ok(ApiResult<UpdateAdminLevel3ResponseModel>.Success(
            await _wardService.UpdateAsync(id, ward)));
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteAsync(Guid id)
    {
        return Ok(ApiResult<BaseResponseModel>.Success(await _wardService.DeleteAsync(id)));
    }
    #endregion
}
