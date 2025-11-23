using FluentValidation;
using Solidaridad.Application.Models.Project;

namespace Solidaridad.Application.Models.Validators.Project;

public class UpdateProjectValidator : AbstractValidator<UpdateProjectModel>
{
    public UpdateProjectValidator()
    {
        RuleFor(project => project.ProjectName)
            .NotEmpty().WithMessage("Project name must be provided")
            .MinimumLength(3).WithMessage("Project name must contain a minimum of 3 characters")
            .MaximumLength(100).WithMessage("Project name must contain a maximum of 100 characters");

        RuleFor(project => project.CountryId)
            .NotEmpty().WithMessage("Country ID must be provided");

        //RuleFor(project => project.Address)
        //    .NotNull().WithMessage("Address must be provided")
        //    .SetValidator(new AddressValidator());

        RuleFor(project => project.ProjectCode)
            .NotEmpty().WithMessage("Project code must be provided")
            .Matches(@"^[A-Z0-9]+$").WithMessage("Project code must contain only uppercase letters and numbers")
            .MaximumLength(10).WithMessage("Project code must contain a maximum of 10 characters");

        RuleFor(project => project.Description)
            .MaximumLength(500).WithMessage("Description must contain a maximum of 500 characters")
            .When(project => !string.IsNullOrEmpty(project.Description));
    }
}
