using Fiorella.App.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Fiorella.App.ViewComponents
{
    public class BasketViewComponent(IHttpContextAccessor httpContextAccessor) : ViewComponent
    {
        private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

        public IViewComponentResult Invoke()
        {
            string? basketJson = _httpContextAccessor.HttpContext?.Request.Cookies["Basket"];
            List<BasketViewModel> basketItems = [];

            if (!string.IsNullOrEmpty(basketJson))
            {
                basketItems = JsonConvert.DeserializeObject<List<BasketViewModel>>(basketJson)!;
            }

            var totalPrice = basketItems.Sum(item => item.Price * item.Quantity);
            var itemCount = basketItems.Sum(item => item.Quantity);

            BasketSummaryViewModel model = new ()
            {
                Items = basketItems,
                TotalPrice = totalPrice,
                ItemCount = itemCount
            };

            return View(model);
        }
    }
}
