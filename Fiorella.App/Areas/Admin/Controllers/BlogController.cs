using AutoMapper;
using Fiorella.App.Context;
using Fiorella.App.Dtos.Blog;
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
        private readonly IMapper _mapper;

        public BlogController(FiorellaDbContext context, IWebHostEnvironment env, IMapper mapper)
        {
            _context = context;
            _webEnv = env;
            _mapper = mapper;
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            //ICollection<Blog> blogs = await _context.Blogs.Where(x => !x.IsDeleted).ToListAsync();

            List<BlogGetDto> blogs = await _context.Blogs.Select(b => _mapper.Map<BlogGetDto>(b)).ToListAsync();

            return View(blogs);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(BlogPostDto blogDto)
        {
            if (!ModelState.IsValid)
            {
                return View(blogDto);
            }

            if (blogDto.FormFile != null)
            {
                //if (!Helper.IsImage(blogDto.FormFile))
                //{
                //    ModelState.AddModelError(nameof(blogDto.FormFile), "File type must be an image.");
                //    return View();
                //}

                //if (!Helper.IsSizeOk(blogDto.FormFile, 1))
                //{
                //    ModelState.AddModelError(nameof(blogDto.FormFile), "File size must be less than 1 mbs.");
                //    return View();
                //}

                blogDto.Image = await blogDto.FormFile.SaveFileAsync(_webEnv.WebRootPath, "assets/images/blog");
            }

            Blog blog = _mapper.Map<Blog>(blogDto);

            await _context.Blogs.AddAsync(blog);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            Blog? blog = await _context.Blogs.FirstOrDefaultAsync(b => b.Id == id);

            if (blog == null)
            {
                return NotFound();
            }

            //BlogUpdateDto blogDto = new()
            //{
            //    Title = blog.Title,
            //    Description = blog.Description,
            //};

            BlogUpdateDto blogDto = _mapper.Map<BlogUpdateDto>(blog);

            //if (blog.Image != null)
            //{
            //    blogDto.Image = blog.Image;
            //}

            return View(blogDto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int id, BlogUpdateDto updatedBlog)
        {

            Blog? blog = await _context.Blogs.FirstOrDefaultAsync(b => b.Id == id);

            if (blog == null)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                return View(updatedBlog);
            }

            if (updatedBlog.FormFile != null)
            {
                //if (!Helper.IsImage(updatedBlog.FormFile))
                //{
                //    ModelState.AddModelError(nameof(updatedBlog.FormFile), "File type must be an image.");
                //    return View();
                //}

                //if (!Helper.IsSizeOk(updatedBlog.FormFile, 1))
                //{
                //    ModelState.AddModelError(nameof(updatedBlog.FormFile), "File size must be less than 1 mbs.");
                //    return View();
                //}

                if (blog.Image != null)
                    Helper.RemoveImage(_webEnv.WebRootPath, "assets/images/blog", blog.Image);

                blog.Image = await updatedBlog.FormFile.SaveFileAsync(_webEnv.WebRootPath, "assets/images/blog");
            }

            blog.Title = updatedBlog.Title;
            blog.Description = updatedBlog.Description;

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Remove(int id)
        {
            Blog? blog = await _context.Blogs.FirstOrDefaultAsync(b => b.Id == id);

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
