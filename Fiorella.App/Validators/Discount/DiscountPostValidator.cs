using Fiorella.App.Dtos.Discount;
using FluentValidation;

namespace Fiorella.App.Validators.Discount
{
    public class DiscountPostValidator : AbstractValidator<DiscountPostDto>
    {
        public DiscountPostValidator()
        {

            RuleFor(c => c.Percent)
                .NotNull().WithMessage("Percent field can not be null.")
                .NotEmpty().WithMessage("Percent field can not be empty.")
                .Must(BeNumeric).WithMessage("Percent field must be a numeric value.")
                .InclusiveBetween(0, 100).WithMessage("Percent field must be between 0 and 100.");

            RuleFor(c => c.StartDate)
                .NotNull().WithMessage("StartDate field can not be null.")
                .NotEmpty().WithMessage("StartDate field can not be empty.")
                .LessThanOrEqualTo(c => c.EndDate).WithMessage("StartDate must be before or equal to EndDate.");

            RuleFor(c => c.EndDate)
                .NotNull().WithMessage("EndDate field can not be null.")
                .NotEmpty().WithMessage("EndDate field can not be empty.")
                .GreaterThanOrEqualTo(c => c.StartDate).WithMessage("EndDate must be after or equal to StartDate.");
        }

        private static bool BeNumeric(double percent)
        {
            return int.TryParse(percent.ToString(), out _);
        }

    }
}
