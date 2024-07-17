using Fiorella.App.Context;
using Fiorella.App.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Fiorella.App.Areas.Admin.Controllers
{
    [Area("admin")]
    public class PositionController : Controller
    {
        public readonly FiorellaDbContext _context;

        public PositionController(FiorellaDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            List<Position> positions = await _context.Positions.Where(x => !x.IsDeleted).ToListAsync();

            return View(positions);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Position position)
        {
            if (!ModelState.IsValid)
            {
                return View(position);
            }

            await _context.Positions.AddAsync(position);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Update(int id)
        {
            Position? position = await _context.Positions.FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted);

            if (position == null)
            {
                return NotFound();
            }

            return View(position);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int id, Position updatedPosition)
        {
            Position? position = await _context.Positions.FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted);

            if (position == null)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                return View(position);
            }

            position.Name = updatedPosition.Name;
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Remove(int id)
        {
            Position? position = await _context.Positions.FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted);

            if (position == null)
            {
                return NotFound();
            }

            position.IsDeleted = true;

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}
