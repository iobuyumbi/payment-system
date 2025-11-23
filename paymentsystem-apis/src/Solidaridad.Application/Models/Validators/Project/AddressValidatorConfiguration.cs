using FluentValidation;
using Solidaridad.Core.Entities;

namespace Solidaridad.Application.Models.Validators.Project;

public class AddressValidator : AbstractValidator<Address>
{
    public AddressValidator()
    {
        RuleFor(address => address.CountryId)
            .NotEmpty().WithMessage("Country ID must be provided");

        RuleFor(address => address.AdminLevel1Id)
            .NotEmpty().WithMessage("Admin Level 1 ID must be provided");

        RuleFor(address => address.AdminLevel2Id)
            .NotEmpty().When(address => address.AdminLevel1Id != Guid.Empty)
            .WithMessage("Admin Level 2 ID must be provided when Admin Level 1 ID is specified");

        RuleFor(address => address.AdminLevel3Id)
            .NotEmpty().When(address => address.AdminLevel2Id != Guid.Empty)
            .WithMessage("Admin Level 3 ID must be provided when Admin Level 2 ID is specified");

        RuleFor(address => address.AdminLevel4Id)
            .NotEmpty().When(address => address.AdminLevel3Id != Guid.Empty)
            .WithMessage("Admin Level 4 ID must be provided when Admin Level 3 ID is specified");

        RuleFor(address => address.CreatedBy)
            .NotEmpty().WithMessage("CreatedBy ID must be provided");

        RuleFor(address => address.CreatedOn)
            .NotEmpty().WithMessage("CreatedOn date must be provided")
            .LessThanOrEqualTo(DateTime.Now).WithMessage("CreatedOn date cannot be in the future");

        RuleFor(address => address.UpdatedBy)
            .NotEmpty().When(address => address.UpdatedOn.HasValue)
            .WithMessage("UpdatedBy ID must be provided if UpdatedOn is specified");

        RuleFor(address => address.UpdatedOn)
            .LessThanOrEqualTo(DateTime.Now).When(address => address.UpdatedOn.HasValue)
            .WithMessage("UpdatedOn date cannot be in the future");
    }
}
