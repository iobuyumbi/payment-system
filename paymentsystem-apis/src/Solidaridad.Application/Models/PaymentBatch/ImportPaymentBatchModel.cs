namespace Solidaridad.Application.Models.PaymentBatch;

public class ImportPaymentBatchModel
{
    //Uwanjani System ID	
    //Beneficiary Farmer Card ID	
    //Carbon Units Acrued	
    //Unit Cost(EUR)	
    //Total Units Earning (EUR)	
    //Total Units Earning (LC)	
    //Solidaridad Earnings Share	
    //Farmer Earnings Share (EUR)	
    //Farmer Earnings Share (LC)	
    //Farmer Payable Earnings (LC)	
    //Farmer Loans Deductions (LC)	
    //Farmer Loans Balance (LC)

    public string SystemId { get; set; }
    
    public string BeneficiaryId { get; set; }
    
    public decimal? CarbonUnitsAccured { get; set; }
                  
    public decimal? UnitCostEur { get; set; }
                  
    public decimal? TotalUnitsEarningEur { get; set; }
                  
    public decimal? TotalUnitsEarningLc { get; set; }
                  
    public decimal? SolidaridadEarningsShare { get; set; }
                  
    public decimal? FarmerEarningsShareEur { get; set; }
                  
    public decimal? FarmerEarningsShareLc { get; set; }
                  
    public decimal? FarmerPayableEarningsLc { get; set; }
                  
    public decimal? FarmerLoansDeductionsLc { get; set; }
                  
    public decimal? FarmerLoansBalanceLc { get; set; }

    //public decimal? CruCount { get; set; }

    //public Guid PaymentBatchId { get; set; }

    //public decimal? PricePerCru { get; set; }

    //public decimal? PurchasePriceSum { get; set; }

    //public decimal? FarmerShareUsd { get; set; }

    //public decimal? ConvertedPurchasePrice { get; set; }

    //public decimal? AdminDeduction { get; set; }

    //public decimal? FarmerShareConverted { get; set; }

    //public decimal? LoanAdjustmentAmount { get; set; }

    //public decimal? NetDisbursementAmount { get; set; }
}
