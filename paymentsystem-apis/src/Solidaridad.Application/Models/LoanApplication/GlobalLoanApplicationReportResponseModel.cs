namespace Solidaridad.Application.Models.LoanApplication;

public class GlobalLoanApplicationReportResponseModel
{
    public string LoanNumber { get; set; }
    public string LoanProduct { get; set; }
    public DateTime? DateOfWitness { get; set; }
    public DateTime CreatedOn { get; set; }
    public string Status { get; set; }
    public decimal InterestAmount { get; set; }
    public decimal PrincipalAmount { get; set; }
    public decimal RemainingBalance { get; set; }

    // Witness details
    public string WitnessName { get; set; }
    public string WitnessPhoneNumber { get; set; }
    public string WitnessNationalId { get; set; }
    public string WitnessRelation { get; set; }

    // Loan extra info
    public decimal FeeApplied { get; set; }
    public string ProjectName { get; set; }
    public decimal InterestRate { get; set; }
    public string InterestType { get; set; }
    public string InterestCalculationType { get; set; }
    public int Tenure { get; set; }

    // Farmer / Beneficiary info
    public string SystemId { get; set; }
    public string FirstName { get; set; }
    public string OtherNames { get; set; }
    public string CountryName { get; set; }
    public string Mobile { get; set; }
    public string PaymentPhoneNumber { get; set; }
    public bool AccessToMobile { get; set; }
    public string AlternateContactNumber { get; set; }
    public string BeneficiaryId { get; set; }
    public int? BirthMonth { get; set; }
    public int? BirthYear { get; set; }
    public string CooperativeName { get; set; }
    public DateTime? EnumerationDate { get; set; }
}
