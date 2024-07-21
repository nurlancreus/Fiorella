using Fiorella.App.Dtos.Blog;
using Fiorella.App.Extensions;
using FluentValidation;

namespace Fiorella.App.Validators.Blog
{
    public class BlogUpdateDtoValidator : AbstractValidator<BlogUpdateDto>
    {
        public BlogUpdateDtoValidator()
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

                    if (!file.IsSizeOk(1))
                    {
                        context.AddFailure("File must be less than 1 mb.");
                    }

                    if (!file.RestrictMimeTypes())
                    {
                        context.AddFailure("File must be an image.");
                    }
                }
            });
        }


    }
}
