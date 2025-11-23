using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Solidaridad.Application.Models;
using Solidaridad.Application.Models.Project;
using Solidaridad.Application.Services;
using Solidaridad.Core.Entities.Base;
using Solidaridad.Core.Entities.Pagination;

namespace Solidaridad.API.Controllers;

[Authorize]
public class ProjectController : ApiController
{
    #region DI
    private readonly IProjectService _projectService;
    public ProjectController(IProjectService projectService)
    {
        _projectService = projectService;
    }
    #endregion

    #region Methods
    [HttpPost]
    [Route("Search")]
    public async Task<IActionResult> GetAll(ProjectSearchParams searchParams)
    {
       
        var farmers = await _projectService.GetAllAsync(searchParams);
        if (farmers.Count() > 0)
        {
            int totalRecords = farmers.Count();
            Page pageInfo = new Page
            {
                PageNumber = searchParams.PageNumber,
                Size = searchParams.PageSize,
                TotalElements = totalRecords,
                TotalPages = totalRecords / searchParams.PageSize
            };
            var pagedData = new PagedData<List<ProjectResponseModel>>
            {
                Page = pageInfo,
                Result = farmers.ToList()
            };

            return Ok(new ApiResponseModel<PagedData<List<ProjectResponseModel>>>
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

    [HttpPost]
    [Route("GetByCountryId")]
    public async Task<IActionResult> GetAllByCountryId(ProjectSearchParams searchParams)
    {
        searchParams.CountryId = CountryId;
        var farmers = await _projectService.GetAllAsync(searchParams);
        if (farmers.Count() > 0)
        {
            int totalRecords = farmers.Count();
            Page pageInfo = new Page
            {
                PageNumber = searchParams.PageNumber,
                Size = searchParams.PageSize,
                TotalElements = totalRecords,
                TotalPages = totalRecords / searchParams.PageSize
            };
            var pagedData = new PagedData<List<ProjectResponseModel>>
            {
                Page = pageInfo,
                Result = farmers.ToList()
            };

            return Ok(new ApiResponseModel<PagedData<List<ProjectResponseModel>>>
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


    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetProjectById(Guid id)
    {
        return Ok(ApiResult<IEnumerable<ProjectResponseModel>>.Success(
            await _projectService.GetAllByListIdAsync(id)));
    }

    [HttpPost]
    public async Task<IActionResult> CreateAsync(CreateProjectModel project)
    {
        return Ok(ApiResult<CreateProjectResponseModel>.Success(
            await _projectService.CreateAsync(project)));
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> UpdateAsync(Guid id, UpdateProjectModel projectModel)
    {
        return Ok(ApiResult<UpdateProjectResponseModel>.Success(
            await _projectService.UpdateAsync(id, projectModel)));
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteAsync(Guid id)
    {
        return Ok(ApiResult<BaseResponseModel>.Success(await _projectService.DeleteAsync(id)));
    } 
    #endregion
}
