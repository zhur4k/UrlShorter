using System.Security.Cryptography;
using System.Text;
using UrlShorter.Dto;
using UrlShorter.Models;
using UrlShorter.Repository.Interfaces;
using UrlShorter.Services.Interfaces;

namespace UrlShorter.Services
{
    /// <summary>
    /// Service for managing URL entries.
    /// </summary>
    public class UrlEntryService : IUrlEntryService
    {
        private readonly IUrlEntryRepository _urlEntryRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="UrlEntryService"/> class.
        /// </summary>
        /// <param name="urlEntryRepository">The repository used for accessing URL entries.</param>
        public UrlEntryService(IUrlEntryRepository urlEntryRepository)
        {
            _urlEntryRepository = urlEntryRepository;
        }

        /// <summary>
        /// Adds a new URL entry.
        /// </summary>
        /// <param name="longUrl">The long URL to be added.</param>
        /// <exception cref="InvalidOperationException">Thrown when the URL already exists in the database.</exception>
        public async Task AddUrlEntryAsync(string longUrl)
        {
            if (await _urlEntryRepository.IsExistByLongUrlAsync(longUrl))
            {
                throw new InvalidOperationException("The URL already exists in the database.");
            }

            var url = new UrlEntry
            {
                LongUrl = longUrl,
                ShortUrl = await GenerateShortUrlEntryAsync(longUrl),
                ClickCount = 0,
                CreatedDate = DateTime.Now
            };

            await _urlEntryRepository.AddAsync(url);
        }

        /// <summary>
        /// Deletes a URL entry by ID.
        /// </summary>
        /// <param name="id">The ID of the URL entry to delete.</param>
        /// <exception cref="ArgumentNullException">Thrown when no URL entry is found with the specified ID.</exception>
        public async Task DeleteUrlEntryAsync(int id)
        {
            var url = await _urlEntryRepository.GetByIdAsync(id);

            if (url == null)
            {
                throw new ArgumentNullException(nameof(id), $"URL entry with ID {id} not found.");
            }

            await _urlEntryRepository.DeleteAsync(url);
        }

        /// <summary>
        /// Retrieves the long URL converted to the specified short URL.
        /// </summary>
        /// <param name="shortUrl">The short URL to resolve.</param>
        /// <returns>The corresponding long URL.</returns>
        /// <exception cref="ArgumentNullException">Thrown when the URL is not found.</exception>
        public async Task<string> GetLongUrlEntryAsync(string shortUrl)
        {
            var url = await _urlEntryRepository.GetByShortUrlAsync(shortUrl);

            if (string.IsNullOrEmpty(url.LongUrl))
            {
                throw new ArgumentNullException("URL not found.");
            }

            await RegisterClickAsync(url);
            return url.LongUrl;
        }

        /// <summary>
        /// Generates a unique short URL based on the provided long URL.
        /// </summary>
        /// <param name="longUrl">The long URL for which to generate a short URL.</param>
        /// <returns>A task representing the asynchronous operation. The task result contains the generated short URL.</returns>
        private async Task<string> GenerateShortUrlEntryAsync(string longUrl)
        {
            using (var sha256 = SHA256.Create())
            {
                string shortUrl;
                do
                {
                    var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(longUrl));
                    shortUrl = BitConverter.ToString(bytes).Replace("-", "").Substring(0, 6).ToLower();
                }
                while (await _urlEntryRepository.IsExistByShortUrlAsync(shortUrl));

                return shortUrl;
            }
        }

        /// <summary>
        /// Retrieves all URL entries for display in a table format.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation. The task result contains a collection of <see cref="UrlEntry"/>.</returns>
        public async Task<IEnumerable<UrlEntry>> GetUrlEntriesToTableAsync()
        {
            return await _urlEntryRepository.GetAllAsync();
        }

        /// <summary>
        /// Registers a click on a URL entry, incrementing its click count.
        /// </summary>
        /// <param name="url">The URL entry to register the click for.</param>
        public async Task RegisterClickAsync(UrlEntry url)
        {
            url.ClickCount++;

            await _urlEntryRepository.UpdateAsync(url);
        }

        /// <summary>
        /// Updates an existing URL entry with the provided information.
        /// </summary>
        /// <param name="urlDto">The <see cref="UrlEntryUpdateDto"/> containing the updated URL entry information.</param>
        /// <exception cref="ArgumentNullException">Thrown when the provided URL entry DTO is null.</exception>
        public async Task UpdateUrlEntryAsync(UrlEntryUpdateDto urlDto)
        {
            if (urlDto == null)
            {
                throw new ArgumentNullException(nameof(urlDto), "The URL entry cannot be null.");
            }
            var url = await _urlEntryRepository.GetByIdAsync(urlDto.Id);

            url.LongUrl = urlDto.LongUrl;

            await _urlEntryRepository.UpdateAsync(url);
        }
    }
}
