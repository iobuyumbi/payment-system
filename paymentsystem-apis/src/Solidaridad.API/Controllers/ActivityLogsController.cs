using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Solidaridad.Application.Models;
using Solidaridad.Application.Models.ActivityLog;
using Solidaridad.Application.Services;

namespace Solidaridad.API.Controllers;

[Authorize]
public class ActivityLogsController : ApiController
{
    #region DI

    private readonly IActivityLogService _activityLogService;

    public ActivityLogsController(IActivityLogService activityLogService)
    {
        _activityLogService = activityLogService;
    }

    #endregion

    #region Methods

    [HttpGet]
    public async Task<IActionResult> GetAll(string keyword)
    {
        return Ok(ApiResult<IEnumerable<ActivityLogResponseModel>>.Success(await _activityLogService.GetAllAsync(keyword)));
    }

    [HttpPost]
    public async Task<IActionResult> CreateAsync(CreateActivityLogModel createActivityLogModel)
    {
        return Ok(ApiResult<CreateActivityLogResponseModel>.Success(
            await _activityLogService.CreateAsync(createActivityLogModel)));
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> UpdateAsync(Guid id, UpdateActivityLogModel updateActivityLogModel)
    {
        return Ok(ApiResult<UpdateActivityLogResponseModel>.Success(
            await _activityLogService.UpdateAsync(id, updateActivityLogModel)));
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteAsync(Guid id)
    {
        return Ok(ApiResult<BaseResponseModel>.Success(await _activityLogService.DeleteAsync(id)));
    }

    #endregion
}
