using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Triade.Models;

namespace Triade.Controllers
{
    public class ProdutosController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public ProdutosController(ILogger<HomeController> logger)
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