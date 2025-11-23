using AutoMapper;
using NPOI.SS.Formula.Functions;
using Solidaridad.Application.Models;
using Solidaridad.Application.Models.ApplicationStatus;
using Solidaridad.Application.Models.Country;
using Solidaridad.Application.Models.County;
using Solidaridad.Core.Entities;
using Solidaridad.Core.Entities.Base;
using Solidaridad.Core.Entities.Loans;
using Solidaridad.DataAccess.Repositories;
using Solidaridad.DataAccess.Repositories.Impl;
using System.Collections.ObjectModel;

namespace Solidaridad.Application.Services.Impl;

public class ApplicationStatusService : IApplicationStatusService
{
    #region DI
    private readonly IMapper _mapper;
    private readonly IApplicationStatusRepository _statusRepository;
    
    

    public ApplicationStatusService(IMapper mapper,
         
         IApplicationStatusRepository statusRepository)
    {
        _mapper = mapper;
        _statusRepository = statusRepository;
       
    }
    #endregion
    public  async Task<CreateApplcationStatusResponseModel> CreateAsync(CreateApplicationStatusModel createApplicationModel)
    {
        try
        {
            var status = _mapper.Map<ApplicationStatus>(createApplicationModel);
            var addedStatus= await _statusRepository.AddAsync(status);

            return new CreateApplcationStatusResponseModel
            {
                Id = addedStatus.Id
            };
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    public async Task<BaseResponseModel> DeleteAsync(Guid id)
    {
        var status = await _statusRepository.GetFirstAsync(tl => tl.Id == id);
        status.IsDeleted = true;

        return new BaseResponseModel
        {
            Id = (await _statusRepository.UpdateAsync(status)).Id
        };
    }

    public async Task<ReadOnlyCollection<ApplicationStatusResponseModel>> GetAllAsync(ApplicationStatusSearchParams searchParams)
    {
        var _status = await _statusRepository.GetAllAsync(c => c.IsDeleted == false);
        var status = _mapper.Map<ReadOnlyCollection<ApplicationStatusResponseModel>>(_status);
        return status;

    }

    public async Task<UpdateApplicationStatusResponseModel> UpdateAsync(Guid id, UpdateApplicationStatusModel updateApplicationStatusModel)
    {
        var _status = await _statusRepository.GetAllAsync(ti => ti.Id == id);
        var status = _status.FirstOrDefault();
        _mapper.Map(updateApplicationStatusModel, status);

        return new UpdateApplicationStatusResponseModel
        {
            Id = (await _statusRepository.UpdateAsync(status)).Id
        };
    }
}
