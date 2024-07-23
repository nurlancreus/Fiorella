using Fiorella.App.Dtos.Product;
using Fiorella.App.Extensions;
using FluentValidation;

namespace Fiorella.App.Validators.Product
{
    public class ProductPostValidator : AbstractValidator<ProductPostDto>
    {
        public ProductPostValidator()
        {
            RuleFor(p => p.Name)
                .NotNull().WithMessage("Name cannot be null.")
                .NotEmpty().WithMessage("Name cannot be empty.")
                .MaximumLength(100).WithMessage("Name cannot exceed 100 characters.");

            RuleFor(p => p.Price)
                .GreaterThan(0).WithMessage("Price must be greater than 0.");

            RuleFor(p => p.Info)
                .MaximumLength(500).WithMessage("Info cannot exceed 500 characters.");

            RuleFor(p => p.TitleDescription)
                .MaximumLength(200).WithMessage("TitleDescription cannot exceed 200 characters.");

            RuleFor(p => p.Description)
                .MaximumLength(1000).WithMessage("Description cannot exceed 1000 characters.");

            RuleFor(p => p.Weight)
                .GreaterThan(0).WithMessage("Weight must be greater than 0.");

            RuleFor(p => p.Dimensions)
                .MaximumLength(50).WithMessage("Dimensions cannot exceed 50 characters.");

            RuleFor(p => p.DiscountId)
                .Must(BeAValidDiscount).When(p => p.DiscountId.HasValue).WithMessage("Invalid Discount ID.");

            RuleFor(p => p)
                .Must(DiscountLessThanPrice).WithMessage("Discount must be less than the price.");

            RuleFor(p => p.FormFiles).Custom((files, context) =>
            {
                if (files != null)
                {
                    foreach (IFormFile file in files)
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
                }
            });
        }

        private static bool BeAValidDiscount(int? discountId)
        {
            // Implement the logic to validate if the discount ID exists in the database.
            // This can be done by calling a repository method.
            return true; // Assuming it returns true for the sake of this example.
        }

        private static bool DiscountLessThanPrice(ProductPostDto dto)
        {
            if (dto.Discount != null)
            {
                return dto.Discount.Percent < dto.Price;
            }
            return true; // If no discount is provided, it's valid
        }
    }
}
