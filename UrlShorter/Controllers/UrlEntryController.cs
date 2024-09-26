using Microsoft.AspNetCore.Mvc;
using UrlShorter.Dto;
using UrlShorter.Services.Interfaces;

namespace UrlShorter.Controllers
{
    /// <summary>
    /// Controller for for managing URL entries.
    /// </summary>
    [Route("url")]
    public class UrlEntryController : Controller
    {
        private readonly IUrlEntryService _urlEntryService;

        public UrlEntryController(IUrlEntryService urlEntryService)
        {
            _urlEntryService = urlEntryService;
        }

        /// <summary>
        /// Gets all URL entries.
        /// </summary>
        /// <returns>A list of URL entries.</returns>
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

        /// <summary>
        /// Redirect to the long URL.
        /// </summary>
        /// <param name="shortUrl">The short URl to redirect from.</param>
        /// <returns>Long URL.</returns>
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

        /// <summary>
        /// Creates a new URL entry.
        /// </summary>
        /// <param name="urlDto">The dto with long URL to create.</param>
        /// <returns>A status indicating the result of the creation.</returns>
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

        /// <summary>
        /// Edits an existing URL entry.
        /// </summary>
        /// <param name="urlDto">The data transfer object containing the updated URL information.</param>
        /// <returns>A status indicating the result of the update.</returns>
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

        /// <summary>
        /// Deletes a URL entry by ID.
        /// </summary>
        /// <param name="id">The ID of the URL entry to delete.</param>
        /// <returns>A status indicating the result of the deletion.</returns>
        [HttpPost]
        [Route("delete/{id}")]
        public async Task<IActionResult> Delete(int id)
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
