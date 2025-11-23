using AutoMapper;
using Solidaridad.Application.Models;
using Solidaridad.Application.Models.LoanTerm;
using Solidaridad.Core.Entities;
using Solidaridad.Core.Entities.Base;
using Solidaridad.Core.Entities.Loans;
using Solidaridad.DataAccess.Repositories;

namespace Solidaridad.Application.Services.Impl;

public class MasterLoanTermService : IMasterLoanTermService

{
    #region DI
    private IMapper _mapper;
    private IMasterLoanTermRepository _loanTermRepository;
    private IMasterLoanTermsMappingRepository _masterLoanTermsMappingRepository;
    private readonly ILoanProcessingFeeRepository _loanProcessingFeeRepository;
    public MasterLoanTermService(IMasterLoanTermRepository loanTermRepository, IMapper mapper, IMasterLoanTermsMappingRepository masterLoanTermsMappingRepository, ILoanProcessingFeeRepository loanProcessingFeeRepository)
    {
        _loanTermRepository = loanTermRepository;
        _mapper = mapper;
        _masterLoanTermsMappingRepository = masterLoanTermsMappingRepository;
        _loanProcessingFeeRepository = loanProcessingFeeRepository;
    }

    #endregion

    #region Methods
    public async Task<CreateMasterLoanTermResponseModel> CreateAsync(CreateMasterLoanTermModel createLoanTermModel)
    {
        try
        {
            var loanTerm = _mapper.Map<MasterLoanTerm>(createLoanTermModel);
            var addedTerm = await _loanTermRepository.AddAsync(loanTerm);
            if (addedTerm.Id != Guid.Empty)
            {
                var list = new List<MasterLoanTermAdditionalFeeMapping>();

                if (createLoanTermModel.AdditionalFee != null)
                {
                    foreach (var item in createLoanTermModel.AdditionalFee)
                    {
                        list.Add(new MasterLoanTermAdditionalFeeMapping
                        {
                            Id = new Guid(),
                            LoanTermId = addedTerm.Id,
                            AdditionalFeeId = item.Id,
                            IsDeleted = false,
                        });
                    }
                    await _masterLoanTermsMappingRepository.AddRange(list);
                }
            }
            return new CreateMasterLoanTermResponseModel
            {
                Id = addedTerm.Id
            };
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    public async Task<BaseResponseModel> DeleteAsync(Guid id)
    {

        var loanTerm = await _loanTermRepository.GetFirstAsync(tl => tl.Id == id);

        return new BaseResponseModel
        {
            Id = (await _loanTermRepository.DeleteAsync(loanTerm)).Id
        };
    }

    public async Task<IEnumerable<MasterLoanTermResponseModel>> GetAllAsync(SearchParams searchParams)
    {
        var loanTerms = await _loanTermRepository.GetAllAsync(c => true && c.CountryId == searchParams.CountryId);
        var loanTermResponses = _mapper.Map<List<MasterLoanTermResponseModel>>(loanTerms);

        var allMappings = await _masterLoanTermsMappingRepository.GetAllAsync(m => loanTerms.Select(t => t.Id).Contains(m.LoanTermId));

        var additionalFeeIds = allMappings.Select(m => m.AdditionalFeeId).Distinct();
        var allAdditionalFees = await _loanProcessingFeeRepository.GetAllAsync(f => additionalFeeIds.Contains(f.Id));

        foreach (var loanTermResponse in loanTermResponses)
        {

            var loanTermMappings = allMappings.Where(m => m.LoanTermId == loanTermResponse.Id);

            foreach (var mapping in loanTermMappings)
            {

                var fee = allAdditionalFees.FirstOrDefault(f => f.Id == mapping.AdditionalFeeId);

                if (fee != null)
                {
                    var mappingResponse = new MasterLoanTermAdditionalFee
                    {
                        Id = fee.Id,
                        FeeName = fee.FeeName,
                        FeeType = fee.FeeType,
                        Value = fee.Value,
                    };

                    loanTermResponse.AdditionalFee.Add(mappingResponse);
                }
            }
        }

        return loanTermResponses;
    }

    public async Task<MasterLoanTermResponseModel> GetByIdAsync(Guid id, Guid countryId)
    {
        var masterTerm = await _loanTermRepository.GetAllAsync(ti => ti.Id == id && ti.CountryId == countryId);

        if (masterTerm == null)
        {
            return null;
        }

        var mappings = await _masterLoanTermsMappingRepository.GetAllAsync(ti => ti.LoanTermId == id);
        var masterTermResponse = _mapper.Map<MasterLoanTermResponseModel>(masterTerm[0]);

        foreach (var mapping in mappings)
        {
            var additionalFees = await _loanProcessingFeeRepository.GetAllAsync(f => f.Id == mapping.AdditionalFeeId);
            if (additionalFees != null)
            {
                var fee = additionalFees.FirstOrDefault();
                var mappingResponse = new MasterLoanTermAdditionalFee
                {
                    Id = fee.Id,
                    FeeName = fee.FeeName,
                    FeeType = fee.FeeType,
                    Value = fee.Value,
                };
                masterTermResponse.AdditionalFee.Add(mappingResponse);
            }
        }

        return masterTermResponse;
    }

    public async Task<UpdateMasterLoanTermResponseModel> UpdateAsync(Guid id, UpdateMasterLoanTermModel updateLoanTermModel)
    {
        var loanTerm = await _loanTermRepository.GetFirstAsync(ti => ti.Id == id);

        _mapper.Map(updateLoanTermModel, loanTerm);
        var existingFeeMaps = _masterLoanTermsMappingRepository.GetAllAsync(c => c.LoanTermId == id).Result;

        foreach (var item in existingFeeMaps)
        {
            await _masterLoanTermsMappingRepository.DeleteAsync(item);
        }

        var list = new List<MasterLoanTermAdditionalFeeMapping>();
        if (updateLoanTermModel.AdditionalFee != null)
        {
            foreach (var item in updateLoanTermModel.AdditionalFee)
            {
                list.Add(new MasterLoanTermAdditionalFeeMapping
                {
                    Id = new Guid(),
                    LoanTermId = id,
                    AdditionalFeeId = item.Id,
                    IsDeleted = false,
                });
            }
        }
            await _masterLoanTermsMappingRepository.AddRange(list);
        
        return new UpdateMasterLoanTermResponseModel
        {
            Id = (await _loanTermRepository.UpdateAsync(loanTerm)).Id
        };
    }
    #endregion
}
