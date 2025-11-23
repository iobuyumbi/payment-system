using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Solidaridad.Application.Models;
using Solidaridad.Application.Models.LocationProfiles;
using Solidaridad.Application.Services;
using Solidaridad.Core.Entities.Base;
using Solidaridad.Core.Entities.Pagination;

namespace Solidaridad.API.Controllers;

[Authorize]
public class LocationProfilesController : ApiController
{
    #region DI
    private readonly ILocationProfileService _locationProfileService;
    public LocationProfilesController(ILocationProfileService locationProfileService)
    {
        _locationProfileService = locationProfileService;
    }
    #endregion

    #region Methods
    [HttpPost]
    [Route("search")]
    public async Task<IActionResult> GetAll(SearchParams searchParams)
    {
        searchParams.CountryId = CountryId;

        var locations = await _locationProfileService.GetAllAsync(searchParams);
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
            var pagedData = new PagedData<List<LocationProfileResponseModel>>
            {
                Page = pageInfo,
                Result = locations.ToList()
            };

            return Ok(new ApiResponseModel<PagedData<List<LocationProfileResponseModel>>>
            {
                Success = true,
                Message = "success",
                Data = pagedData
            });
        }

        return Ok(new ApiResponseModel<PagedData<List<LocationProfileResponseModel>>>
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
        var locations = await _locationProfileService.GetAllAsync(searchParams);
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
            var pagedData = new PagedData<List<LocationProfileResponseModel>>
            {
                Page = pageInfo,
                Result = locations.ToList()
            };

            return Ok(new ApiResponseModel<PagedData<List<LocationProfileResponseModel>>>
            {
                Success = true,
                Message = "success",
                Data = pagedData
            });
        }

        return Ok(new ApiResponseModel<PagedData<List<LocationProfileResponseModel>>>
        {
            Success = false,
            Message = "error",
            Data = null
        });

    }


    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetLocationById(Guid id)
    {
     
        return Ok(ApiResult<LocationProfileResponseModel>.Success(
            await _locationProfileService.GetByIdAsync(id)));
    }

    [HttpPost]
    public async Task<IActionResult> CreateAsync(CreateLocationProfileModel location)
    {
        location.CountryId = (Guid)CountryId;
        return Ok(ApiResult<CreateLocationProfileResponseModel>.Success(
            await _locationProfileService.CreateAsync(location)));
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> UpdateAsync(Guid id, UpdateLocationProfileModel locationModel)
    {
        return Ok(ApiResult<UpdateLocationProfileResponseModel>.Success(
            await _locationProfileService.UpdateAsync(id, locationModel)));
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteAsync(Guid id)
    {
        return Ok(ApiResult<BaseResponseModel>.Success(await _locationProfileService.DeleteAsync(id)));
    }
    #endregion
}
