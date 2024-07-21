using Fiorella.App.Models.Base;
using System.ComponentModel.DataAnnotations;

namespace Fiorella.App.Models
{
    public class Category : BaseEntity
    {
        public Category()
        {
            CategoryProducts = new HashSet<ProductCategory>();
        }

        //[Required(ErrorMessage = "The name is required.")]
        //[StringLength(30)]
        public string Name { get; set; } = string.Empty;
        public ICollection<Product> Products { get; set; }
        public ICollection<ProductCategory> CategoryProducts { get; set; }
    }
}
