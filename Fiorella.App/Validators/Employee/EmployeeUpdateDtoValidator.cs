using Fiorella.App.Dtos.Employee;
using Fiorella.App.Extensions;
using FluentValidation;

namespace Fiorella.App.Validators.Employee
{
    public class EmployeeUpdateDtoValidator : AbstractValidator<EmployeeUpdateDto>
    {
        public EmployeeUpdateDtoValidator()
        {
            RuleFor(e => e.FirstName).NotNull().WithMessage("First name can not be null.")
                .NotEmpty().WithMessage("FirstName can not be emplty.")
                .MaximumLength(30)
                .MinimumLength(3);

            RuleFor(e => e.LastName).NotNull().WithMessage("Last name can not be null.")
                .NotEmpty().WithMessage("Last name can not be emplty.")
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
