using AutoMapper;
using Solidaridad.Application.Models;
using Solidaridad.Application.Models.LoanItem;
using Solidaridad.Core.Entities.Base;
using Solidaridad.Core.Entities.Loans;
using Solidaridad.DataAccess.Repositories;
using Solidaridad.Shared.Services;
using System.Collections.ObjectModel;

namespace Solidaridad.Application.Services.Impl;

public class MasterLoanItemService : IMasterLoanItemService
{
    #region DI
    private IMapper _mapper;
    private IMasterLoanItemRepository _loanItemRepository;
    private readonly IClaimService _claimService;

    public MasterLoanItemService(IMasterLoanItemRepository loanItemRepository,
        IClaimService claimService,
        IMapper mapper)
    {
        _loanItemRepository = loanItemRepository;
        _claimService = claimService;
        _mapper = mapper;
    }

    #endregion

    #region Methods
    public async Task<CreateMasterLoanItemResponseModel> CreateAsync(CreateMasterLoanItemModel createLoanItemModel)
    {
        try
        {
            var loanItem = _mapper.Map<MasterLoanItem>(createLoanItemModel);

            loanItem.CreatedBy = Guid.Parse(_claimService.GetUserId());
            loanItem.CreatedOn = DateTime.UtcNow;

            var addedItem = await _loanItemRepository.AddAsync(loanItem);

            return new CreateMasterLoanItemResponseModel
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

        var loanItem = await _loanItemRepository.GetFirstAsync(tl => tl.Id == id);
        loanItem.IsDeleted = true;
        return new BaseResponseModel
        {
            Id = (await _loanItemRepository.UpdateAsync(loanItem)).Id
        };
    }

    public async Task<IEnumerable<MasterLoanItemResponseModel>> GetAllAsync(LoanItemSearchParams searchParams)
    {
        var _loanItems = await _loanItemRepository.GetFullAsync(c =>(
            string.IsNullOrEmpty(searchParams.Filter) ||
            c.ItemName.Contains(searchParams.Filter) ||
            c.Description.Contains(searchParams.Filter)) && c.IsDeleted == false
        );

        var loanItem = _mapper.Map<ReadOnlyCollection<MasterLoanItemResponseModel>>(_loanItems);
        return loanItem;

    }

    public async Task<UpdateMasterLoanItemResponseModel> UpdateAsync(Guid id, UpdateMasterLoanItemModel updateLoanItemModel)
    {
        var loanItem = await _loanItemRepository.GetFirstAsync(ti => ti.Id == id);

        loanItem.UpdatedBy = Guid.Parse(_claimService.GetUserId());
        loanItem.UpdatedOn = DateTime.UtcNow;

        _mapper.Map(updateLoanItemModel, loanItem);

        return new UpdateMasterLoanItemResponseModel
        {
            Id = (await _loanItemRepository.UpdateAsync(loanItem)).Id
        };
    }
    #endregion
}
