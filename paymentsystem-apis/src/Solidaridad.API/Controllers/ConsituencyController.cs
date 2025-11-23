using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Solidaridad.Application.Models;
using Solidaridad.Application.Models.Constituency;
using Solidaridad.Application.Services;
using System.Collections.ObjectModel;

namespace Solidaridad.API.Controllers;

[Authorize]
public class ConsituencyController : ApiController
{
    #region DI
    private readonly IConstituencyService _constituencyService;
    public ConsituencyController(IConstituencyService constituencyService)
    {
        _constituencyService = constituencyService;
    }
    #endregion

    #region Methods
    [HttpGet]
    public async Task<IActionResult> GetAll(bool? isActive)
    {
        var constituency = await _constituencyService.GetAllAsync();
        if (isActive.HasValue)
        {
            return Ok(ApiResult<ReadOnlyCollection<ConstituencyResponseModel>>.Success(constituency.Where(c => c.IsActive == isActive).ToList().AsReadOnly()));
        }

        return Ok(ApiResult<ReadOnlyCollection<ConstituencyResponseModel>>.Success(constituency));
    }
    #endregion
}
