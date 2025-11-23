namespace Solidaridad.Application.Models.ExcelImport;

public class PaymentRequestFacilitationModel : BaseResponseModel
{
    public string FullName { get; set; }
    public string PhoneNo { get; set; }
    public decimal NetDisbursementAmount { get; set; }
    public int StatusId { get; set; }
    public Guid PaymentStatus { get; set; }
    public string Remarks { get; set; }
    public string Comments { get; set; }
    public bool IsPaymentComplete { get; set; }
    public string NationalId { get; set; }
}
