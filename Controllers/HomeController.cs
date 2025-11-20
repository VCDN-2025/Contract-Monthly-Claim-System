using System.Diagnostics;
using CMCS.Models;
using Microsoft.AspNetCore.Mvc;

namespace CMCS.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        // Constructor: Initializes the controller with a logger dependency.
        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        // Index (GET): Displays the application's main home page.
        public IActionResult Index()
        {
            return View();
        }

        // Privacy (GET): Displays the application's privacy policy page.
        public IActionResult Privacy()
        {
            return View();
        }

        // Error (GET): Displays the error page with details about the current request's error.
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}