using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Solidaridad.Application.Models;
using Solidaridad.Application.Models.ItemCategory;
using Solidaridad.Application.Services;
using Solidaridad.Core.Entities.Base;
using Solidaridad.Core.Entities.Pagination;

namespace Solidaridad.API.Controllers;

[Authorize]
public class ItemCategoryController : ApiController
{
    #region DI
    private readonly IItemCategoryService _itemCategoryService;
    private readonly IMapper _mapper;
    public ItemCategoryController(IItemCategoryService itemCategoryService, IMapper mapper)
    {
        _itemCategoryService = itemCategoryService;
        _mapper = mapper;
    }
    #endregion

    #region Methods
    [HttpPost]
    [Route("Search")]
    public async Task<IActionResult> Search(ItemCategorySearchParams itemCateogrySearchParams)
    {
        var itemsCategory = await _itemCategoryService.GetAllAsync(itemCateogrySearchParams);

        int totalRecords = itemsCategory.Count();
        Page pageInfo = new Page
        {
            PageNumber = itemCateogrySearchParams.PageNumber,
            Size = itemCateogrySearchParams.PageSize,
            TotalElements = totalRecords,
            TotalPages = totalRecords / itemCateogrySearchParams.PageSize
        };
        var pagedData = new PagedData<List<ItemCategoryResponseModel>>
        {
            Page = pageInfo,
            Result = itemsCategory.OrderBy(c => c.Name).ToList()
        };

        return Ok(new ApiResponseModel<PagedData<List<ItemCategoryResponseModel>>>
        {
            Success = true,
            Message = "success",
            Data = pagedData
        });
    }

    [HttpPost]
    public async Task<IActionResult> Add(CreateItemCategoryModel createItemCategoryModel)
    {
        return Ok(ApiResult<CreateItemCategoryResponseModel>.Success(
            await _itemCategoryService.CreateAsync(createItemCategoryModel)));
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> UpdateAsync(Guid id, UpdateItemCategoryModel updateItemCategoryModel)
    {
        return Ok(ApiResult<UpdateItemCategoryResponseModel>.Success(
            await _itemCategoryService.UpdateAsync(id, updateItemCategoryModel)));
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteAsync(Guid id)
    {
        return Ok(ApiResult<BaseResponseModel>.Success(await _itemCategoryService.DeleteAsync(id)));
    }
    #endregion
}
