namespace Solidaridad.Application.Models.Farmer;

public class ImportFarmerModel
{
    public string FirstName { get; set; }

    public string OtherNames { get; set; }

    public string ProjectName { get; set; }

    public DateTime? DateOfBirth { get; set; }

    public string Mobile { get; set; }

    public string AlternateContactNumber { get; set; }

    public string Email { get; set; }

    public string SystemId { get; set; }

    public string ParticipantId { get; set; }

    public DateTime? EnumerationDate { get; set; }

    public string CooperativeName { get; set; }

    public bool HasDisability { get; set; } = false;

    public bool AccessToMobile { get; set; } = true;

    public string PaymentPhoneNumber { get; set; }

    public bool IsFarmerPhoneOwner { get; set; } = true;

    public string PhoneOwnerName { get; set; }

    public string PhoneOwnerNationalId { get; set; }

    public string PhoneOwnerRelationWithFarmer { get; set; }

    public string PhoneOwnerAddress { get; set; }

    public string CountryName { get; set; }

    public string AdminLevel1 { get; set; }

    public string AdminLevel2 { get; set; }

    public string AdminLevel3 { get; set; }

    public string AdminLevel4 { get; set; }

    public short Gender { get; set; }

    public string BeneficiaryId { get; set; }

    public Guid? DocumentTypeId { get; set; }
}

public class ImportFarmerResponseModel : BaseResponseModel { }
