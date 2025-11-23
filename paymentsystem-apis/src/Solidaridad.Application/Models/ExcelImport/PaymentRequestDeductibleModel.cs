using Solidaridad.Application.Models.Farmer;

namespace Solidaridad.Application.Models.ExcelImport;

public class PaymentRequestDeductibleModel
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
   
    public Guid Id { get; set; }
    
    public string Remarks { get; set; }
    
    public bool IsPaymentComplete { get; set; }
    
    public FarmerResponseModel Farmer {  get; set; }

    public Guid CreatedBy { get; set; }

    public DateTime CreatedOn { get; set; }

    public Guid? UpdatedBy { get; set; }

    public DateTime? UpdatedOn { get; set; }

    public Guid? PaymentStatus { get; set; }
    public string NationalId { get; set; }
    public bool IsManual { get; set; }
    public string LoanAccountNo { get; set; }
}

public class PaymentRequestDeductibleResponse: BaseResponseModel
{

}
