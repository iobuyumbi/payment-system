using Solidaridad.Application.Models;
using Solidaridad.Application.Models.ItemCategory;
using Solidaridad.Core.Entities.Base;

namespace Solidaridad.Application.Services;

public interface IItemCategoryService
{
    Task<CreateItemCategoryResponseModel> CreateAsync(CreateItemCategoryModel project);

    Task<BaseResponseModel> DeleteAsync(Guid id);

    Task<IEnumerable<ItemCategoryResponseModel>> GetAllAsync(ItemCategorySearchParams searchParams);

    Task<UpdateItemCategoryResponseModel> UpdateAsync(Guid id, UpdateItemCategoryModel projectModel);
}
