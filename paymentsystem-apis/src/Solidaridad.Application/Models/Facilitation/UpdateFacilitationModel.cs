namespace Solidaridad.Application.Models.Facilitation;

public class UpdateFacilitationModel
{
    public string FullName { get; set; }
    public string PhoneNo { get; set; }
    public decimal NetDisbursementAmount { get; set; }
    public string? Comments { get; set; }
}
public class UpdateFacilitationResponseModel : BaseResponseModel { }
