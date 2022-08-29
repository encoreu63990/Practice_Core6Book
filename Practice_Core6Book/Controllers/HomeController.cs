using Microsoft.AspNetCore.Mvc;
using Practice_Core6Book.Models;
using Serilog;
using System.Diagnostics;

namespace Practice_Core6Book.Controllers
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


            Log.Verbose("Verbose");
            Log.Debug("Debug");
            Log.Information("Information");
            Log.Warning("Warning");
            Log.Error("Error");
            Log.Fatal("Fatal");
            return View();
        }

        public IActionResult Get()
        {
            int[] arr = new int[] { 1, 2, 3, 4, 5 };
            return Json(arr);
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