using FluentValidation;
using Solidaridad.Application.Models.Ward;

namespace Solidaridad.Application.Models.Validators.AdminLevel3;

public class CreateAdminLevel3Validator : AbstractValidator<CreateAdminLevel3Model>
{
    public CreateAdminLevel3Validator()
    {
        RuleFor(admin => admin.WardName)
            .NotEmpty().WithMessage("Ward name must be provided")
            .MinimumLength(2).WithMessage("Ward name must contain a minimum of 2 characters")
            .MaximumLength(100).WithMessage("Ward name must contain a maximum of 100 characters");

        RuleFor(admin => admin.WardCode)
            .NotEmpty().WithMessage("Ward code must be provided")
            .Length(2, 10).WithMessage("Ward code must be between 2 and 10 characters");

        RuleFor(admin => admin.SubCountyId)
            .NotEmpty().WithMessage("Sub-county must be provided");

        RuleFor(admin => admin.IsActive)
            .Must(value => value == true || value == false)
            .WithMessage("IsActive status must be specified as true or false");
    }
}
