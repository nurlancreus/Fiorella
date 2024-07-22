using AutoMapper;
using Fiorella.App.Context;
using Fiorella.App.Dtos.Discount;
using Fiorella.App.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Fiorella.App.Areas.Admin.Controllers
{
    [Area("admin")]
    public class DiscountController : Controller
    {
        private readonly FiorellaDbContext _context;
        private readonly IMapper _mapper;

        public DiscountController(FiorellaDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var query = _context.Discounts.Where(d => !d.IsDeleted);

            ICollection<DiscountGetDto> discounts = await query.Select(c => _mapper.Map<DiscountGetDto>(c)).ToListAsync();

            return View(discounts);
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(DiscountPostDto discountDto)
        {
            if (!ModelState.IsValid)
            {
                return View(discountDto);
            }

            if (await _context.Discounts.AnyAsync(d => d.Percent == discountDto.Percent && !d.IsDeleted))
            {
                ModelState.AddModelError(nameof(discountDto.Percent), $"Discount {discountDto.Percent}% is already exist");
                return View();
            }

            Discount discount = _mapper.Map<Discount>(discountDto);

            await _context.Discounts.AddAsync(discount);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {

            Discount? Discount = await _context.Discounts.FirstOrDefaultAsync(d => !d.IsDeleted && d.Id == id);
            if (Discount == null)
            {
                return NotFound();
            }

            DiscountUpdateDto DiscountDto = _mapper.Map<DiscountUpdateDto>(Discount);
            return View(DiscountDto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int id, DiscountUpdateDto updateDiscountDto)
        {

            Discount? discount = await _context.Discounts.FirstOrDefaultAsync(d => !d.IsDeleted && d.Id == id);
            if (discount == null)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                return View(updateDiscountDto);
            }

            discount.Percent = updateDiscountDto.Percent;
            discount.StartDate = updateDiscountDto.StartDate;

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Remove(int id)
        {
            Discount? discount = await _context.Discounts.FirstOrDefaultAsync(d => !d.IsDeleted && d.Id == id);

            if (discount == null)
            {
                return NotFound();
            }

            discount.IsDeleted = true;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));

        }
    }
}
