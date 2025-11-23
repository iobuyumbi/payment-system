using Solidaridad.Core.Entities.Loans;

namespace Solidaridad.Application.Models.LoanApplication;

public class ImportApplicationModel
{

    public string FullName { get; set; }
    public string NationalId { get; set; }
    public string Mobile { get; set; }
    public string BeneficiaryId { get; set; }

    public string CooperativeName { get; set; }
    public string CountryName { get; set; }

    public string AdminLevel1 { get; set; }

    public string AdminLevel2 { get; set; }

    public string AdminLevel3 { get; set; }

    public string AdminLevel4 { get; set; }

    //Farmer details
    public bool IsRegistered { get; set; }
    public Guid? SolidaridadId { get; set; }
    public string LostReason { get; set; }


    //Seedling & quantities
    public string FarmSize { get; set; }
    public List<Seedling>? Seedling { get; set; }

    // Application summary
    public int TotalSeedlings { get; set; }
    public float TotalSeedlingCost { get; set; }
    public float FacilitationCost { get; set; }
    public float TransportCost { get; set; }
    public float GrandTotal { get; set; }
    // Seedling request witness
    public string WitnessFullName { get; set; }
    public string WitnessNationalId { get; set; }
    public string WitnessPhoneNo { get; set; }
    public string WinessRelation { get; set; }
    public DateTime DateOfWitness { get; set; }

    //Photos of physical form

    public string? FirstPagePath { get; set; }
    public string? SecondPagePath { get; set; }
    public string? ThirdPagePath { get; set; }

    //Enumerator and other details
    public string EnumeratorFullName { get; set; }
    public string? QSFarmer { get; set; }
    public bool? QRSeedling { get; set; }

    public Guid CreatedBy { get; set; }
    public DateTime CreatedOn { get; set; }
    public Guid? UpdatedBy { get; set; }
    public DateTime? UpdatedOn { get; set; }

    public Guid? LoanBatchId { get; set; }
}
