using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Triade.Models;

namespace Triade.Controllers
{
    public class RelatoriosController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public RelatoriosController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
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