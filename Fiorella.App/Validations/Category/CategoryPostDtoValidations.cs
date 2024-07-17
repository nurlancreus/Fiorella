using Fiorella.App.Dtos.Category;
using FluentValidation;

namespace Fiorella.App.Validations.Category
{
    public class CategoryPostDtoValidations : AbstractValidator<CategoryPostDto>

    {
        public CategoryPostDtoValidations()
        {
            RuleFor(c => c.Name).NotNull().WithMessage("Name can not be null.")
                .NotEmpty().WithMessage("Name can not be emplty.")
                .MaximumLength(30)
                .MinimumLength(3);
        }
    }
}
