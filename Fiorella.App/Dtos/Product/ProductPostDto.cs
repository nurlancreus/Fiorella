using Fiorella.App.Dtos.Category;
using Fiorella.App.Dtos.Discount;
using Fiorella.App.Dtos.ProductImage;
using Fiorella.App.Dtos.Tag;
using Fiorella.App.Models;

namespace Fiorella.App.Dtos.Product
{
    public record ProductPostDto
    {
        public ProductPostDto()
        {
            TagIds = new HashSet<int>();
            CategoryIds = new HashSet<int>();

            ProductCategories = new HashSet<ProductCategory>();
            ProductTags = new HashSet<ProductTag>();

            Images = new HashSet<ProductImagePostDto>();
        }
        public string Name { get; set; } = string.Empty;
        public double Price { get; set; }
        public string Info { get; set; } = string.Empty;
        public string TitleDescription { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal Weight { get; set; }
        public string Dimensions { get; set; } = string.Empty;
        public ICollection<int> TagIds { get; set; }
        public ICollection<int> CategoryIds { get; set; }
        //public ICollection<TagPostDto> Tags { get; set; }
        //public ICollection<CategoryPostDto> Categories { get; set; }
        public ICollection<ProductCategory> ProductCategories { get; set; }
        public ICollection<ProductTag> ProductTags { get; set; }
        public ICollection<IFormFile>? FormFiles { get; set; }
        public ICollection<ProductImagePostDto> Images { get; set; }

        public int? DiscountId { get; set; }
        public DiscountGetDto? Discount { get; set; }
    }
}
