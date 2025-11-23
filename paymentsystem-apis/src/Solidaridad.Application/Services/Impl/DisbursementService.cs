using AutoMapper;
using Solidaridad.Application.Models;
using Solidaridad.Application.Models.Associate;
using Solidaridad.Application.Models.Disbursement;
using Solidaridad.Core.Entities.Payments;
using Solidaridad.DataAccess.Repositories;
using System.Collections.ObjectModel;

namespace Solidaridad.Application.Services.Impl;

public class DisbursementService : IDisbursementService
{
    private readonly IDisbursementRepository _disbursementRepository;
    private readonly IMapper _mapper;
    public DisbursementService(IDisbursementRepository disbursementRepository, IMapper mapper) 
    { 
        _disbursementRepository= disbursementRepository;
        _mapper = mapper;

    }    
    public async Task<CreateDisbursementResponseModel> CreateAsync(CreateDisbursementModel createDisbursementModel)
    {
        try
        {
            //var existingAssociate = await _disbursementRepository.GetAllAsync(c => c.FarmerId
            //.Equals(createDisbursementModel.FarmerId));

            //if (existingAssociate.Any())
            //{

            //    throw new InvalidOperationException("A disbursement with the same fa already exists.");
            //}
            var disbursement = _mapper.Map<Disbursement>(createDisbursementModel);
            disbursement.Id = Guid.NewGuid();
            var addedDisbursement = await _disbursementRepository.AddAsync(disbursement);

            return new CreateDisbursementResponseModel
            {
                Id = addedDisbursement.Id
            };
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    public async Task<BaseResponseModel> DeleteAsync(Guid id)
    {
        var disbursement = await _disbursementRepository.GetFirstAsync(tl => tl.Id == id);
        disbursement.IsDeleted = true;

        return new BaseResponseModel
        {
            Id = (await _disbursementRepository.UpdateAsync(disbursement)).Id
        };
    }

    public async Task<ReadOnlyCollection<DisbursementResponseModel>> GetAllAsync()
    {
        var _associate = await _disbursementRepository.GetAllAsync(c =>  c.IsDeleted == false);


        return (ReadOnlyCollection<DisbursementResponseModel>)_mapper.Map<IEnumerable<DisbursementResponseModel>>(_associate);
    }

    public async Task<UpdateDisbursementResponseModel> UpdateAsync(Guid id, UpdateDisbursementModel updateCountryModel)
    {
        var associate = await _disbursementRepository.GetFirstAsync(ti => ti.Id == id);

        _mapper.Map(updateCountryModel, associate);

        return new UpdateDisbursementResponseModel
        {
            Id = (await _disbursementRepository.UpdateAsync(associate)).Id
        };
    }
}
