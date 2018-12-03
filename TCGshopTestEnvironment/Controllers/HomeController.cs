using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using TCGshopTestEnvironment.Models;
using TCGshopTestEnvironment.Services;

namespace TCGshopTestEnvironment.Controllers
{
    public class HomeController : Controller
    {
        private readonly IProducts _assets;
        private DBModel _context;

        public HomeController(IProducts assets, DBModel context)
        {
            _assets = assets;
            _context = context;
        }

        public IActionResult Index()
        {
            var assetModel = _assets.GetMostViewed();

            return View(assetModel);
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

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