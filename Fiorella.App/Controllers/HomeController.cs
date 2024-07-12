using Fiorella.App.Context;
using Fiorella.App.Models;
using Fiorella.App.ViewModels;
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
            HomeViewModel model = new()
            {
                Categories = await _context.Categories.Where(x => !x.IsDeleted).ToListAsync(),
                Blogs = await _context.Blogs.Where(x => !x.IsDeleted).OrderByDescending(x => x.CreatedAt).Take(3).ToListAsync(),
            };

            return View(model);
        }
    }
}
