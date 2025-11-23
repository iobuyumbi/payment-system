using FluentValidation;
using Solidaridad.Application.Models.Farmer;

namespace Solidaridad.Application.Models.Validators.Farmer;

public class ImportFarmerValidator : AbstractValidator<ImportFarmerModel>
{
    public ImportFarmerValidator()
    {
        RuleFor(ctl => ctl.FirstName)
            .NotEmpty().WithMessage("First name must be provided")
            .MinimumLength(FarmerValidationConfiguration.MinimumFirstNameLength)
            .WithMessage(
                $"First name must contain a minimum of {FarmerValidationConfiguration.MinimumFirstNameLength} characters")
            .MaximumLength(FarmerValidationConfiguration.MaximumFirstNameLength)
            .WithMessage(
                $"First name must contain a maximum of {FarmerValidationConfiguration.MaximumFirstNameLength} characters");

        RuleFor(ctl => ctl.OtherNames)
             .NotEmpty().WithMessage("OtherNames must be provided");

        RuleFor(ctl => ctl.BeneficiaryId)
             .NotEmpty().WithMessage("BeneficiaryId must be provided");

        RuleFor(ctl => ctl.CountryName)
            .NotEmpty().WithMessage("Country must be provided");

        RuleFor(ctl => ctl.CooperativeName)
            .NotEmpty().WithMessage("Cooperative must be provided");

        RuleFor(ctl => ctl.Mobile)
            .NotEmpty().WithMessage("Mobile must be provided");

        RuleFor(ctl => ctl.SystemId)
            .NotEmpty().WithMessage("SystemId must be provided");

        RuleFor(ctl => ctl.ParticipantId)
            .NotEmpty().WithMessage("ParticipantId must be provided");

        RuleFor(ctl => ctl.PaymentPhoneNumber)
            .NotEmpty().WithMessage("PaymentPhoneNumber must be provided");

        RuleFor(ctl => ctl.AdminLevel1)
             .NotEmpty().WithMessage("AdminLevel1 must be provided");

        RuleFor(ctl => ctl.AdminLevel2)
             .NotEmpty().WithMessage("AdminLevel2 must be provided");

        RuleFor(ctl => ctl.AdminLevel3)
             .NotEmpty().WithMessage("AdminLevel3 must be provided");

        RuleFor(ctl => ctl.AdminLevel4)
             .NotEmpty().WithMessage("AdminLevel4 must be provided");
    }
}
