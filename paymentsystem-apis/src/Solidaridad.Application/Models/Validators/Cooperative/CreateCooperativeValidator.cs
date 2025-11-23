using FluentValidation;
using Solidaridad.Application.Models.Cooperative;

namespace Solidaridad.Application.Models.Validators.Cooperative;


public class CreateCooperativeValidator : AbstractValidator<CreateCooperativeModel>
{
    public CreateCooperativeValidator()
    {
        RuleFor(coop => coop.Name)
            .NotEmpty().WithMessage("Name must be provided")
            .MinimumLength(2).WithMessage("Name must contain a minimum of 2 characters")
            .MaximumLength(100).WithMessage("Name must contain a maximum of 100 characters");

        RuleFor(coop => coop.CountryId)
            .NotEmpty().WithMessage("Country ID must be provided");

        RuleFor(coop => coop.Description)
            .MaximumLength(500).WithMessage("Description must contain a maximum of 500 characters")
            .When(coop => !string.IsNullOrEmpty(coop.Description));
    }
}
