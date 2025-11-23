namespace Solidaridad.Application.Models.PaymentDeductible;

public class UpdatePaymentDeductibleModel
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

    public Guid? ExcelImportId { get; set; }

    public int StatusId { get; set; }
    public string Remarks { get; set; }
    public Guid? PaymentStatus { get; set; }
}
public class UpdatePaymentDeductibleResponseModel : BaseResponseModel { }
