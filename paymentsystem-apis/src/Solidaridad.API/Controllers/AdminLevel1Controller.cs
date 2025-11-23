using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Solidaridad.Application.Models;
using Solidaridad.Application.Models.County;
using Solidaridad.Application.Models.Ward;
using Solidaridad.Application.Services;
using Solidaridad.Core.Entities.Base;
using Solidaridad.Core.Entities.Pagination;
using System.Collections.ObjectModel;

namespace Solidaridad.API.Controllers;

[Authorize]
public class AdminLevel1Controller : ApiController
{
    #region DI
    private readonly IAdminLevel1Service _countyService;
    public AdminLevel1Controller(IAdminLevel1Service countyService)
    {
        _countyService = countyService;
    }
    #endregion

    #region Methods
    [HttpPost]
    [Route("Search")]
    public async Task<IActionResult> GetAll(AdminLevel1SearchParams searchParams)
    {
        searchParams.CountryId = CountryId;
        var county = await _countyService.GetAllAsync(searchParams);
        int totalRecords = county.Count();
        Page pageInfo = new Page
        {
            PageNumber = searchParams.PageNumber,
            Size = searchParams.PageSize,
            TotalElements = totalRecords,
            TotalPages = totalRecords / searchParams.PageSize
        };
        var pagedData = new PagedData<List<AdminLevel1ResponseModel>>
        {
            Page = pageInfo,
            Result = county.ToList()
        };

        return Ok(new ApiResponseModel<PagedData<List<AdminLevel1ResponseModel>>>
        {
            Success = true,
            Message = "success",
            Data = pagedData
        });


    }
    
    [HttpPost]
    public async Task<IActionResult> CreateAsync(CreateAdminLevel1Model countryModel)
    {
        return Ok(ApiResult<CreateAdminLevel1ResponseModel>.Success(
            await _countyService.CreateAsync(countryModel)));
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> UpdateAsync(UpdateAdminLevel1Model countyModel,Guid id )
    {
        return Ok(ApiResult<UpdateAdminLevel1ResponseModel>.Success(
            await _countyService.UpdateAsync(id, countyModel)));
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteAsync(Guid id)
    {
        return Ok(ApiResult<BaseResponseModel>.Success(await _countyService.DeleteAsync(id)));
    }
    #endregion
}
