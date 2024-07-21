using Fiorella.App.Dtos.Category;
using FluentValidation;

namespace Fiorella.App.Validators.Category
{
    public class CategoryUpdateDtoValidator : AbstractValidator<CategoryUpdateDto>
    {
        public CategoryUpdateDtoValidator()
        {
            RuleFor(c => c.Name).NotNull().WithMessage("Name can not be null.")
                .NotEmpty().WithMessage("Name can not be emplty.")
                .MaximumLength(30)
                .MinimumLength(3);
        }
    }
}
