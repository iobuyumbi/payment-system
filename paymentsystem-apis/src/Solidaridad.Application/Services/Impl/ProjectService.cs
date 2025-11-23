using AutoMapper;
using FluentValidation;
using Microsoft.Extensions.Caching.Memory;
using Solidaridad.Application.Models;
using Solidaridad.Application.Models.Cooperative;
using Solidaridad.Application.Models.Project;
using Solidaridad.Application.Models.Validators.Cooperative;
using Solidaridad.Application.Models.Validators.Project;
using Solidaridad.Core.Entities;
using Solidaridad.Core.Entities.Base;
using Solidaridad.DataAccess.Repositories;
using Solidaridad.DataAccess.Repositories.Impl;
using System.Collections.ObjectModel;

namespace Solidaridad.Application.Services.Impl;

public class ProjectService : IProjectService
{
    #region DI
    private readonly IMapper _mapper;
    private readonly IProjectRepository _projectRepository;
    private readonly ICountryRepository _countryRepository;
    private readonly IMemoryCache _memoryCache;
    private readonly CreateProjectValidator _createProjectValidator;
    private readonly UpdateProjectValidator _updateProjectValidator;
    public ProjectService(IMapper mapper, IProjectRepository projectRepository,
        ICountryRepository countryRepository,
        IMemoryCache memoryCache)
    {
        _mapper = mapper;
        _projectRepository = projectRepository;
        _countryRepository = countryRepository;
        _memoryCache = memoryCache;
        _createProjectValidator = new CreateProjectValidator();
        _updateProjectValidator = new UpdateProjectValidator();
    }
    #endregion

    #region Methods
    public async Task<CreateProjectResponseModel> CreateAsync(CreateProjectModel projectModel)
    {
        try
        {
            var validationResult = _createProjectValidator.Validate(projectModel);
            if (!validationResult.IsValid)
            {

                throw new ValidationException("Validation failed", validationResult.Errors);
            }


            var project = _mapper.Map<Project>(projectModel);
            var addedProject = await _projectRepository.AddAsync(project);
            return new CreateProjectResponseModel
            {
                Id = addedProject.Id
            };
        }
        catch (ValidationException ex)
        {
            throw ex;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    public async Task<BaseResponseModel> DeleteAsync(Guid id)
    {
        var project = await _projectRepository.GetFirstAsync(tl => tl.Id == id);
        project.IsDeleted = true;
        return new BaseResponseModel
        {
            Id = (await _projectRepository.UpdateAsync(project)).Id
        };
    }

    public async Task<IEnumerable<ProjectResponseModel>> GetAllAsync(ProjectSearchParams searchParams)
    {
        try
        {
            var _projects = await _projectRepository.GetAllAsync(c =>(
                                string.IsNullOrEmpty(searchParams.Filter) ||
                                c.ProjectName.Contains(searchParams.Filter)) && c.IsDeleted == false);

            //var _countries = await _countryRepository.GetAllAsync(c => 1 == 1);

            //var list = from c in _countries
            //           join p in _projects on c.Id equals p.CountryId
            //           select new ProjectResponseModel
            //           {
            //               Id = p.Id,
            //               CountryId = p.CountryId,
            //               ProjectName = p.ProjectName,
            //               CountryName = c.CountryName,
            //               ProjectCode = p.ProjectCode,
            //               Description = p.Description,
            //           };



            // Handle single or multiple CountryId filters
            if (searchParams.CountryId != Guid.Empty && searchParams.CountryId != null)
            {
                var countryId = searchParams.CountryId;
                _projects = _projects.Where(p => p.CountryId == countryId).ToList();
            }
            else if (searchParams.CountryIdList != null && searchParams.CountryIdList.Any())
            {
                var countryGuidList = searchParams.CountryIdList
                    .Where(id => Guid.TryParse(id, out _))
                    .Select(Guid.Parse)
                    .ToList();

                _projects = _projects
                    .Where(p => countryGuidList.Contains(p.CountryId))
                    .ToList();
            }

            var _countries = await _countryRepository.GetAllAsync(c => true);

            var list = from c in _countries
                       join p in _projects on c.Id equals p.CountryId
                       select new ProjectResponseModel
                       {
                           Id = p.Id,
                           CountryId = p.CountryId,
                           ProjectName = p.ProjectName,
                           CountryName = c.CountryName,
                           ProjectCode = p.ProjectCode,
                           Description = p.Description,
                       };


            if (list != null && list.Any())
            {
                int numberOfObjectsPerPage = searchParams.PageSize;
                var queryResult = list
                                .Skip(numberOfObjectsPerPage * (searchParams.PageNumber - 1))
                                .Take(numberOfObjectsPerPage);

                var projects = _mapper.Map<ReadOnlyCollection<ProjectResponseModel>>(queryResult);
                return _mapper.Map<IEnumerable<ProjectResponseModel>>(projects.OrderBy(c => c.ProjectName));
            }
            return Enumerable.Empty<ProjectResponseModel>();
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    public async Task<IEnumerable<ProjectResponseModel>> GetAllByListIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var project = await _projectRepository.GetAllAsync(ti => ti.Id == id);

        return _mapper.Map<IEnumerable<ProjectResponseModel>>(project);
    }

    public async Task<UpdateProjectResponseModel> UpdateAsync(Guid id, UpdateProjectModel projectModel)
    {
        try
        {
            var validationResult = _updateProjectValidator.Validate(projectModel);
            if (!validationResult.IsValid)
            {

                throw new ValidationException("Validation failed", validationResult.Errors);
            }


            var project = await _projectRepository.GetFirstAsync(ti => ti.Id == id);

            _mapper.Map(projectModel, project);

            return new UpdateProjectResponseModel
            {
                Id = (await _projectRepository.UpdateAsync(project)).Id
            };
        }
        catch (ValidationException ex)
        {
            throw ex;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    #endregion
}
