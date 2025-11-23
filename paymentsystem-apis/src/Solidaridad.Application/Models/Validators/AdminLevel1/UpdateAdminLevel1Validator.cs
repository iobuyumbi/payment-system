using FluentValidation;
using Solidaridad.Application.Models.Cooperative;
using Solidaridad.Application.Models.County;

namespace Solidaridad.Application.Models.Validators.AdminLevel1;

public class UpdateAdminLevel1Validator : AbstractValidator<UpdateAdminLevel1Model>
{
    public UpdateAdminLevel1Validator()
    {
        RuleFor(county => county.CountyName)
                 .NotEmpty().WithMessage("County name must be provided")
                 .MinimumLength(2).WithMessage("County name must contain a minimum of 2 characters")
                 .MaximumLength(100).WithMessage("County name must contain a maximum of 100 characters");

        RuleFor(county => county.CountyCode)
            .NotEmpty().WithMessage("County code must be provided")
            .Length(2, 10).WithMessage("County code must be between 2 and 10 characters");

        RuleFor(county => county.CountryId)
            .NotEmpty().WithMessage("Country must be provided");

        RuleFor(county => county.IsActive)
            .NotNull().WithMessage("IsActive status must be specified");
    }
}
