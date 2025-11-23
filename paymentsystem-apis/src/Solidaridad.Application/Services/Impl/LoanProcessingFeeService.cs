using AutoMapper;
using Solidaridad.Application.Models;
using Solidaridad.Application.Models.LoanProcessingFee;
using Solidaridad.Core.Entities;
using Solidaridad.DataAccess.Repositories;
using System.Collections.ObjectModel;

namespace Solidaridad.Application.Services.Impl;

public class LoanProcessingFeeService : ILoanProcessingFeeService
{
    #region DI
    private IMapper _mapper;
    private ILoanProcessingFeeRepository _loanProcessingFeeRepository;
    public LoanProcessingFeeService(ILoanProcessingFeeRepository loanProcessingFeeRepository, IMapper mapper)
    {
        _loanProcessingFeeRepository = loanProcessingFeeRepository;
        _mapper = mapper;
    }
    #endregion

    #region Methods
    public async Task<CreateLoanProcessingFeeResponseModel> CreateAsync(CreateLoanProcessingFeeModel loanProcessingFeeModel)
    {
        try
        {
            var loanProcessingFee = _mapper.Map<MasterLoanTermAdditionalFee>(loanProcessingFeeModel);
            var addedItem = await _loanProcessingFeeRepository.AddAsync(loanProcessingFee);

            return new CreateLoanProcessingFeeResponseModel
            {
                Id = addedItem.Id
            };
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    public async Task<BaseResponseModel> DeleteAsync(Guid id)
    {

        var loanProcessingFee = await _loanProcessingFeeRepository.GetFirstAsync(tl => tl.Id == id);
        loanProcessingFee.IsDeleted = true;
        return new BaseResponseModel
        {
            Id = (await _loanProcessingFeeRepository.UpdateAsync(loanProcessingFee)).Id
        };
    }

    public async Task<IEnumerable<LoanProcessingFeeResponseModel>> GetAllAsync()
    {
        var _loanProcessingFees = await _loanProcessingFeeRepository.GetAllAsync(c =>  c.IsDeleted == false);

        var loanProcessingFee = _mapper.Map<ReadOnlyCollection<LoanProcessingFeeResponseModel>>(_loanProcessingFees);
        return loanProcessingFee;

    }

    public async Task<UpdateLoanProcessingFeeResponseModel> UpdateAsync(Guid id, UpdateLoanProcessingFeeModel updateLoanProcessingFeeModel)
    {
        var loanProcessingFee = await _loanProcessingFeeRepository.GetFirstAsync(ti => ti.Id == id);

        _mapper.Map(updateLoanProcessingFeeModel, loanProcessingFee);

        return new UpdateLoanProcessingFeeResponseModel
        {
            Id = (await _loanProcessingFeeRepository.UpdateAsync(loanProcessingFee)).Id
        };
    } 
    #endregion

}
