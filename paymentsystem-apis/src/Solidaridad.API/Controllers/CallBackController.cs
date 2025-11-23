using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Solidaridad.Application.Models.Project;
using Solidaridad.Application.Models;
using Solidaridad.Application.Services;
using Solidaridad.Application.Services.Impl;
using Solidaridad.Core.Entities.Base;
using Solidaridad.Core.Entities.Pagination;
using Solidaridad.Application.Models.CallBack;

namespace Solidaridad.API.Controllers;
[Authorize]
public class CallBackController : ApiController
{
    #region DI
    private readonly ICallBackService _callBackService;
    public CallBackController(ICallBackService callBackService)
    {
        _callBackService = callBackService;
    }
    #endregion

    #region Methods
    [AllowAnonymous]
    [HttpPost]
    [Route("Search")]
    public async Task<IActionResult> GetAll(SearchParams searchParams)
    {
        var data = await _callBackService.GetAllAsync();
        if (data.Count() > 0)
        {
            int totalRecords = data.Count();
            Page pageInfo = new Page
            {
                PageNumber = searchParams.PageNumber,
                Size = searchParams.PageSize,
                TotalElements = totalRecords,
                TotalPages = totalRecords / searchParams.PageSize
            };
            var pagedData = new PagedData<List<CallBackResponseModel>>
            {
                Page = pageInfo,
                Result = data.ToList()
            };

            return Ok(new ApiResponseModel<PagedData<List<CallBackResponseModel>>>
            {
                Success = true,
                Message = "success",
                Data = pagedData
            });
        }

        return Ok(new ApiResponseModel<PagedData<List<ProjectResponseModel>>>
        {
            Success = false,
            Message = "error",
            Data = null
        });

    }


    [AllowAnonymous]
    [HttpPost]
    public async Task<IActionResult> CreateAsync(CreateCallBackModel model)
    {
        return Ok(ApiResult<CreateCallBackResponseModel>.Success(
            await _callBackService.CreateAsync(model)));
    }
    #endregion
    //[HttpGet("{id:guid}")]
    //public async Task<IActionResult> GetProjectById(Guid id)
    //{
    //    return Ok(ApiResult<IEnumerable<ProjectResponseModel>>.Success(
    //        await _projectService.GetAllByListIdAsync(id)));
    //}
}
