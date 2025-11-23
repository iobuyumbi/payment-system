using FluentValidation;
using Solidaridad.Application.Models.County;
using Solidaridad.Application.Models.SubCounty;

namespace Solidaridad.Application.Models.Validators.AdminLevel2;

public class UpdateAdminLevel2Validator : AbstractValidator<UpdateAdminLevel2Model>
{
    public UpdateAdminLevel2Validator()
    {
        RuleFor(subCounty => subCounty.SubCountyName)
             .NotEmpty().WithMessage("Sub-county name must be provided")
             .MinimumLength(2).WithMessage("Sub-county name must contain a minimum of 2 characters")
             .MaximumLength(100).WithMessage("Sub-county name must contain a maximum of 100 characters");

        RuleFor(subCounty => subCounty.SubCountyCode)
            .NotEmpty().WithMessage("Sub-county code must be provided")
            .Length(2, 10).WithMessage("Sub-county code must be between 2 and 10 characters");

        RuleFor(subCounty => subCounty.CountyId)
            .NotEmpty().WithMessage("County must be provided");

        RuleFor(subCounty => subCounty.IsActive)
            .NotNull().WithMessage("IsActive status must be specified");
    }
}
