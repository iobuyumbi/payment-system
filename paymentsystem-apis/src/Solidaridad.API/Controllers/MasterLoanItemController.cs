using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Solidaridad.Application.Models;
using Solidaridad.Application.Models.LoanItem;
using Solidaridad.Application.Services;
using Solidaridad.Core.Entities.Base;
using Solidaridad.Core.Entities.Pagination;

namespace Solidaridad.API.Controllers;

[Authorize]
public class MasterLoanItemController : ApiController
{
   private readonly IMasterLoanItemService _loanItemService;
    public MasterLoanItemController(IMasterLoanItemService loanItemService)
    {
        _loanItemService = loanItemService;
    }

    [HttpPost]
    [Route("Search")]
    public async Task<IActionResult> Search(LoanItemSearchParams loanItemSearchParams)
    {
        var loanItems = await _loanItemService.GetAllAsync(loanItemSearchParams);

        int totalRecords = loanItems.Count();
        Page pageInfo = new Page
        {
            PageNumber = loanItemSearchParams.PageNumber,
            Size = loanItemSearchParams.PageSize,
            TotalElements = totalRecords,
            TotalPages = totalRecords / loanItemSearchParams.PageSize
        };
        var pagedData = new PagedData<List<MasterLoanItemResponseModel>>
        {
            Page = pageInfo,
            Result = loanItems.ToList()
        };

        return Ok(new ApiResponseModel<PagedData<List<MasterLoanItemResponseModel>>>
        {
            Success = true,
            Message = "success",
            Data = pagedData
        });
    }

    [HttpPost]
    public async Task<IActionResult> Add(CreateMasterLoanItemModel createLoanItemModel)
    {
        return Ok(ApiResult<CreateMasterLoanItemResponseModel>.Success(
            await _loanItemService.CreateAsync(createLoanItemModel)));
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> UpdateAsync(Guid id, UpdateMasterLoanItemModel updateFarmerModel)
    {
        return Ok(ApiResult<UpdateMasterLoanItemResponseModel>.Success(
            await _loanItemService.UpdateAsync(id, updateFarmerModel)));
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteAsync(Guid id)
    {
        return Ok(ApiResult<BaseResponseModel>.Success(await _loanItemService.DeleteAsync(id)));
    }
}
