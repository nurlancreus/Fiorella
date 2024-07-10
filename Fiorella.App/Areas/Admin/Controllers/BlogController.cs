using Fiorella.App.Context;
using Fiorella.App.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Fiorella.App.Areas.Admin.Controllers
{
    [Area("admin")]
    public class BlogController : Controller
    {
        private readonly FiorellaDbContext _context;
        private readonly IWebHostEnvironment _webEnv;

        public BlogController(FiorellaDbContext context, IWebHostEnvironment _env)
        {
            _context = context;
            _webEnv = _env;
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            ICollection<Blog> blogs = await _context.Blogs.Where(x => !x.IsDeleted).ToListAsync();

            return View(blogs);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Blog blog)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            string root = _webEnv.WebRootPath;
            string path = "assets/images/blog";
            string fileName = Guid.NewGuid().ToString() + blog.FormFile.FileName;
            string fullPath = Path.Combine(root, path, fileName);

            using (FileStream fileStream = new(fullPath, FileMode.Create))
            {
                blog.FormFile.CopyTo(fileStream);
            }

            blog.Image = fileName;

            await _context.Blogs.AddAsync(blog);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
