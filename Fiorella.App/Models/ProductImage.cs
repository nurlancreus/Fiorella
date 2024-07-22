using Fiorella.App.Models.Base;

namespace Fiorella.App.Models
{
    public class ProductImage : BaseEntity
    {
        public string Url { get; set; } = string.Empty;
        public bool IsMain { get; set; }
        public int? ProductId { get; set; }
        public Product? Product { get; set; }
    }
}
