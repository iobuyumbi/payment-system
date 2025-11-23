using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Solidaridad.Application.Models;
using Solidaridad.Application.Models.Ward;
using Solidaridad.Application.Services;
using System.Collections.ObjectModel;

namespace Solidaridad.API.Controllers;

[Authorize]
public class WardController : ApiController
{
    #region DI
    private readonly IWardService _wardService;
    public WardController(IWardService wardService)
    {
        _wardService = wardService;
    }
    #endregion

    #region Methods
    [HttpGet]
    public async Task<IActionResult> GetAll(bool? isActive)
    {
        var ward = await _wardService.GetAllAsync();
        if (isActive.HasValue)
        {
            return Ok(ApiResult<ReadOnlyCollection<WardResponseModel>>.Success(ward.Where(c => c.IsActive == isActive).ToList().AsReadOnly()));
        }

        return Ok(ApiResult<ReadOnlyCollection<WardResponseModel>>.Success(ward));
    }
    #endregion
}
