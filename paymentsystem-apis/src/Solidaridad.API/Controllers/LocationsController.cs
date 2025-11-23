using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Solidaridad.Application.Models;
using Solidaridad.Application.Models.Location;
using Solidaridad.Application.Services;
using Solidaridad.Core.Entities.Base;
using Solidaridad.Core.Entities.Pagination;

namespace Solidaridad.API.Controllers;

[Authorize]
public class LocationsController : ApiController
{
    #region DI
    private readonly ILocationService _locationService;
    public LocationsController(ILocationService locationService)
    {
        _locationService = locationService;
    }
    #endregion

    #region Methods
    [HttpPost]
    [Route("search")]
    public async Task<IActionResult> GetAll(SearchParams searchParams)
    {

        var locations = await _locationService.GetAllAsync(searchParams);
        if (locations.Count() > 0)
        {
            int totalRecords = locations.Count();
            Page pageInfo = new Page
            {
                PageNumber = searchParams.PageNumber,
                Size = searchParams.PageSize,
                TotalElements = totalRecords,
                TotalPages = totalRecords / searchParams.PageSize
            };
            var pagedData = new PagedData<List<LocationResponseModel>>
            {
                Page = pageInfo,
                Result = locations.ToList()
            };

            return Ok(new ApiResponseModel<PagedData<List<LocationResponseModel>>>
            {
                Success = true,
                Message = "success",
                Data = pagedData
            });
        }

        return Ok(new ApiResponseModel<PagedData<List<LocationResponseModel>>>
        {
            Success = false,
            Message = "error",
            Data = null
        });

    }

    [HttpPost]
    [Route("get-by-location")]
    public async Task<IActionResult> GetAllByLocationId(SearchParams searchParams)
    {
        searchParams.CountryId = CountryId;
        var locations = await _locationService.GetAllAsync(searchParams);
        if (locations.Count() > 0)
        {
            int totalRecords = locations.Count();
            Page pageInfo = new Page
            {
                PageNumber = searchParams.PageNumber,
                Size = searchParams.PageSize,
                TotalElements = totalRecords,
                TotalPages = totalRecords / searchParams.PageSize
            };
            var pagedData = new PagedData<List<LocationResponseModel>>
            {
                Page = pageInfo,
                Result = locations.ToList()
            };

            return Ok(new ApiResponseModel<PagedData<List<LocationResponseModel>>>
            {
                Success = true,
                Message = "success",
                Data = pagedData
            });
        }

        return Ok(new ApiResponseModel<PagedData<List<LocationResponseModel>>>
        {
            Success = false,
            Message = "error",
            Data = null
        });

    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetLocationById(Guid id, Guid countryId)
    {
        return Ok(ApiResult<LocationResponseModel>.Success(
            await _locationService.GetByIdAsync(id, countryId)));
    }

    [HttpPost]
    public async Task<IActionResult> CreateAsync(CreateLocationModel location)
    {
        return Ok(ApiResult<CreateLocationResponseModel>.Success(
            await _locationService.CreateAsync(location)));
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> UpdateAsync(Guid id, UpdateLocationModel locationModel)
    {
        return Ok(ApiResult<UpdateLocationResponseModel>.Success(
            await _locationService.UpdateAsync(id, locationModel)));
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteAsync(Guid id)
    {
        return Ok(ApiResult<BaseResponseModel>.Success(await _locationService.DeleteAsync(id)));
    }
    #endregion
}
