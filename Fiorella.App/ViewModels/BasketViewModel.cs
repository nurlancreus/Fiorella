namespace Fiorella.App.ViewModels
{
    public class BasketViewModel
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public double Price { get; set; }
        public string Image { get; set; } = "default-img.jpg";
    }
}
