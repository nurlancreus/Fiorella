using Fiorella.App.Models;

namespace Fiorella.App.Dtos.Discount
{
    public record DiscountPostDto
    {
        public double Percent { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
}
