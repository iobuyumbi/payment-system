using AutoMapper;
using Solidaridad.Application.Models;
using Solidaridad.Application.Models.ItemCategory;
using Solidaridad.Application.Models.LoanCategory;
using Solidaridad.Core.Entities.Base;
using Solidaridad.Core.Entities.Loans;
using Solidaridad.DataAccess.Repositories;
using Solidaridad.DataAccess.Repositories.Impl;
using System.Collections.ObjectModel;

namespace Solidaridad.Application.Services.Impl;

public class LoanCategoryService : ILoanCategoryService
{
    private readonly ILoanCategoryRepository _loanCategoryRepository;
    private readonly IMapper _mapper;
   public LoanCategoryService(ILoanCategoryRepository loanCategoryRepository, IMapper mapper)
    { 
    _loanCategoryRepository = loanCategoryRepository;
        _mapper = mapper;
    }
    public async Task<CreateLoanCategoryResponseModel> CreateAsync(CreateLoanCategoryModel loanCategoryModel)
    {
        try
        {
            var itemCategory = _mapper.Map<LoanCategory>(loanCategoryModel);
            var addedItem = await _loanCategoryRepository.AddAsync(itemCategory);

            return new CreateLoanCategoryResponseModel
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
        var itemCategory = await _loanCategoryRepository.GetFirstAsync(tl => tl.Id == id);
        itemCategory.IsDeleted = true;

        return new BaseResponseModel
        {
            Id = (await _loanCategoryRepository.UpdateAsync(itemCategory)).Id
        };
    }

    public async Task<IEnumerable<LoanCategoryResponseModel>> GetAllAsync(LoanCategorySearchParams searchParams)
    {
        var _loanCategory = await _loanCategoryRepository.GetAllAsync(c =>
             string.IsNullOrEmpty(searchParams.Filter) ||
             c.Name.Contains(searchParams.Filter) ||
             c.Description.Contains(searchParams.Filter) && c.IsDeleted == false
         );


        var loanCategory = _mapper.Map<ReadOnlyCollection<LoanCategoryResponseModel>>(_loanCategory);
        return loanCategory;
    }

    public async Task<UpdateLoanCategoryResponseModel> UpdateAsync(Guid id, UpdateLoanCategoryModel loanCategoryModel)
    {
        var loanCategory = await _loanCategoryRepository.GetFirstAsync(ti => ti.Id == id);

        _mapper.Map(loanCategoryModel, loanCategory);

        return new UpdateLoanCategoryResponseModel
        {
            Id = (await _loanCategoryRepository.UpdateAsync(loanCategory)).Id
        };
    }
}
