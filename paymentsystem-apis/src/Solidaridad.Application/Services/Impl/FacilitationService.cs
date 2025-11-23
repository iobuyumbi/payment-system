using AutoMapper;
using Solidaridad.Application.Models;
using Solidaridad.Application.Models.Facilitation;
using Solidaridad.Core.Entities.Base;
using Solidaridad.Core.Entities.Payments;
using Solidaridad.DataAccess.Repositories;

namespace Solidaridad.Application.Services.Impl;

public class FacilitationService : IFacilitationService
{
    private readonly IFacilitationRepository _facilitationRepository;
    private readonly IMapper _mapper;

    public FacilitationService(
        IFacilitationRepository facilitationRepository, IMapper mapper)
    {
        _facilitationRepository = facilitationRepository;
        _mapper = mapper;
    }

    public async Task<CreateFacilitationResponseModel> CreateAsync(CreateFacilitationModel createModel)
    {
        try
        {
            var facilitation = _mapper.Map<PaymentRequestFacilitation>(createModel);
            facilitation.Id = Guid.NewGuid();
            facilitation.StatusId = 0;

            var addedFacilitation = await _facilitationRepository.AddAsync(facilitation);

            return new CreateFacilitationResponseModel
            {
                Id = addedFacilitation.Id
            };
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    public async Task<BaseResponseModel> DeleteAsync(Guid id)
    {
        var facilitation = await _facilitationRepository.GetFirstAsync(tl => tl.Id == id);

        return new BaseResponseModel
        {
            Id = (await _facilitationRepository.DeleteAsync(facilitation)).Id
        };
    }

    public async Task<IEnumerable<FacilitationResponseModel>> GetAllAsync(FacilitationSearchParams searchParams)
    {
        var facilitations = await _facilitationRepository.GetAllAsync(c => c.PaymentBatchId == Guid.Parse(searchParams.BatchId) && c.IsDeleted == false);
        return _mapper.Map<IEnumerable<FacilitationResponseModel>>(facilitations);
    }

    public async Task<UpdateFacilitationResponseModel> UpdateAsync(Guid id, UpdateFacilitationModel updateModel)
    {
        var facilitation = await _facilitationRepository.GetFirstAsync(ti => ti.Id == id);

        _mapper.Map(updateModel, facilitation);

        return new UpdateFacilitationResponseModel
        {
            Id = (await _facilitationRepository.UpdateAsync(facilitation)).Id
        };
    }
}
