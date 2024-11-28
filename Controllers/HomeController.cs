using EmployeeCRUD.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace EmployeeCRUD.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {

            // Set dynamic data
            ViewBag.PageTitle = "Dashboard";
            ViewBag.Breadcrumbs = new List<Breadcrumb>
        {
            new Breadcrumb { Name = "Home", Link = Url.Action("Index", "Home") },
            new Breadcrumb { Name = "Dashboard v1", Link = null } // Current page
        };

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
