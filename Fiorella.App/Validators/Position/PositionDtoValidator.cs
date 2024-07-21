using Fiorella.App.Dtos.Position;
using FluentValidation;

namespace Fiorella.App.Validators.Position
{
    public class PositionDtoValidator : AbstractValidator<PositionDto>
    {
        public PositionDtoValidator()
        {
            RuleFor(c => c.Name).NotNull().WithMessage("Position can not be null.")
                .NotEmpty().WithMessage("Position can not be emplty.")
                .MaximumLength(30)
                .MinimumLength(3);
        }
    }
}
