using Fiorella.App.Dtos.Discount;
using FluentValidation;

namespace Fiorella.App.Validators.Discount
{
    public class DiscountUpdateValidator : AbstractValidator<DiscountUpdateDto>
    {
        public DiscountUpdateValidator()
        {
            RuleFor(d => d.Percent).NotNull().WithMessage("Percent field can not be null.")
                .NotEmpty().WithMessage("Percent field can not be empty.");

            RuleFor(d => d.StartDate)
                .NotNull().WithMessage("StartDate field can not be null.")
                .NotEmpty().WithMessage("StartDate field can not be empty.")
                .LessThanOrEqualTo(c => c.EndDate).WithMessage("StartDate must be before or equal to EndDate.");

            RuleFor(d => d.EndDate)
                .NotNull().WithMessage("EndDate field can not be null.")
                .NotEmpty().WithMessage("EndDate field can not be empty.")
                .GreaterThanOrEqualTo(c => c.StartDate).WithMessage("EndDate must be after or equal to StartDate.");
        }
    }
}
