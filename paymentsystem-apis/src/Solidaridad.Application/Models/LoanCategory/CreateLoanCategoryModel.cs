namespace Solidaridad.Application.Models.LoanCategory;

public class CreateLoanCategoryModel
{
    public string Name { get; set; }
    public string? Description { get; set; } = null;
}
public class CreateLoanCategoryResponseModel : BaseResponseModel
{

}
