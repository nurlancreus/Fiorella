using Fiorella.App.Dtos.Blog;
using Fiorella.App.Helpers;
using FluentValidation;

namespace Fiorella.App.Validations.Blog
{
    public class BlogUpdateDtoValidations : AbstractValidator<BlogUpdateDto>
    {
        public BlogUpdateDtoValidations()
        {
            RuleFor(b => b.Title).NotNull().WithMessage("Title can not be null.")
                .NotEmpty().WithMessage("Title can not be emplty.")
                .MaximumLength(30)
                .MinimumLength(3);

            RuleFor(b => b.Description).NotNull().WithMessage("Description can not be null.")
                    .NotEmpty().WithMessage("Description can not be emplty.")
                    .MaximumLength(300)
                    .MinimumLength(10);

            RuleFor(b => b.FormFile).Custom((file, context) =>
            {
                if (file != null)
                {

                    if (Helper.IsSizeOk(file, 1))
                    {
                        context.AddFailure("File must be less than 1 mb.");
                    }

                    if (Helper.IsImage(file))
                    {
                        context.AddFailure("File must be an image.");
                    }
                }
            });
        }


    }
}
