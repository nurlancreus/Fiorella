using Fiorella.App.Context;
using Fiorella.App.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace Fiorella.App.Controllers
{
    public class HomeController : Controller
    {
        private readonly FiorellaDbContext _context;

        public HomeController(FiorellaDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            ICollection<Category> categories = await _context.Categories.Where(x => !x.IsDeleted).ToListAsync();

            return View(categories);
        }
    }
}
