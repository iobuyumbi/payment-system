using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Solidaridad.Application.Models;
using Solidaridad.Application.Models.Farmer;
using Solidaridad.Application.Services;
using Solidaridad.Core.Entities.Base;
using Solidaridad.Core.Entities.Pagination;

namespace Solidaridad.API.Controllers;

[Authorize]
public class FarmerController : ApiController
{
    #region DI
    private readonly IFarmerService _farmerService;
    public FarmerController(IFarmerService farmerService)
    {
        _farmerService = farmerService;
    }
    #endregion

    [HttpPost]
    [Route("Search")]
    public async Task<IActionResult> Search(FarmerSearchParams farmerSearchParams)
    {
        farmerSearchParams.CountryId = CountryId;
        var farmers = await _farmerService.GetAllAsync(farmerSearchParams);

        int totalRecords = farmers != null ? farmers.TotalCount : 0;

        Page pageInfo = new Page
        {
            PageNumber = farmerSearchParams.PageNumber,
            Size = farmerSearchParams.PageSize,
            TotalElements = totalRecords,
            TotalPages = totalRecords / farmerSearchParams.PageSize
        };
        var pagedData = new PagedData<FarmerSearchResponseModel>
        {
            Page = pageInfo,
            Result = farmers
        };

        return Ok(new ApiResponseModel<PagedData<FarmerSearchResponseModel>>
        {
            Success = true,
            Message = "success",
            Data = pagedData
        });
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetAllAsync(Guid id)
    {
        return Ok(ApiResult<IEnumerable<FarmerResponseModel>>.Success(
            await _farmerService.GetAllByListIdAsync(id)));
    }

    [HttpPost]
    public async Task<IActionResult> Import(ImportFarmerModel importFarmerModel)
    {
        return Ok(ApiResult<ImportFarmerResponseModel>.Success(
            await _farmerService.ImportByApiAsync(importFarmerModel)));
    }

    [HttpPost("add")]
    public async Task<IActionResult> Add(CreateFarmerModel createFarmerModel)
    {
        return Ok(ApiResult<CreateFarmerResponseModel>.Success(
            await _farmerService.CreateAsync(createFarmerModel)));
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> UpdateAsync(Guid id, UpdateFarmerModel updateFarmerModel)
    {
        return Ok(ApiResult<UpdateFarmerResponseModel>.Success(
            await _farmerService.UpdateAsync(id, updateFarmerModel)));
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteAsync(Guid id)
    {
        return Ok(ApiResult<BaseResponseModel>.Success(await _farmerService.DeleteAsync(id)));
    }

    [HttpGet("cooperatives/{id:guid}")]
    public async Task<IActionResult> GetCooperativesAsync(Guid id)
    {
        return Ok(ApiResult<IEnumerable<SelectItemModel>>.Success(
            await _farmerService.GetCooperatives(id)));
    }

    [HttpGet("projects/{id:guid}")]
    public async Task<IActionResult> GetProjectsAsync(Guid id)
    {
        return Ok(ApiResult<IEnumerable<SelectItemModel>>.Success(
            await _farmerService.GetProjects(id)));
    }

    [HttpGet("masterDocTypes")]
    public async Task<IActionResult> GetMasterDocumentTypes(Guid id)
    {
        return Ok(ApiResult<IEnumerable<SelectItemModel>>.Success(
            await _farmerService.GetMasterDocumentTypes()));
    }
}
