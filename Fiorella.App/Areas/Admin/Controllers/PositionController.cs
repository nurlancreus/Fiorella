using AutoMapper;
using Fiorella.App.Context;
using Fiorella.App.Dtos.Position;
using Fiorella.App.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Fiorella.App.Areas.Admin.Controllers
{
    [Area("admin")]
    public class PositionController : Controller
    {
        private readonly FiorellaDbContext _context;
        private readonly IMapper _mapper;

        public PositionController(FiorellaDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<IActionResult> Index()
        {
            //List<Position> positions = await _context.Positions.Where(x => !x.IsDeleted).ToListAsync();
            List<PositionDto> positions = await _context.Positions.Select(p => _mapper.Map<PositionDto>(p)).ToListAsync();  

            return View(positions);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PositionDto positionDto)
        {
            if (!ModelState.IsValid)
            {
                return View(positionDto);
            }

            Position position = _mapper.Map<Position>(positionDto);

            await _context.Positions.AddAsync(position);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Update(int id)
        {
            Position? position = await _context.Positions.FirstOrDefaultAsync(p => p.Id == id);

            if (position == null)
            {
                return NotFound();
            }

            PositionDto positionDto = _mapper.Map<PositionDto>(position);

            return View(positionDto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int id, PositionDto updatedPosition)
        {
            Position? position = await _context.Positions.FirstOrDefaultAsync(p => p.Id == id);

            if (position == null)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                return View(updatedPosition);
            }

            position.Name = updatedPosition.Name;
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Remove(int id)
        {
            Position? position = await _context.Positions.FirstOrDefaultAsync(p => p.Id == id);

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
