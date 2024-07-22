namespace Fiorella.App.Dtos.Discount
{
    public record DiscountUpdateDto
    {
        public double Percent { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
}
