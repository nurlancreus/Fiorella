using AutoMapper;
using Fiorella.App.Context;
using Fiorella.App.Dtos.Blog;
using Fiorella.App.Dtos.Category;
using Fiorella.App.Dtos.Employee;
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
        private readonly IMapper _mapper;

        public HomeController(FiorellaDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IActionResult> Index()
        {
            HomeViewModel model = new()
            {
                Categories = await _context.Categories.Where(c => !c.IsDeleted).Select(c => _mapper.Map<CategoryGetDto>(c)).ToListAsync(),

                Blogs = await _context.Blogs.Where(b => !b.IsDeleted).OrderByDescending(b => b.CreatedAt).Take(3).Select(b => _mapper.Map<BlogGetDto>(b)).ToListAsync(),

                Employees = await _context.Employees.Where(e => !e.IsDeleted).Include(e => e.Position).OrderByDescending(e => e.CreatedAt).Take(4).Select(e => _mapper.Map<EmployeeGetDto>(e)).ToListAsync()
            };

            return View(model);
        }
    }
}
