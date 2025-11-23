using Solidaridad.Application.Models.Country;
using Solidaridad.Application.Models.County;
using Solidaridad.Application.Models.Project;
using Solidaridad.Application.Models.SubCounty;
using Solidaridad.Application.Models.Ward;

namespace Solidaridad.Application.Models.Farmer;

public class FarmerResponseModel : BaseResponseModel
{
    public string FirstName { get; set; }

    public string OtherNames { get; set; }

    public short Gender { get; set; }

    public DateTime? DateOfBirth { get; set; }

    public string Mobile { get; set; }

    public string AlternateContactNumber { get; set; }

    public string Email { get; set; }

    public string SystemId { get; set; }

    public string ParticipantId { get; set; }

    public DateTime? EnumerationDate { get; set; }

    public string CooperativeName { get; set; }

    public IEnumerable<SelectItemModel> Cooperative { get; set; }

    public bool HasDisability { get; set; } = false;

    public bool AccessToMobile { get; set; } = false;

    public string PaymentPhoneNumber { get; set; }

    public bool IsFarmerPhoneOwner { get; set; } = false;

    public string PhoneOwnerName { get; set; }

    public string PhoneOwnerNationalId { get; set; }

    public string PhoneOwnerRelationWithFarmer { get; set; }

    public string PhoneOwnerAddress { get; set; }

    public AdminLevel1ResponseModel AdminLevel1 { get; set; }

    public AdminLevel2ResponseModel AdminLevel2 { get; set; }

    public AdminLevel3ResponseModel AdminLevel3 { get; set; }

    public string BeneficiaryId { get; set; }

    public CountryResponseModel Country { get; set; }

    public ProjectResponseModel Project { get; set; }

    public string Village { get; set; }

    public short BirthMonth { get; set; }

    public short BirthYear { get; set; }

    public DateTime CreatedOn { get; internal set; }

    public bool IsFarmerVerified { get; set; }

    public string FullName
    {
        get { return $"{FirstName} {OtherNames}"; }
    }

    public string LoanBatchName { get; set; }

    public DateTime? UpdatedOn { get; set; }

    public DateTime? MobileLastVerifiedOn { get; set; }

    public decimal WalletBalance { get; set; }

    public SelectItemModel DocumentType { get; set; }
    
    public string ValidationSource { get; set; }
    
    public Guid DocumentTypeId { get; set; }
}
