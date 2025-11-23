using Solidaridad.Core.Common;
using System.ComponentModel.DataAnnotations.Schema;

namespace Solidaridad.Core.Entities;

[Table("Farmers")]
public class Farmer : BaseEntity
{
    public string FirstName { get; set; }

    public string OtherNames { get; set; }

    public DateTime? DateOfBirth { get; set; }

    public string Mobile { get; set; }

    public string? AlternateContactNumber { get; set; }

    public string? Email { get; set; }

    public string SystemId { get; set; }

    public string ParticipantId { get; set; }

    public DateTime? EnumerationDate { get; set; }

    public bool HasDisability { get; set; } = false;

    public bool AccessToMobile { get; set; } = true;

    public string PaymentPhoneNumber { get; set; }

    public bool IsFarmerPhoneOwner { get; set; } = true;

    public string? PhoneOwnerName { get; set; }

    public string? PhoneOwnerNationalId { get; set; }

    public string? PhoneOwnerRelationWithFarmer { get; set; }

    public string? PhoneOwnerAddress { get; set; }

    public Guid CountryId { get; set; }

    public Guid AdminLevel1Id { get; set; }

    public Guid AdminLevel2Id { get; set; }

    public Guid AdminLevel3Id { get; set; }

    public short Gender { get; set; }

    public Guid CreatedBy { get; set; }

    public DateTime CreatedOn { get; set; }

    public Guid? UpdatedBy { get; set; }

    public DateTime? UpdatedOn { get; set; }

    public int? ImportId { get; set; }

    public string? BeneficiaryId { get; set; }

    public virtual Country Country { get; set; }

    public virtual AdminLevel1 AdminLevel1 { get; set; }

    public virtual AdminLevel2 AdminLevel2 { get; set; }

    public virtual AdminLevel3 AdminLevel3 { get; set; }

    public string Village { get; set; }

    public short? BirthMonth { get; set; }

    public short? BirthYear { get; set; }
    
    public bool IsFarmerVerified { get; set; }= false;

    public string Remarks { get; set; }

    public DateTime? MobileLastVerifiedOn { get; set; }

    public Guid? DocumentTypeId { get; set; }

    public DocumentType? DocumentType { get; set; }

    public string Source { get; set; }
}
