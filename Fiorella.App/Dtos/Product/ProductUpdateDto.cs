using Fiorella.App.Dtos.Discount;

namespace Fiorella.App.Dtos.Product
{
    public record ProductUpdateDto
    {
        public string Name { get; set; } = string.Empty;
        public double Price { get; set; }
        public string Info { get; set; } = string.Empty;
        public string TitleDescription { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal Weight { get; set; }
        public string Dimensions { get; set; } = string.Empty;
        public ICollection<int> TagIds { get; set; }
        public ICollection<int> CategoryIds { get; set; }
        public ICollection<IFormFile>? FormFiles { get; set; }
        public int? DiscountId { get; set; }
        public DiscountGetDto? Discount { get; set; }
    }
}
