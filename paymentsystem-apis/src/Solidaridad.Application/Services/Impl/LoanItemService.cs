using AutoMapper;
using Solidaridad.Application.Models;
using Solidaridad.Application.Models.LoanItem;
using Solidaridad.Core.Entities.Base;
using Solidaridad.Core.Entities.Loans;
using Solidaridad.DataAccess.Repositories;
using System.Collections.ObjectModel;

namespace Solidaridad.Application.Services.Impl;

public class LoanItemService : ILoanItemService
{
    private IMapper _mapper;
    private ILoanItemRepository _loanItemRepository;
    public LoanItemService(ILoanItemRepository loanItemRepository, IMapper mapper)
    {
        _loanItemRepository = loanItemRepository;
        _mapper = mapper;
    }

    public async Task<CreateLoanItemResponseModel> CreateAsync(CreateLoanItemModel loanItemModel)
    {
        try
       {
            var loanItem = _mapper.Map<LoanItem>(loanItemModel);
            var addedItem = await _loanItemRepository.AddAsync(loanItem);

                return new CreateLoanItemResponseModel
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

    public async Task<IEnumerable<LoanItemResponseModel>> GetAllAsync(LoanItemSearchParams searchParams)
    {
        var _loanItems = await _loanItemRepository.GetAllAsync(c =>
            string.IsNullOrEmpty(searchParams.Filter) ||
            c.ItemName.Contains(searchParams.Filter) ||
            c.Description.Contains(searchParams.Filter) && c.IsDeleted == false
        );


        var loanItem = _mapper.Map<ReadOnlyCollection<LoanItemResponseModel>>(_loanItems);
        return loanItem;

    }

    public async Task<UpdateLoanItemResponseModel> UpdateAsync(Guid id, UpdateLoanItemModel updateLoanItemModel)
    {
        var loanItem = await _loanItemRepository.GetFirstAsync(ti => ti.Id == id);

        _mapper.Map(updateLoanItemModel, loanItem);

        return new UpdateLoanItemResponseModel
        {
            Id = (await _loanItemRepository.UpdateAsync(loanItem)).Id
        };
    }

  

}
