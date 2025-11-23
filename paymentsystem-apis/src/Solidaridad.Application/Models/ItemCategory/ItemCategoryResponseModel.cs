namespace Solidaridad.Application.Models.ItemCategory;

public class ItemCategoryResponseModel : BaseResponseModel
{
    public string Name { get; set; }
    public string? Description { get; set; }
    public int ItemCount { get; set; }
}
