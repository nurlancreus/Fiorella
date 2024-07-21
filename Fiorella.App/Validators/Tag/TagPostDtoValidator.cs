using Fiorella.App.Dtos.Tag;
using FluentValidation;

namespace Fiorella.App.Validators.Tag
{
    public class TagPostDtoValidator : AbstractValidator<TagPostDto>

    {
        public TagPostDtoValidator()
        {
            RuleFor(c => c.Name).NotNull().WithMessage("Name can not be null.")
                .NotEmpty().WithMessage("Name can not be emplty.")
                .MaximumLength(30)
                .MinimumLength(3);
        }
    }
}
