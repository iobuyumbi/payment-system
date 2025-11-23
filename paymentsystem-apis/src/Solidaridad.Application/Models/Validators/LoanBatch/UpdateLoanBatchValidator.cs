using FluentValidation;
using Solidaridad.Application.Models.LoanApplication;

namespace Solidaridad.Application.Models.Validators.LoanBatch;

public class UpdateLoanBatchValidator : AbstractValidator<UpdateLoanBatchModel>
{
    public UpdateLoanBatchValidator()
    {
        RuleFor(loan => loan.Name)
            .NotEmpty().WithMessage("Name must be provided")
            .MinimumLength(2).WithMessage("Name must contain a minimum of 2 characters")
            .MaximumLength(100).WithMessage("Name must contain a maximum of 100 characters");

        RuleFor(loan => loan.InterestRate)
            .GreaterThanOrEqualTo(0).WithMessage("Interest rate must be non-negative")
            .LessThanOrEqualTo(100).WithMessage("Interest rate must be less than or equal to 100");

        RuleFor(loan => loan.PaymentTerms)
            //.NotEmpty().WithMessage("Payment terms must be provided")
            .MaximumLength(200).WithMessage("Payment terms must contain a maximum of 200 characters");

        RuleFor(loan => loan.Tenure)
            .GreaterThan(0).WithMessage("Tenure must be greater than 0");

        RuleFor(loan => loan.GracePeriod)
            .GreaterThanOrEqualTo(0).WithMessage("Grace period must be non-negative");

        RuleFor(loan => loan.RateType)
            .NotEmpty().WithMessage("Rate type must be provided");

        RuleFor(loan => loan.CalculationTimeframe)
            .NotEmpty().WithMessage("Calculation timeframe must be provided");

        //RuleFor(loan => loan.ProcessingFee)
        //    .GreaterThanOrEqualTo(0).WithMessage("Processing fee must be non-negative");

        //RuleFor(loan => loan.EffectiveDate)
        //    .NotEmpty().WithMessage("Effective date must be provided");
        ////.LessThanOrEqualTo(DateTime.Now).WithMessage("Effective date cannot be in the future");

        //RuleFor(loan => loan.InitiationDate)
        //    .NotEmpty().WithMessage("Initiation date must be provided");
        //    //.LessThanOrEqualTo(DateTime.Now).WithMessage("Initiation date cannot be in the future");

        RuleFor(loan => loan.ProjectId)
            .NotEmpty().WithMessage("Project must be provided");

        RuleFor(loan => loan.StatusId)
            .GreaterThan(0).WithMessage("Status must be a valid one");
    }
}
