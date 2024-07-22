using Fiorella.App.Models.Base;

namespace Fiorella.App.Models
{
    public class Product : BaseEntity
    {
        public Product()
        {
            Images = new HashSet<ProductImage>();
            Tags = new HashSet<Tag>();
            Categories = new HashSet<Category>();
            ProductTags = new HashSet<ProductTag>();
            ProductCategories = new HashSet<ProductCategory>();
        }
        public string Name { get; set; } = string.Empty;
        public double Price { get; set; }
        public string Info { get; set; } = string.Empty;
        public string TitleDescription { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal Weight { get; set; }
        public string Dimensions { get; set; } = string.Empty;
        public Discount Discount { get; set; }
        public ICollection<ProductImage> Images { get; set; }
        public ICollection<Tag> Tags { get; set; }
        public ICollection<ProductTag> ProductTags { get; set; }
        public ICollection<Category> Categories { get; set; }
        public ICollection<ProductCategory> ProductCategories { get; set; }
    }
}
