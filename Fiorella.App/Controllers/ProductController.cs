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
        private readonly string BasketCookieName = "Basket";

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

        [HttpPost]
        public async Task<IActionResult> AddBasket(int id, [FromBody] int? quantity)
        {
            Product? product = await _context.Products.FindAsync(id);

            if (product == null || product.Stock == 0)
            {
                return BadRequest("There is no product");
            }

            // Explicitly load the main image
            await _context.Entry(product)
                .Collection(p => p.Images)
                .Query()
                .Where(i => i.IsMain)
                .LoadAsync();

            ProductImage? mainImage = product.Images.FirstOrDefault(i => i.IsMain);

            IList<BasketViewModel> basketViewModels = GetBasketFromCookies();

            var existingItem = basketViewModels.FirstOrDefault(x => x.ProductId == id);

            if (existingItem != null)
            {
                existingItem.Quantity++;
            }
            else
            {
                BasketViewModel addedProduct = new()
                {
                    ProductId = id,
                    Quantity = quantity ?? 1,
                    ProductName = product.Name,
                    Price = product.Price,
                };

                if (mainImage != null)
                {
                    addedProduct.Image = mainImage.Url;
                }

                basketViewModels.Add(addedProduct);
            }

            UpdateBasketCookie(basketViewModels);

            return Ok();
        }

        public IActionResult DeleteBasketItem(int id)
        {
            var basketViewModels = GetBasketFromCookies();
            var itemToRemove = basketViewModels.FirstOrDefault(x => x.ProductId == id);

            if (itemToRemove != null)
            {
                basketViewModels.Remove(itemToRemove);
                UpdateBasketCookie(basketViewModels);
                return Ok();
            }

            return NotFound("Item not found in basket");
        }

        [NonAction]
        private IList<BasketViewModel> GetBasketFromCookies()
        {
            if (Request.Cookies.ContainsKey(BasketCookieName))
            {
                var basketJson = Request.Cookies[BasketCookieName];
                if (!string.IsNullOrEmpty(basketJson))
                {
                    return JsonConvert.DeserializeObject<IList<BasketViewModel>>(basketJson) ?? [];
                }
            }
            return [];
        }

        [NonAction]
        private void UpdateBasketCookie(IList<BasketViewModel> basketViewModels)
        {
            var updatedBasketJson = JsonConvert.SerializeObject(basketViewModels);
            Response.Cookies.Append(BasketCookieName, updatedBasketJson, new CookieOptions
            {
                Domain = "localhost",
                Path = "/",
                Expires = DateTime.Now.AddDays(1),
                HttpOnly = true,
                Secure = true
            });
        }
    }
}
