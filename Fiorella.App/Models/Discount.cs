using Fiorella.App.Models.Base;

namespace Fiorella.App.Models
{
    public class Discount : BaseEntity
    {
        public double Percent { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public ICollection<Product> Products { get; set; } = [];
    }
}
