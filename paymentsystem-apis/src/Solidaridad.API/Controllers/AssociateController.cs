
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Solidaridad.Application.Models.Farmer;
using Solidaridad.Application.Models;
using Solidaridad.Application.Services;
using Solidaridad.Application.Services.Impl;
using Solidaridad.Core.Entities.Base;
using Solidaridad.Core.Entities.Pagination;
using Solidaridad.Application.Models.Associate;
using Solidaridad.Application.Models.LoanApplication;

namespace Solidaridad.API.Controllers;


[Authorize]
public class AssociateController : ApiController
{
    #region DI
    private readonly IAssociateService _associateService;
    public AssociateController(IAssociateService associateService)
    {
        _associateService = associateService;
    }
    #endregion
    [HttpPost]
    [Route("Search")]
    public async Task<IActionResult> Search(AssociateSearchParams associateSearchParams)
    {
        var associate = await _associateService.GetAssociatedFarmers(associateSearchParams.BatchId);

        int totalRecords = associate.Count();
        Page pageInfo = new Page
        {
            PageNumber = associateSearchParams.PageNumber,
            Size = associateSearchParams.PageSize,
            TotalElements = totalRecords,
            TotalPages = totalRecords / associateSearchParams.PageSize
        };
        var pagedData = new PagedData<List<FarmerResponseModel>>
        {
            Page = pageInfo,
            Result = associate.ToList()
        };

        return Ok(new ApiResponseModel<PagedData<List<FarmerResponseModel>>>
        {
            Success = true,
            Message = "success",
            Data = pagedData
        });
    }
   
    [HttpPost("add")]
    public async Task<IActionResult> Add(CreateAssociateModel createAssociateModel)
    {
        return Ok(ApiResult<CreateAssociateResponseModel>.Success(
            await _associateService.CreateAsync(createAssociateModel)));
    }

    [HttpPost("addRange")]
    public async Task<IActionResult> AddRange(IEnumerable<CreateAssociateModel> createAssociateModel)
    {
        return Ok(ApiResult<IEnumerable<CreateAssociateResponseModel>>.Success(
            await _associateService.MultiAdd(createAssociateModel)));
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> UpdateAsync(Guid id, UpdateAssociateModel updateAssociateModel)
    {
        return Ok(ApiResult<UpdateAssociateResponseModel>.Success(
            await _associateService.UpdateAsync(id, updateAssociateModel)));
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteAsync(Guid id)
    {
        return Ok(ApiResult<BaseResponseModel>.Success(await _associateService.DeleteAsync(id)));
    }
}
