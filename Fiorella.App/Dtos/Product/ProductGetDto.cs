using Fiorella.App.Dtos.Category;
using Fiorella.App.Dtos.Discount;
using Fiorella.App.Dtos.ProductImage;
using Fiorella.App.Dtos.Tag;
using Fiorella.App.Models;

namespace Fiorella.App.Dtos.Product
{
    public record ProductGetDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public double Price { get; set; }
        public string Info { get; set; } = string.Empty;
        public string TitleDescription { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal Weight { get; set; }
        public string Dimensions { get; set; } = string.Empty;
        public int? DiscountId { get; set; }
        public DiscountGetDto? Discount { get; set; }
        public ICollection<ProductImageGetDto> Images { get; set; } = [];
        public ICollection<TagGetDto> Tags { get; set; } = [];
        public ICollection<CategoryGetDto> Categories { get; set; } = [];

    }
}
