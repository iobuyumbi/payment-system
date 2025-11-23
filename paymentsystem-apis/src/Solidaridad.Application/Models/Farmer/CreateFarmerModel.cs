namespace Solidaridad.Application.Models.Farmer;

public class CreateFarmerModel
{
    public string FirstName { get; set; }

    public string OtherNames { get; set; }

    public List<Guid> ProjectIds { get; set; }

    public string Mobile { get; set; }

    public string AlternateContactNumber { get; set; }

    public string Email { get; set; }

    public string SystemId { get; set; }

    public string ParticipantId { get; set; }

    public DateTime? EnumerationDate { get; set; }

    public List<SelectItemModel> Cooperative { get; set; }

    public bool HasDisability { get; set; } = false;

    public bool AccessToMobile { get; set; } = true;

    public string PaymentPhoneNumber { get; set; }

    public bool IsFarmerPhoneOwner { get; set; } = true;

    public string PhoneOwnerName { get; set; }

    public string PhoneOwnerNationalId { get; set; }

    public string PhoneOwnerRelationWithFarmer { get; set; }

    public string PhoneOwnerAddress { get; set; }

    public Guid CountryId { get; set; }

    public Guid? AdminLevel1Id { get; set; }

    public Guid? AdminLevel2Id { get; set; }

    public Guid? AdminLevel3Id { get; set; }

    public short Gender { get; set; }

    public string? BeneficiaryId { get; set; }

    public string Village { get; set; }

    public short BirthMonth { get; set; }

    public short BirthYear { get; set; }

    public bool? IsFarmerVerified { get; set; }

    public SelectItemModel? DocumentType { get; set; }

    public string DocumentTypeId { get; set; }
}
public class CreateFarmerResponseModel : BaseResponseModel { }

