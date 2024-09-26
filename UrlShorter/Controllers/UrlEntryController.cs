using Microsoft.AspNetCore.Mvc;
using UrlShorter.Dto;
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
        [Route("getAll")]
        public async Task<IActionResult> GetAllUrlEntry()
        {
            try
            {
                var urlEntries = await _urlEntryService.GetUrlEntriesToTableAsync();
                return Ok(urlEntries);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal Server Error: " + ex.Message);
            }
        }

        [HttpGet]
        [Route("{shortUrl}")]
        public async Task<IActionResult> UrlEntryRedirect(string shortUrl) 
        {
            try
            {
                var longUrl = await _urlEntryService.GetLongUrlEntryAsync(shortUrl);

                return Redirect(longUrl);
            }
            catch (ArgumentNullException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal Server Error: " + ex.Message);
            }
        }

        [HttpPost]
        [Route("create")]
        public async Task<IActionResult> Create([FromBody] UrlEntryCreateDto urlDto)
        {
            try
            {
                await _urlEntryService.AddUrlEntryAsync(urlDto.LongUrl);

                return StatusCode(201, "The URL was created successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal Server Error: " + ex.Message);
            }
        }

        [HttpPut]
        [Route("edit")]
        public async Task<IActionResult> Edit([FromBody] UrlEntryUpdateDto urlDto)
        {
            try
            {
                await _urlEntryService.UpdateUrlEntryAsync(urlDto);

                return StatusCode(201, "The URL was edited successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal Server Error: " + ex.Message);
            }
        }

        [HttpPost]
        [Route("delete/{id}")]
        public async Task<IActionResult> Deletet(int id)
        {
            try
            {
                await _urlEntryService.DeleteUrlEntryAsync(id);

                return StatusCode(201, "The URL was deleted successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal Server Error: " + ex.Message);
            }
        }
    }
}
