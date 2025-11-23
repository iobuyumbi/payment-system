using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Solidaridad.Application.Models.County;
using Solidaridad.Application.Models;
using Solidaridad.Application.Services;
using Solidaridad.Core.Entities.Base;
using Solidaridad.Core.Entities.Pagination;
using Solidaridad.Application.Models.ApplicationStatus;

namespace Solidaridad.API.Controllers;

[Authorize]
public class ApplicationStatusController : ApiController
{
    #region DI
    private readonly IApplicationStatusService _statusService;
    public ApplicationStatusController(IApplicationStatusService statusService)
    {
        _statusService = statusService;
    }
    #endregion

    #region Methods
    [HttpPost]
    [Route("Search")]
    public async Task<IActionResult> GetAll(ApplicationStatusSearchParams searchParams)
    {
        var status = await _statusService.GetAllAsync(searchParams);
        int totalRecords = status.Count();
        Page pageInfo = new Page
        {
            PageNumber = searchParams.PageNumber,
            Size = searchParams.PageSize,
            TotalElements = totalRecords,
            TotalPages = totalRecords / searchParams.PageSize
        };
        var pagedData = new PagedData<List<ApplicationStatusResponseModel>>
        {
            Page = pageInfo,
            Result = status.ToList()
        };

        return Ok(new ApiResponseModel<PagedData<List<ApplicationStatusResponseModel>>>
        {
            Success = true,
            Message = "success",
            Data = pagedData
        });


    }

    [HttpPost]
    public async Task<IActionResult> CreateAsync(CreateApplicationStatusModel statusModel)
    {
        return Ok(ApiResult<CreateApplcationStatusResponseModel>.Success(
            await _statusService.CreateAsync(statusModel)));
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> UpdateAsync(UpdateApplicationStatusModel statusModel, Guid id)
    {
        return Ok(ApiResult<UpdateApplicationStatusResponseModel>.Success(
            await _statusService.UpdateAsync(id, statusModel)));
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteAsync(Guid id)
    {
        return Ok(ApiResult<BaseResponseModel>.Success(await _statusService.DeleteAsync(id)));
    }
    #endregion
}
