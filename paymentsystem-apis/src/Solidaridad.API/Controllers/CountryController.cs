using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Solidaridad.Application.Models;
using Solidaridad.Application.Models.AdminLevels;
using Solidaridad.Application.Models.Country;
using Solidaridad.Application.Services;
using System.Collections.ObjectModel;

namespace Solidaridad.API.Controllers;

[Authorize]
public class CountryController : ApiController
{
    #region DI
    private readonly ICountryService _countryService;
    public CountryController(ICountryService countryService)
    {
        _countryService = countryService;
    }
    #endregion

    #region Methods
    [HttpGet]
    public async Task<IActionResult> GetAll(bool? isActive)
    {
        
        var countries = await _countryService.GetAllAsync();
        if (isActive.HasValue)
        {
            return Ok(ApiResult<ReadOnlyCollection<CountryResponseModel>>.Success(countries.Where(c => c.IsActive == isActive).ToList().AsReadOnly()));
        }

        return Ok(ApiResult<ReadOnlyCollection<CountryResponseModel>>.Success(countries));
    }

    [HttpGet("GetSelectedCountry")]
    public async Task<IActionResult> GetAllWithCountryId(bool? isActive)
    {
     

        var countries = await _countryService.GetAllWithCountryIdAsync(CountryId);
        if (isActive.HasValue)
        {
            return Ok(ApiResult<ReadOnlyCollection<CountryResponseModel>>.Success(countries.Where(c => c.IsActive == isActive).ToList().AsReadOnly()));
        }

        return Ok(ApiResult<ReadOnlyCollection<CountryResponseModel>>.Success(countries));
    }
    [HttpGet("GetAllAdminLevels")]
    public async Task<IActionResult> GetAllAdminLevels(string countryName)
    {
        var adminLevels = await _countryService.GetAllAdminLevels(countryName);
        //if (isActive.HasValue)
        //{
        //    return Ok(ApiResult<ReadOnlyCollection<AdminLevelResponseModel>>.Success(adminLevels.Country.Where(c => c.IsActive == isActive).ToList().AsReadOnly()));
        //}

        return Ok(ApiResult<AdminLevelResponseModel>.Success(adminLevels));
    }

    [HttpPost]
    public async Task<IActionResult> CreateAsync(CreateCountryModel countryModel)
    {
        return Ok(ApiResult<CreateCountryResponseModel>.Success(
            await _countryService.CreateAsync(countryModel)));
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> UpdateAsync(Guid id, UpdateCountryModel countryModel)
    {
        return Ok(ApiResult<UpdateCountryResponseModel>.Success(
            await _countryService.UpdateAsync(id, countryModel)));
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteAsync(Guid id)
    {
        return Ok(ApiResult<BaseResponseModel>.Success(await _countryService.DeleteAsync(id)));
    }

    #endregion
}
