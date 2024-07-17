using Fiorella.App.Context;
using Fiorella.App.Extensions;
using Fiorella.App.Helpers;
using Fiorella.App.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata;

namespace Fiorella.App.Areas.Admin.Controllers
{
    [Area("admin")]
    public class EmployeeController : Controller
    {
        private readonly FiorellaDbContext _context;
        private readonly IWebHostEnvironment _webEnv;

        public EmployeeController(FiorellaDbContext context, IWebHostEnvironment _env)
        {
            _context = context;
            _webEnv = _env;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            List<Employee> employees = await _context.Employees.Where(x => !x.IsDeleted).Include(e => e.Position).ToListAsync();

            return View(employees);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            ViewBag.Positions = await _context.Positions.Where(x => !x.IsDeleted).ToListAsync();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Employee employee)
        {
            ViewBag.Positions = await _context.Positions.Where(x => !x.IsDeleted).ToListAsync();

            if (!ModelState.IsValid)
            {
                return View();
            }

            if (employee.FormFile != null)
            {
                if (!Helper.IsImage(employee.FormFile))
                {
                    ModelState.AddModelError(nameof(employee.FormFile), "File type must be an image.");
                    return View();
                }

                if (!Helper.IsSizeOk(employee.FormFile, 1))
                {
                    ModelState.AddModelError(nameof(employee.FormFile), "File size must be less than 1 mbs.");
                    return View();
                }
                employee.Image = await employee.FormFile.SaveFileAsync(_webEnv.WebRootPath, "assets/images/employee");
            }

            if (employee.PositionId == 0) employee.PositionId = null;

            await _context.Employees.AddAsync(employee);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            ViewBag.Positions = await _context.Positions.Where(x => !x.IsDeleted).ToListAsync();
            Employee? employee = await _context.Employees.FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted);

            if (employee == null)
            {
                return NotFound();
            }

            return View(employee);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int id, Employee updatedEmployee)
        {
            Employee? employee = await _context.Employees.FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted);

            if (employee == null)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                return View(employee);
            }

            if (updatedEmployee.FormFile != null)
            {
                if (!Helper.IsImage(updatedEmployee.FormFile))
                {
                    ModelState.AddModelError(nameof(employee.FormFile), "File type must be an image.");
                    return View();
                }

                if (!Helper.IsSizeOk(updatedEmployee.FormFile, 1))
                {
                    ModelState.AddModelError(nameof(employee.FormFile), "File size must be less than 1 mbs.");
                    return View();
                }

                if (employee.Image != null)
                    Helper.RemoveImage(_webEnv.WebRootPath, "assets/images/employee", employee.Image);

                employee.Image = await updatedEmployee.FormFile.SaveFileAsync(_webEnv.WebRootPath, "assets/images/employee");
            }

            if (updatedEmployee.PositionId > 0)
            {
                employee.PositionId = updatedEmployee.PositionId;
            }

            employee.FirstName = updatedEmployee.FirstName;
            employee.LastName = updatedEmployee.LastName;

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Remove(int id)
        {
            Employee? employee = await _context.Employees.FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted);

            if (employee == null)
            {
                return NotFound();
            }

            employee.IsDeleted = true;

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}
