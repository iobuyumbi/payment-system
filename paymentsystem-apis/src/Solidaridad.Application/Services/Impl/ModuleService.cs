using AutoMapper;
using Solidaridad.Application.Models;
using Solidaridad.Application.Models.Farmer;
using Solidaridad.Application.Models.Project;
using Solidaridad.Core.Entities;
using Solidaridad.Core.Entities.Base;
using Solidaridad.DataAccess.Repositories;
using System.Collections.ObjectModel;

namespace Solidaridad.Application.Services.Impl;

public class ModuleService : IModuleService
{
    private readonly IMapper _mapper;
    private readonly IModuleRepository _moduleRepository;
    private readonly ICountryRepository _countryRepository;
    public ModuleService(IMapper mapper, IModuleRepository moduleRepository) 
    {

        _mapper = mapper;   
        _moduleRepository = moduleRepository;
    
    }

    public async Task<CreateModuleResponseModel> CreateAsync(ModuleResponseModel moduleModel)
    {
        try
        {
            var module = _mapper.Map<Module>(moduleModel);
            module.Id = Guid.NewGuid();
          


            var addedModule = await _moduleRepository.AddAsync(module);

            return new CreateModuleResponseModel
            {
                Id = addedModule.Id
            };
        }
        catch (Exception ex)

        {
            return new CreateModuleResponseModel { };
        }
    }

    public async Task<BaseResponseModel> DeleteAsync(Guid id)
    {
        var project = await _moduleRepository.GetFirstAsync(tl => tl.Id == id);

        return new BaseResponseModel
        {
            Id = (await _moduleRepository.DeleteAsync(project)).Id
        };
    }

    public async Task<IEnumerable<ModuleResponseModel>> GetAllAsync(ModuleSearchParams searchParams)
    {
        try
        {
           
                var _modules = await _moduleRepository.GetAllAsync(c =>
               string.IsNullOrEmpty(searchParams.Filter) 
               );
                
                 

              
                    int numberOfObjectsPerPage = searchParams.PageSize;
                    var queryResult = _modules
                      .Skip(numberOfObjectsPerPage * (searchParams.PageNumber - 1))
              .Take(numberOfObjectsPerPage);

                    var projects = _mapper.Map<ReadOnlyCollection<ProjectResponseModel>>(queryResult);

                 
               
            
            return _mapper.Map<IEnumerable<ModuleResponseModel>>(projects);
        }
        catch (Exception ex)
        {
            return Enumerable.Empty<ModuleResponseModel>();
        }
    }

    public async Task<IEnumerable<ModuleResponseModel>> GetAllByListIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var project = await _moduleRepository.GetAllAsync(ti => ti.Id == id);

        return _mapper.Map<IEnumerable<ModuleResponseModel>>(project);
    }

    public async Task<UpdateModuleResponseModel> UpdateAsync(Guid id, ModuleResponseModel projectModel)
    {

        var project = await _moduleRepository.GetFirstAsync(ti => ti.Id == id);

        _mapper.Map(projectModel, project);

        return new UpdateModuleResponseModel
        {
            Id = (await _moduleRepository.UpdateAsync(project)).Id
        };
    }
}
