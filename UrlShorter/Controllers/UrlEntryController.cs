using Microsoft.AspNetCore.Mvc;
using UrlShorter.Services.Interfaces;

namespace UrlShorter.Controllers
{
    [Route("url")]
    public class UrlEntryController : Controller
    {
        private readonly IUrlEntryService _urlEntryService;

        public UrlEntryController(IUrlEntryService urlEntryService)
        {
            _urlEntryService = urlEntryService;
        }
        [HttpGet]
        [Route("{shortUrl}")]
        public async Task<IActionResult> UrlEntryRedirect(string shortUrl) 
        {
            var longUrl = await _urlEntryService.GetLongUrlEntryAsync(shortUrl);

            if (string.IsNullOrEmpty(longUrl))
            {
                return NotFound("URL not found.");
            }

            return Redirect(longUrl);
        }
    }
}
