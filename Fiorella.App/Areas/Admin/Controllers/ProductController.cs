using AutoMapper;
using Fiorella.App.Context;
using Fiorella.App.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Fiorella.App.Areas.Admin.Controllers
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

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Product product)
        {
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
