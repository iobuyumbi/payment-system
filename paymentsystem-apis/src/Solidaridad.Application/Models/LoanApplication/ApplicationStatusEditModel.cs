namespace Solidaridad.Application.Models.LoanApplication;

public class ApplicationStatusEditModel
{
    public Guid Id { get; set; }
    public Guid Status { get; set; }
    public string Comments { get; set; }

}
public class ApplicationStatusEditResponseModel : BaseResponseModel { }
