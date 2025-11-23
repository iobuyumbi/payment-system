using Solidaridad.Core.Common;

namespace Solidaridad.Core.Entities.Payments;

public class PaymentRequestFacilitation : BaseEntity
{
    public string FullName { get; set; }

    public string PhoneNo { get; set; }

    public decimal? NetDisbursementAmount { get; set; }

    public Guid PaymentBatchId { get; set; }

    public string Comments { get; set; }

    public int StatusId { get; set; }

    public string Remarks { get; set; } = string.Empty;

    public Guid PaymentStatus { get; set; } = new Guid("d8a75d19-0b59-4ba0-95a4-f800e48da2c9");

    public bool IsPaymentComplete { get; set; }

    public string NationalId { get; set; }
}
