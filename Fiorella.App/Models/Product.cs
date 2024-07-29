using Fiorella.App.Models.Base;

namespace Fiorella.App.Models
{
    public class Product : BaseEntity
    {
        public string Name { get; set; } = string.Empty;
        public double Price { get; set; }
        public int Stock { get; set; }
        public string Info { get; set; } = string.Empty;
        public string TitleDescription { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal Weight { get; set; }
        public string Dimensions { get; set; } = string.Empty;
        public int? DiscountId { get; set; }
        public Discount? Discount { get; set; }
        public ICollection<ProductImage> Images { get; set; } = [];
        public ICollection<Tag> Tags { get; set; } = [];
        public ICollection<ProductTag> ProductTags { get; set; } = [];
        public ICollection<Category> Categories { get; set; } = [];
        public ICollection<ProductCategory> ProductCategories { get; set; } = [];
    }
}
