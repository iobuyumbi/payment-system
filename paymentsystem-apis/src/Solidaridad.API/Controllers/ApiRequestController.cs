using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Solidaridad.Application.Models;
using Solidaridad.Application.Models.ApiRequestLog;
using Solidaridad.Application.Services.Impl;
using Solidaridad.Core.Entities.Base;
using Solidaridad.Core.Entities.Pagination;

namespace Solidaridad.API.Controllers;

[Authorize]
public class ApiRequestController : ApiController
{
    #region DI
    private readonly ApiService _apiService;
    public ApiRequestController(ApiService apiService)
    {
        _apiService = apiService;
    }
    #endregion

    #region Methods
    [HttpPost]
    [Route("GetAll")]
    public async Task<IActionResult> GetAll(CooperativeSearchParams cooperativeSearchParams)
    {
        var apiRequests = await _apiService.GetAllAsync();

        int totalRecords = apiRequests.ApiRequestLogResponseModel.Count();
        Page pageInfo = new Page
        {
            PageNumber = cooperativeSearchParams.PageNumber,
            Size = cooperativeSearchParams.PageSize,
            TotalElements = totalRecords,
            TotalPages = totalRecords / cooperativeSearchParams.PageSize
        };
        var pagedData = new PagedData<TransactionsResponseModel>
        {
            Page = pageInfo,
            Result = apiRequests
        };

        return Ok(new ApiResponseModel<PagedData<TransactionsResponseModel>>
        {
            Success = true,
            Message = "success",
            Data = pagedData
        });
    }

    [HttpGet("GetSingleResponseBody/{id:guid}")]
    public async Task<IActionResult> GetSingleResponseBody(Guid id)
    {
        return Ok(ApiResult<string>.Success(
            await _apiService.GetSingleResponseBody(id)));
    }
    
    [HttpGet("GetSingleRequestBody/{id:guid}")]
    public async Task<IActionResult> GetSingleRequestBody(Guid id)
    {
        return Ok(ApiResult<string>.Success(
            await _apiService.GetSingleRequestBody(id)));
    }
    #endregion
}
