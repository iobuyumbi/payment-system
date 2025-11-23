using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Solidaridad.Application.Models;
using Solidaridad.Application.Models.County;
using Solidaridad.Application.Services;
using System.Collections.ObjectModel;

namespace Solidaridad.API.Controllers;

[Authorize]
public class CountyController : ApiController
{
    #region DI
    private readonly ICountyService _countyService;
    public CountyController(ICountyService countyService)
    {
        _countyService = countyService;
    }
    #endregion

    #region Methods
    [HttpGet]
    public async Task<IActionResult> GetAll(bool? isActive)
    {
        var county = await _countyService.GetAllAsync();
        if (isActive.HasValue)
        {
            return Ok(ApiResult<ReadOnlyCollection<CountyResponseModel>>.Success(county.Where(c => c.IsActive == isActive).ToList().AsReadOnly()));
        }

        return Ok(ApiResult<ReadOnlyCollection<CountyResponseModel>>.Success(county));
    }
    #endregion
}
