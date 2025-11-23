namespace Solidaridad.Application.Models.LoanCategory;

public class LoanCategoryResponseModel : BaseResponseModel
{
    public string Name { get; set; }
    public string? Description { get; set; }
    public int ItemCount { get; set; }
}
