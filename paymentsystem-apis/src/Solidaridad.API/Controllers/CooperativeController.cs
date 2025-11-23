using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Solidaridad.Application.Models;
using Solidaridad.Application.Models.Cooperative;
using Solidaridad.Application.Services;
using Solidaridad.Core.Entities.Base;
using Solidaridad.Core.Entities.Pagination;

namespace Solidaridad.API.Controllers;

[Authorize]
public class CooperativeController : ApiController
{
    #region DI
    private readonly ICooperativeService _cooperativeService;
    public CooperativeController(ICooperativeService cooperativeService)
    {
        _cooperativeService = cooperativeService;
    }
    #endregion

    [HttpPost]
    [Route("Search")]
    public async Task<IActionResult> Search(CooperativeSearchParams cooperativeSearchParams)
    {
        cooperativeSearchParams.CountryId = CountryId;
        var cooperatives = await _cooperativeService.GetAllAsync(cooperativeSearchParams);

        int totalRecords = cooperatives.Count();
        Page pageInfo = new Page
        {
            PageNumber = cooperativeSearchParams.PageNumber,
            Size = cooperativeSearchParams.PageSize,
            TotalElements = totalRecords,
            TotalPages = totalRecords / cooperativeSearchParams.PageSize
        };
        var pagedData = new PagedData<List<CooperativeResponseModel>>
        {
            Page = pageInfo,
            Result = cooperatives.ToList()
        };

        return Ok(new ApiResponseModel<PagedData<List<CooperativeResponseModel>>>
        {
            Success = true,
            Message = "success",
            Data = pagedData
        });
    }
 

    [HttpPost("add")]
    public async Task<IActionResult> Add(CreateCooperativeModel createCooperativeModel)
    {
        return Ok(ApiResult<CreateCooperativeResponseModel>.Success(
            await _cooperativeService.CreateAsync(createCooperativeModel)));
    }

    [HttpPost]
    public async Task<IActionResult> Import(ImportCoperativeModel importCoperativeModel)
    {
        return Ok(ApiResult<CreateCooperativeResponseModel>.Success(
            await _cooperativeService.ImportAsync(importCoperativeModel)));
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> UpdateAsync(Guid id, UpdateCooperativeModel updateCooperativeModel)
    {
        return Ok(ApiResult<UpdateCooperativeResponseModel>.Success(
            await _cooperativeService.UpdateAsync(id, updateCooperativeModel)));
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteAsync(Guid id)
    {
        return Ok(ApiResult<BaseResponseModel>.Success(await _cooperativeService.DeleteAsync(id)));
    }
}
