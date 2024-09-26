using Microsoft.AspNetCore.Mvc;

namespace UrlShorter.Controllers
{
    /// <summary>
    /// Controller for handling requests related to the home page.
    /// </summary>
    public class HomeController : Controller
    {
        /// <summary>
        /// Displays the main page.
        /// </summary>
        /// <returns>A view representing the main page.</returns>
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }
    }
}
