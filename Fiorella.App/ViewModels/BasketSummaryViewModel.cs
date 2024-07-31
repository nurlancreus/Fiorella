namespace Fiorella.App.ViewModels
{
    public class BasketSummaryViewModel
    {
        public List<BasketViewModel> Items { get; set; } = [];
        public double TotalPrice { get; set; }
        public int ItemCount { get; set; }
        public string Image { get; set; } = "";
    }
}
