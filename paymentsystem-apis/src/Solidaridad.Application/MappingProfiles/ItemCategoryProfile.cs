using AutoMapper;
using Solidaridad.Application.Models.ItemCategory;
using Solidaridad.Core.Entities.Loans;

namespace Solidaridad.Application.MappingProfiles;

public class ItemCategoryProfile : Profile
{
    public ItemCategoryProfile() 
    {
        CreateMap<CreateItemCategoryModel, ItemCategory>();
        
        CreateMap<ItemCategory, ItemCategoryResponseModel>();
        
        CreateMap<ItemCategory , CreateItemCategoryModel>();
        
        CreateMap<UpdateItemCategoryModel, ItemCategory>();
        
        CreateMap<ItemCategoryResponseModel, ItemCategory>();
    }
}
