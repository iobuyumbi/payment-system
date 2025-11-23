using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Solidaridad.Application.Models.Associate;
using Solidaridad.Application.Models;
using Solidaridad.Application.Services;
using Solidaridad.Application.Services.Impl;
using Solidaridad.Core.Entities.Base;
using Solidaridad.Core.Entities.Pagination;
using Solidaridad.Application.Models.Disbursement;

namespace Solidaridad.API.Controllers;

[Authorize]
public class DisbursementController : ApiController
{
    #region DI
    private readonly IDisbursementService _disbursementService;
    public DisbursementController(IDisbursementService disbursementService)
    {
        _disbursementService = disbursementService;
    }
    #endregion
    [HttpPost]
    [Route("Search")]
    public async Task<IActionResult> Search(DisbursementSearchParams disbursementSearchParams)
    {
        var associate = await _disbursementService.GetAllAsync();

        int totalRecords = associate.Count();
        Page pageInfo = new Page
        {
            PageNumber = disbursementSearchParams.PageNumber,
            Size = disbursementSearchParams.PageSize,
            TotalElements = totalRecords,
            TotalPages = totalRecords / disbursementSearchParams.PageSize
        };
        var pagedData = new PagedData<List<DisbursementResponseModel>>
        {
            Page = pageInfo,
            Result = associate.ToList()
        };

        return Ok(new ApiResponseModel<PagedData<List<DisbursementResponseModel>>>
        {
            Success = true,
            Message = "success",
            Data = pagedData
        });
    }

    [HttpPost("add")]
    public async Task<IActionResult> Add(CreateDisbursementModel createDisbursementModel)
    {
        return Ok(ApiResult<CreateDisbursementResponseModel>.Success(
            await _disbursementService.CreateAsync(createDisbursementModel)));
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> UpdateAsync(Guid id, UpdateDisbursementModel updateDisbursementModel)
    {
        return Ok(ApiResult<UpdateDisbursementResponseModel>.Success(
            await _disbursementService.UpdateAsync(id, updateDisbursementModel)));
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteAsync(Guid id)
    {
        return Ok(ApiResult<BaseResponseModel>.Success(await _disbursementService.DeleteAsync(id)));
    }
}
