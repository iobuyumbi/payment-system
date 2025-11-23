namespace Solidaridad.Application.Models.ItemCategory;

public class CreateItemCategoryModel
{
    public string Name { get; set; }
    public string? Description { get; set; }
}
public class CreateItemCategoryResponseModel : BaseResponseModel
{

}
