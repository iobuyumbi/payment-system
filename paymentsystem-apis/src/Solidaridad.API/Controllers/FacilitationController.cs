using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Solidaridad.Application.Models.Disbursement;
using Solidaridad.Application.Models;
using Solidaridad.Application.Services;
using Solidaridad.Core.Entities.Base;
using Solidaridad.Core.Entities.Pagination;
using Solidaridad.Application.Models.Facilitation;

namespace Solidaridad.API.Controllers;
[Authorize]

public class FacilitationController : ApiController
{
    #region DI
    private readonly IFacilitationService _facilitationService;
    public FacilitationController(IFacilitationService facilitationService)
    {
        _facilitationService = facilitationService;
    }
    #endregion

    [HttpPost]
    [Route("Search")]
    public async Task<IActionResult> Search(FacilitationSearchParams facilitationSearchParams)
    {
        var associate = await _facilitationService.GetAllAsync(facilitationSearchParams);

        int totalRecords = associate.Count();
        Page pageInfo = new Page
        {
            PageNumber = facilitationSearchParams.PageNumber,
            Size = facilitationSearchParams.PageSize,
            TotalElements = totalRecords,
            TotalPages = totalRecords / facilitationSearchParams.PageSize
        };
        var pagedData = new PagedData<List<FacilitationResponseModel>>
        {
            Page = pageInfo,
            Result = associate.ToList()
        };

        return Ok(new ApiResponseModel<PagedData<List<FacilitationResponseModel>>>
        {
            Success = true,
            Message = "success",
            Data = pagedData
        });
    }

    [HttpPost("add")]
    public async Task<IActionResult> Add(CreateFacilitationModel createFacilitationModel)
    {
        return Ok(ApiResult<CreateFacilitationResponseModel>.Success(
            await _facilitationService.CreateAsync(createFacilitationModel)));
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> UpdateAsync(Guid id, UpdateFacilitationModel updateFacilitationModel)
    {
        return Ok(ApiResult<UpdateFacilitationResponseModel>.Success(
            await _facilitationService.UpdateAsync(id, updateFacilitationModel)));
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteAsync(Guid id)
    {
        return Ok(ApiResult<BaseResponseModel>.Success(await _facilitationService.DeleteAsync(id)));
    }
}
