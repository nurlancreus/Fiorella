using Fiorella.App.Context;
using Fiorella.App.Extensions;
using Fiorella.App.Helpers;
using Fiorella.App.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
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


            if (blog.FormFile != null)
            {
                if (!Helper.IsImage(blog.FormFile))
                {
                    ModelState.AddModelError("FormFile", "File type must be an image.");
                    return View();
                }

                if (!Helper.IsSizeOk(blog.FormFile, 1))
                {
                    ModelState.AddModelError("FormFile", "File size must be less than 1 mbs.");
                    return View();
                }

                blog.Image = await blog.FormFile.SaveFileAsync(_webEnv.WebRootPath, "assets/images/blog");
            }

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
            {
                if (!Helper.IsImage(updatedBlog.FormFile))
                {
                    ModelState.AddModelError("FormFile", "File type must be an image.");
                    return View();
                }

                if (!Helper.IsSizeOk(updatedBlog.FormFile, 1))
                {
                    ModelState.AddModelError("FormFile", "File size must be less than 1 mbs.");
                    return View();
                }

                if (blog.Image != null)
                    Helper.RemoveImage(_webEnv.WebRootPath, "assets/images/blog", blog.Image);

                blog.Image = await updatedBlog.FormFile.SaveFileAsync(_webEnv.WebRootPath, "assets/images/blog");
            }

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
