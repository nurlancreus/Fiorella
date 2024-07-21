using AutoMapper;
using Fiorella.App.Context;
using Fiorella.App.Dtos.Tag;
using Fiorella.App.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Fiorella.App.Areas.Admin.Controllers
{
    [Area("admin")]
    public class TagController : Controller
    {
        private readonly FiorellaDbContext _context;
        private readonly IMapper _mapper;

        public TagController(FiorellaDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var query = _context.Tags.Where(t => !t.IsDeleted);

            List<TagGetDto> tags = await query.Select(t => _mapper.Map<TagGetDto>(t)).ToListAsync();

            return View(tags);
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(TagPostDto tagDto)
        {
            if (!ModelState.IsValid)
            {
                return View(tagDto);
            }

            if (await _context.Tags.AnyAsync(t => t.Name.ToLower() == tagDto.Name.ToLower() && !t.IsDeleted))
            {
                ModelState.AddModelError(nameof(tagDto.Name), $"Tag {tagDto.Name.ToLower()} is already exist");
                return View();
            }

            Tag tag = _mapper.Map<Tag>(tagDto);

            await _context.Tags.AddAsync(tag);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {

            Tag? tag = await _context.Tags.FirstOrDefaultAsync(x => !x.IsDeleted && x.Id == id);
            if (tag == null)
            {
                return NotFound();
            }

            TagUpdateDto tagDto = _mapper.Map<TagUpdateDto>(tag);
            return View(tagDto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int id, TagUpdateDto updateTagDto)
        {

            Tag? tag = await _context.Tags.FirstOrDefaultAsync(t => !t.IsDeleted && t.Id == id);
            if (tag == null)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                return View(updateTagDto);
            }

            tag.Name = updateTagDto.Name;

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Remove(int id)
        {
            Tag? tag = await _context.Tags.FirstOrDefaultAsync(t => !t.IsDeleted && t.Id == id);

            if (tag == null)
            {
                return NotFound();
            }

            tag.IsDeleted = true;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));

        }
    }
}
