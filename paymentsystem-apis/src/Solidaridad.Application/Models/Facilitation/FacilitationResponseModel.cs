namespace Solidaridad.Application.Models.Facilitation;

public class FacilitationResponseModel : BaseResponseModel
{
    public string FullName { get; set; }
    public string PhoneNo { get; set; }
    public decimal NetDisbursementAmount { get; set; }
    public string? Comments { get; set; }
    public bool IsPaymentComplete { get; set; }
    public string Remarks { get; set; }
    public string NationalId { get; set; }
}
