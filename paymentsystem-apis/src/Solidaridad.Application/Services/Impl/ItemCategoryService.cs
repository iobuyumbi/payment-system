using AutoMapper;
using Solidaridad.Application.Models;
using Solidaridad.Application.Models.ItemCategory;
using Solidaridad.Application.Models.LoanItem;
using Solidaridad.Core.Entities.Base;
using Solidaridad.Core.Entities.Loans;
using Solidaridad.DataAccess.Migrations;
using Solidaridad.DataAccess.Repositories;
using Solidaridad.DataAccess.Repositories.Impl;
using System.Collections.ObjectModel;

namespace Solidaridad.Application.Services.Impl;

public class ItemCategoryService : IItemCategoryService
{
    private readonly IItemCategoryRepository _itemCategoryRepository;
    private readonly IMapper _mapper;
    private readonly IMasterLoanItemRepository _loanItemRepository;
    public ItemCategoryService (IItemCategoryRepository itemCategoryRepository, IMapper mapper, IMasterLoanItemRepository loanItemRepository)
    {
        _itemCategoryRepository = itemCategoryRepository;
            _mapper = mapper;
        _loanItemRepository = loanItemRepository;
    }
    public async Task<CreateItemCategoryResponseModel> CreateAsync(CreateItemCategoryModel categoryModel)
    {
        try
        {
            var itemCategory = _mapper.Map<ItemCategory>(categoryModel);
            var addedItem = await _itemCategoryRepository.AddAsync(itemCategory);

            return new CreateItemCategoryResponseModel
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
        var itemCategory = await _itemCategoryRepository.GetFirstAsync(tl => tl.Id == id);

        return new BaseResponseModel
        {
            Id = (await _itemCategoryRepository.DeleteAsync(itemCategory)).Id
        };
    }

    public async Task<IEnumerable<ItemCategoryResponseModel>> GetAllAsync(ItemCategorySearchParams searchParams)
    {
        var _itemCategory = await _itemCategoryRepository.GetAllAsync(c =>
             string.IsNullOrEmpty(searchParams.Filter) ||
             c.Name.Contains(searchParams.Filter) ||
             c.Description.Contains(searchParams.Filter) && c.IsDeleted == false
        );

        var item = await _loanItemRepository.GetAllAsync(c => c.IsDeleted == false);
        var itemCategory = _mapper.Map<ReadOnlyCollection<ItemCategoryResponseModel>>(_itemCategory);

        // Replace ForEach with a foreach loop to fix the CS1061 error
        foreach (var f in itemCategory)
        {
            f.ItemCount = item.Count(c => c.CategoryId == f.Id);
        }
        return itemCategory;
    }

    public async Task<UpdateItemCategoryResponseModel> UpdateAsync(Guid id, UpdateItemCategoryModel updateItemCategoryModel)
    {
        var itemCategory = await _itemCategoryRepository.GetFirstAsync(ti => ti.Id == id);

        _mapper.Map(updateItemCategoryModel, itemCategory);

        return new UpdateItemCategoryResponseModel
        {
            Id = (await _itemCategoryRepository.UpdateAsync(itemCategory)).Id
        };
    }
}
