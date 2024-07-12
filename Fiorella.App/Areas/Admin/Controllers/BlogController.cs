using Fiorella.App.Areas.Admin.Extensions;
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
            //string path = "assets/images/blog";
            //string fileName = Guid.NewGuid().ToString() + blog.FormFile.FileName;
            //string fullPath = Path.Combine(root, path, fileName);

            //using (FileStream fileStream = new(fullPath, FileMode.Create))
            //{
            //    blog.FormFile.CopyTo(fileStream);


            if (blog.FormFile == null) return View();

            string fileName = await blog.FormFile.GetFileNameAsync(root, "assets/images/blog");

            blog.Image = fileName;

            await _context.Blogs.AddAsync(blog);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            Blog? blog = await _context.Blogs.FirstOrDefaultAsync(x => !x.IsDeleted && x.Id == id);

            if (blog == null)
            {
                return NotFound();
            }

            return View(blog);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int id, Blog updatedBlog)
        {
            if (!ModelState.IsValid)
            {
                return View(updatedBlog);
            }

            Blog? blog = await _context.Blogs.FirstOrDefaultAsync(x => !x.IsDeleted && x.Id == id);

            if (blog == null)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                return View(blog);
            }

            blog.Title = updatedBlog.Title;
            blog.Description = updatedBlog.Description;

            if (updatedBlog.FormFile != null)
                blog.Image = await updatedBlog.FormFile.GetFileNameAsync(_webEnv.WebRootPath, "assets/images/blog");

            blog.UpdatedAt = DateTime.Now;

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Remove(int id)
        {
            Blog? blog = await _context.Blogs.FirstOrDefaultAsync(x => !x.IsDeleted && x.Id == id);

            if (blog == null)
            {
                return NotFound();
            }

            blog.IsDeleted = true;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
