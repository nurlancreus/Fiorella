using AutoMapper;
using Fiorella.App.Context;
using Fiorella.App.Dtos.Product;
using Fiorella.App.Models;
using Fiorella.App.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Linq;

namespace Fiorella.App.Controllers
{
    public class ProductController : Controller
    {
        private readonly FiorellaDbContext _context;
        private readonly IMapper _mapper;
        public ProductController(FiorellaDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<IActionResult> Index(int id)
        {
            ProductGetDto? product = await _context.Products
                                    .Include(p => p.Discount)
                                    .Include(p => p.Categories)
                                    .Include(p => p.Tags)
                                    .Include(p => p.Images)
                                    .Where(p => p.Id == id)
                                    .Select(p => _mapper.Map<ProductGetDto>(p))
                                    .FirstOrDefaultAsync();

            ViewBag.Products = _context.Products.OrderByDescending(p => p.CreatedAt).Take(4).Select(p => _mapper.Map<ProductGetDto>(p));

            return View(product);
        }

        public async Task<IActionResult> AddBasket(int id)
        {
            string BasketCookieName = "basket";

            Product? product = await _context.Products.FindAsync(id);

            if (product == null || product.Stock == 0)
            {
                return BadRequest("There is no product");
            }

            IList<BasketViewModel> basketViewModels = [];

            if (Request.Cookies.ContainsKey(BasketCookieName))
            {
                var basketJson = Request.Cookies[BasketCookieName];

                if (basketJson != null)
                {
                    basketViewModels = JsonConvert.DeserializeObject<IList<BasketViewModel>>(basketJson) ?? [];
                }
            }

            var existingItem = basketViewModels.FirstOrDefault(x => x.ProductId == id);

            if (existingItem != null)
            {
                existingItem.Quantity++;
            }
            else
            {
                basketViewModels.Add(new BasketViewModel
                {
                    ProductId = id,
                    Quantity = 1,
                });
            }

            var updatedBasketJson = JsonConvert.SerializeObject(basketViewModels);

            Response.Cookies.Append(BasketCookieName, updatedBasketJson, new CookieOptions
            {
                Domain = "localhost",
                Path = "/",
                Expires = DateTime.Now.AddDays(1),
                HttpOnly = true,
                Secure = true
            });

            TempData[BasketCookieName] = basketViewModels;

            return Ok();
        }
    }
}
