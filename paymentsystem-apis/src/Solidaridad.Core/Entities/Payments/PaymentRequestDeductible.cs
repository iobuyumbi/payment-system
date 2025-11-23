using Solidaridad.Core.Common;

namespace Solidaridad.Core.Entities.Payments;

public class PaymentRequestDeductible : BaseEntity, IAuditedEntity
{
    public string SystemId { get; set; }
    
    public Guid PaymentBatchId { get; set; }
    
    public string BeneficiaryId { get; set; }

    public decimal CarbonUnitsAccured { get; set; }

    public decimal UnitCostEur { get; set; }

    public decimal TotalUnitsEarningEur { get; set; }

    public decimal TotalUnitsEarningLc { get; set; }

    public decimal SolidaridadEarningsShare { get; set; }

    public decimal FarmerEarningsShareEur { get; set; }

    public decimal FarmerEarningsShareLc { get; set; }

    public decimal FarmerPayableEarningsLc { get; set; }

    public decimal FarmerLoansDeductionsLc { get; set; }

    public decimal FarmerLoansBalanceLc { get; set; }

    public decimal AmountToTransfer { get; set; }

    public bool IsPaymentComplete { get; set; }

    public Guid? ExcelImportId { get; set; }

    public int StatusId { get; set; }
    
    public Guid PaymentStatus { get; set; } = new Guid("d8a75d19-0b59-4ba0-95a4-f800e48da2c9");
    
    public string Remarks { get; set; }

    public Guid CreatedBy { get; set; }

    public DateTime CreatedOn { get; set; }

    public Guid? UpdatedBy { get; set; }

    public DateTime? UpdatedOn { get; set; }
    public bool IsManual { get; set; } = false;
    public string LoanAccountNo { get; set; }
}
