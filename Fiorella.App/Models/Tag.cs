using Fiorella.App.Models.Base;

namespace Fiorella.App.Models
{
    public class Tag : BaseEntity
    {
        public Tag()
        {
            TagProducts = new HashSet<ProductTag>();
        }
        public string Name { get; set; } = string.Empty;
        public ICollection<Product> Products { get; set; }
        public ICollection<ProductTag> TagProducts { get; set; }
    }
}
