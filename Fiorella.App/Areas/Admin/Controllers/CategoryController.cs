using AutoMapper;
using Fiorella.App.Context;
using Fiorella.App.Dtos.Category;
using Fiorella.App.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Fiorella.App.Areas.Admin.Controllers
{
    [Area("admin")]
    public class CategoryController : Controller
    {
        private readonly FiorellaDbContext _context;
        private readonly IMapper _mapper;

        public CategoryController(FiorellaDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            //IEnumerable<Category> categories = await _context.Categories.Where(x => !x.IsDeleted).ToListAsync();

            var query = _context.Categories.Where(c => !c.IsDeleted).AsQueryable();

            ICollection<CategoryGetDto> categories = await query.Select(c => new CategoryGetDto()
            {
                Id = c.Id,
                Name = c.Name
            }).ToListAsync();

            return View(categories);
        }
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CategoryPostDto categoryDto)
        {
            if (!ModelState.IsValid)
            {
                return View(categoryDto);
            }

            if (await _context.Categories.AnyAsync(x => x.Name.Equals(categoryDto.Name, StringComparison.CurrentCultureIgnoreCase) && !x.IsDeleted))
            {
                ModelState.AddModelError(nameof(categoryDto.Name), $"Category {categoryDto.Name} is already exist");
                return View();
            }

            Category category = _mapper.Map<Category>(categoryDto);

            await _context.Categories.AddAsync(category);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {

            Category? category = await _context.Categories.FirstOrDefaultAsync(x => !x.IsDeleted && x.Id == id);
            if (category == null)
            {
                return NotFound();
            }

            CategoryUpdateDto categoryDto = new() { Name = category.Name };
            return View(categoryDto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update([FromRoute] int id, [FromForm] CategoryUpdateDto updateCategoryDto)
        {

            Category? category = await _context.Categories.FirstOrDefaultAsync(x => !x.IsDeleted && x.Id == id);
            if (category == null)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                return View(updateCategoryDto);
            }

            category.Name = updateCategoryDto.Name;

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Remove(int id)
        {
            Category? category = await _context.Categories.FirstOrDefaultAsync(x => !x.IsDeleted && x.Id == id);

            if (category == null)
            {
                return NotFound();
            }

            category.IsDeleted = true;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));

        }
    }
}
