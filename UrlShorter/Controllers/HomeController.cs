using Microsoft.AspNetCore.Mvc;

namespace UrlShorter.Controllers
{
    public class HomeController : Controller
    {

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }
    }
}
