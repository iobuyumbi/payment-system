using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Solidaridad.Application.Models;
using Solidaridad.Application.Models.LoanCategory;
using Solidaridad.Application.Services.Impl;
using Solidaridad.Core.Entities.Base;
using Solidaridad.Core.Entities.Pagination;

namespace Solidaridad.API.Controllers;

[Authorize]
public class LoanCategoryController : ApiController
{
    #region DI
    private readonly ILoanCategoryService _loanCategoryService;
    private readonly IMapper _mapper;
    public LoanCategoryController(ILoanCategoryService loanCategoryService, IMapper mapper)
    {
        _loanCategoryService = loanCategoryService;
        _mapper = mapper;
    }
    #endregion

    #region Methods
    [HttpPost]
    [Route("Search")]
    public async Task<IActionResult> Search(LoanCategorySearchParams loanCateogrySearchParams)
    {
        var loansCategory = await _loanCategoryService.GetAllAsync(loanCateogrySearchParams);

        int totalRecords = loansCategory.Count();
        Page pageInfo = new Page
        {
            PageNumber = loanCateogrySearchParams.PageNumber,
            Size = loanCateogrySearchParams.PageSize,
            TotalElements = totalRecords,
            TotalPages = totalRecords / loanCateogrySearchParams.PageSize
        };
        var pagedData = new PagedData<List<LoanCategoryResponseModel>>
        {
            Page = pageInfo,
            Result = loansCategory.OrderBy(c => c.Name).ToList()
        };

        return Ok(new ApiResponseModel<PagedData<List<LoanCategoryResponseModel>>>
        {
            Success = true,
            Message = "success",
            Data = pagedData
        });
    }

    [HttpPost]
    public async Task<IActionResult> Add(CreateLoanCategoryModel createloanCategoryModel)
    {
        return Ok(ApiResult<CreateLoanCategoryResponseModel>.Success(
            await _loanCategoryService.CreateAsync(createloanCategoryModel)));
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> UpdateAsync(Guid id, UpdateLoanCategoryModel updateLoanCategoryModel)
    {
        return Ok(ApiResult<UpdateLoanCategoryResponseModel>.Success(
            await _loanCategoryService.UpdateAsync(id, updateLoanCategoryModel)));
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteAsync(Guid id)
    {
        return Ok(ApiResult<BaseResponseModel>.Success(await _loanCategoryService.DeleteAsync(id)));
    }
    #endregion
}
