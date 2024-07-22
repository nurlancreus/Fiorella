using Fiorella.App.Dtos.Product;

namespace Fiorella.App.Dtos.Discount
{
    public record DiscountGetDto
    {
        public DiscountGetDto()
        {
            Products = new HashSet<ProductGetDto>();
        }
        public int Id { get; set; }
        public double Percent { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public ICollection<ProductGetDto> Products { get; set; }
    }
}
