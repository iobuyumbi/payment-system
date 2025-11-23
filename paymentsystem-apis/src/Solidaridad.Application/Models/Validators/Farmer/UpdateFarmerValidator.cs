using FluentValidation;
using Solidaridad.Application.Models.Farmer;

namespace Solidaridad.Application.Models.Validators.Farmer;

public class UpdateFarmerValidator : AbstractValidator<UpdateFarmerModel>
{
    public UpdateFarmerValidator()
    {
        RuleFor(farmer => farmer.FirstName)
            .NotEmpty().WithMessage("First name must be provided")
            .MinimumLength(2).WithMessage("First name must contain a minimum of 2 characters")
            .MaximumLength(50).WithMessage("First name must contain a maximum of 50 characters");

        RuleFor(farmer => farmer.OtherNames)
            .NotEmpty().WithMessage("Other names must be provided");

        RuleFor(farmer => farmer.Mobile)
            .NotEmpty().WithMessage("Mobile number must be provided")
            .Matches(@"^\+?\d+$").WithMessage("Mobile number must be valid");

        RuleFor(farmer => farmer.AlternateContactNumber)
            .Matches(@"^\+?\d+$").When(farmer => !string.IsNullOrEmpty(farmer.AlternateContactNumber))
            .WithMessage("Alternate contact number must be valid");

        RuleFor(farmer => farmer.Email)
         .EmailAddress().WithMessage("Email must be valid")
         .When(farmer => !string.IsNullOrWhiteSpace(farmer.Email));

        RuleFor(farmer => farmer.SystemId)
            .NotEmpty().WithMessage("System ID must be provided");

        RuleFor(farmer => farmer.ParticipantId)
            .NotEmpty().WithMessage("Participant ID must be provided");

        //RuleFor(farmer => farmer.EnumerationDate)
        //    .NotNull().WithMessage("Enumeration date must be provided");

        //RuleFor(farmer => farmer.Cooperative)
        //    .NotNull().WithMessage("Cooperative must be provided")
        //    .Must(coop => coop.Any()).WithMessage("At least one cooperative must be selected");

        RuleFor(farmer => farmer.PaymentPhoneNumber)
            .NotEmpty().WithMessage("Payment phone number must be provided");

        RuleFor(farmer => farmer.PhoneOwnerName)
            .NotEmpty().When(farmer => !farmer.IsFarmerPhoneOwner)
            .WithMessage("Phone owner's name must be provided if the farmer is not the phone owner");

        RuleFor(farmer => farmer.PhoneOwnerNationalId)
            .NotEmpty().When(farmer => !farmer.IsFarmerPhoneOwner)
            .WithMessage("Phone owner's national ID must be provided if the farmer is not the phone owner");

        RuleFor(farmer => farmer.PhoneOwnerRelationWithFarmer)
            .NotEmpty().When(farmer => !farmer.IsFarmerPhoneOwner)
            .WithMessage("Relation with phone owner must be provided if the farmer is not the phone owner");

        RuleFor(farmer => farmer.PhoneOwnerAddress)
            .NotEmpty().When(farmer => !farmer.IsFarmerPhoneOwner)
            .WithMessage("Phone owner's address must be provided if the farmer is not the phone owner");

        RuleFor(farmer => farmer.CountryId)
            .NotEmpty().WithMessage("Country ID must be provided");

        RuleFor(farmer => farmer.AdminLevel1Id)
            .NotEmpty().WithMessage("Admin Level 1 ID must be provided");

        RuleFor(farmer => farmer.AdminLevel2Id)
            .NotEmpty().When(farmer => farmer.AdminLevel1Id != Guid.Empty)
            .WithMessage("Admin Level 2 ID must be provided when Admin Level 1 ID is specified");

        RuleFor(farmer => farmer.AdminLevel3Id)
            .NotEmpty().When(farmer => farmer.AdminLevel2Id != Guid.Empty)
            .WithMessage("Admin Level 3 ID must be provided when Admin Level 2 ID is specified");

        RuleFor(farmer => farmer.Gender)
            .InclusiveBetween((short)0, (short)2).WithMessage("Gender must be valid");

        //RuleFor(farmer => farmer.BeneficiaryId)
        //    .NotEmpty().WithMessage("Beneficiary ID must be provided");

        RuleFor(farmer => farmer.Village)
            .NotEmpty().WithMessage("Village must be provided");

        //RuleFor(farmer => farmer.BirthMonth)
        //    .InclusiveBetween((short)1, (short)12).WithMessage("Birth month must be between 1 and 12");

        //RuleFor(farmer => farmer.BirthYear)
        //    .GreaterThanOrEqualTo((short)1900).WithMessage("Birth year must be valid")
        //    .LessThanOrEqualTo((short)DateTime.Now.Year).WithMessage("Birth year must not exceed the current year");
    }
}
