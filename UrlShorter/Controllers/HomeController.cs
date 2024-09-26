using Microsoft.AspNetCore.Mvc;
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
        public async Task<IActionResult> Index()
        {
            var urlEntries = await _entryService.GetUrlEntriesToTableAsync();
            return View(urlEntries);
        }
    }
}
