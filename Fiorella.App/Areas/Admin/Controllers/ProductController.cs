using AutoMapper;
using Fiorella.App.Context;
using Fiorella.App.Dtos.Category;
using Fiorella.App.Dtos.Discount;
using Fiorella.App.Dtos.Product;
using Fiorella.App.Dtos.Tag;
using Fiorella.App.Extensions;
using Fiorella.App.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Fiorella.App.Areas.Admin.Controllers
{
    public class ProductController : Controller
    {
        private readonly FiorellaDbContext _context;
        private readonly IWebHostEnvironment _webEnv;
        private readonly IMapper _mapper;

        public ProductController(FiorellaDbContext context, IWebHostEnvironment webEnv, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
            _webEnv = webEnv;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var query = _context.Products.Where(p => !p.IsDeleted);

            List<ProductGetDto> products = await query.Include(p => p.Images).Select(p => _mapper.Map<ProductGetDto>(p)).ToListAsync();

            return View(products);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            ViewBag.Categories = await _context.Categories.Where(c => !c.IsDeleted).Select(c => _mapper.Map<CategoryGetDto>(c)).ToListAsync();

            ViewBag.Tags = await _context.Tags.Where(t => !t.IsDeleted).Select(t => _mapper.Map<TagGetDto>(t)).ToListAsync();

            ViewBag.Discounts = await _context.Discounts.Where(d => !d.IsDeleted).Select(d => _mapper.Map<DiscountGetDto>(d)).ToListAsync();

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(ProductPostDto productDto)
        {
            ViewBag.Categories = await _context.Categories.Where(c => !c.IsDeleted).Select(c => _mapper.Map<CategoryGetDto>(c)).ToListAsync();

            ViewBag.Tags = await _context.Tags.Where(t => !t.IsDeleted).Select(t => _mapper.Map<TagGetDto>(t)).ToListAsync();

            ViewBag.Discounts = await _context.Discounts.Where(d => !d.IsDeleted).Select(d => _mapper.Map<DiscountGetDto>(d)).ToListAsync();

            if (productDto.FormFiles != null)
            {
              ICollection<string> fileNames = await productDto.FormFiles.SaveMultipleFileAsync(_webEnv.WebRootPath, "assets/images/product");

                foreach (var fileName in fileNames)
                {
                    productDto.Images.Add(new() { Url = fileName });
                }
            }

            Product product = _mapper.Map<Product>(productDto);

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult Update(int id)
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Update(int id, Product updatedProduct)
        {
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Remove(int id)
        {
            return RedirectToAction(nameof(Index));
        }
    }
}
