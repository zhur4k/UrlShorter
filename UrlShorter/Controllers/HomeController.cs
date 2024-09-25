using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using UrlShorter.Models;
using UrlShorter.Services.Interfaces;

namespace UrlShorter.Controllers
{
    public class HomeController : Controller
    {
        private readonly IUrlEntryService _entryService;

        public HomeController(IUrlEntryService entryService)
        {
            _entryService = entryService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View(_entryService.GetUrlEntriesToTableAsync());
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
