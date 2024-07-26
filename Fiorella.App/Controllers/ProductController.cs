using AutoMapper;
using Fiorella.App.Context;
using Fiorella.App.Dtos.Product;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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

            return View(product);
        }
    }
}
