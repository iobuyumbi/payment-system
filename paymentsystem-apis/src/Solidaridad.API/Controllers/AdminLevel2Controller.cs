using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Solidaridad.Application.Models.County;
using Solidaridad.Application.Models;
using Solidaridad.Application.Services;
using System.Collections.ObjectModel;
using Solidaridad.Application.Models.SubCounty;
using Solidaridad.Core.Entities.Base;
using Solidaridad.Core.Entities.Pagination;

namespace Solidaridad.API.Controllers;

[Authorize]
public class AdminLevel2Controller : ApiController
{
    #region DI
    private readonly IAdminLevel2Service _subCountyService;
    public AdminLevel2Controller(IAdminLevel2Service subCountyService)
    {
        _subCountyService = subCountyService;
    }
    #endregion

    #region Methods

    [HttpPost]
    [Route("Search")]
    public async Task<IActionResult> GetAll(AdminLevel2SearchParams searchParams)
    {
        searchParams.CountryId = CountryId;
        var subCounty = await _subCountyService.GetAllAsync(searchParams);
        int totalRecords = subCounty.Count();
        Page pageInfo = new Page
        {
            PageNumber = searchParams.PageNumber,
            Size = searchParams.PageSize,
            TotalElements = totalRecords,
            TotalPages = totalRecords / searchParams.PageSize
        };
        var pagedData = new PagedData<List<AdminLevel2ResponseModel>>
        {
            Page = pageInfo,
            Result = subCounty.ToList()
        };

        return Ok(new ApiResponseModel<PagedData<List<AdminLevel2ResponseModel>>>
        {
            Success = true,
            Message = "success",
            Data = pagedData
        });
        
    }
    [HttpPost]
    public async Task<IActionResult> CreateAsync(CreateAdminLevel2Model subCountyModel)
    {
        return Ok(ApiResult<CreateAdminLevel2ResponseModel>.Success(
            await _subCountyService.CreateAsync(subCountyModel)));
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> UpdateAsync(Guid id, UpdateAdminLevel2Model subCountyModel)
    {
        return Ok(ApiResult<UpdateAdminLevel2ResponseModel>.Success(
            await _subCountyService.UpdateAsync(id, subCountyModel)));
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteAsync(Guid id)
    {
        return Ok(ApiResult<BaseResponseModel>.Success(await _subCountyService.DeleteAsync(id)));
    }
    #endregion
}
