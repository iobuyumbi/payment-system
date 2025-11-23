using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Solidaridad.Application.Models.LoanItem;
using Solidaridad.Application.Models;
using Solidaridad.Application.Services;
using Solidaridad.Application.Services.Impl;
using Solidaridad.Core.Entities.Base;
using Solidaridad.Core.Entities.Pagination;

namespace Solidaridad.API.Controllers;

[Authorize]
public class LoanItemController : ApiController
{
    #region DI
    private readonly ILoanItemService _loanItemService;
    private readonly IMapper _mapper;
    public LoanItemController(ILoanItemService loanItemService, IMapper mapper)
    {
        _loanItemService = loanItemService;
        _mapper = mapper;
    }
    #endregion

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
        var pagedData = new PagedData<List<LoanItemResponseModel>>
        {
            Page = pageInfo,
            Result = loanItems.ToList()
        };

        return Ok(new ApiResponseModel<PagedData<List<LoanItemResponseModel>>>
        {
            Success = true,
            Message = "success",
            Data = pagedData
        });
    }

    [HttpPost]
    public async Task<IActionResult> Add(CreateLoanItemModel createLoanItemModel)
    {
        return Ok(ApiResult<CreateLoanItemResponseModel>.Success(
            await _loanItemService.CreateAsync(createLoanItemModel)));
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> UpdateAsync(Guid id, UpdateLoanItemModel updateLoanItemModel)
    {
        return Ok(ApiResult<UpdateLoanItemResponseModel>.Success(
            await _loanItemService.UpdateAsync(id, updateLoanItemModel)));
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteAsync(Guid id)
    {
        return Ok(ApiResult<BaseResponseModel>.Success(await _loanItemService.DeleteAsync(id)));
    }

}
