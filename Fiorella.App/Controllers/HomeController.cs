using Fiorella.App.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Fiorella.App.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
