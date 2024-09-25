using System.Security.Cryptography;
using System.Text;
using UrlShorter.Dto;
using UrlShorter.Models;
using UrlShorter.Repository.Interfaces;
using UrlShorter.Services.Interfaces;

namespace UrlShorter.Services
{
    public class UrlEntryService : IUrlEntryService
    {

        private readonly IUrlEntryRepository _urlEntryRepository;

        public UrlEntryService(IUrlEntryRepository urlEntryRepository)
        {
            _urlEntryRepository = urlEntryRepository;
        }

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

        public async Task DeleteUrlEntryAsync(UrlEntry url)
        {
            if (url == null)
            {
                throw new ArgumentNullException(nameof(url), "The URL entry cannot be null.");
            }

            await _urlEntryRepository.DeleteAsync(url);
        }

        public async Task<string> GetLongUrlEntryAsync(string shortUrl)
        {
            var url = await _urlEntryRepository.GetByShortUrlAsync(shortUrl);
            await RegisterClickAsync(url);
            return url.LongUrl;
        }

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

        public async Task<IEnumerable<UrlEntry>> GetUrlEntriesToTableAsync()
        {
            return await _urlEntryRepository.GetAllAsync();
        }

        public async Task RegisterClickAsync(UrlEntry url)
        {
            url.ClickCount++;

            await _urlEntryRepository.UpdateAsync(url);
        }

        public async Task UpdateUrlEntryAsync(UrlEntryUpdateDto urlDto)
        {
            if (urlDto == null)
            {
                throw new ArgumentNullException(nameof(urlDto), "The URL entry cannot be null.");
            }
            var url = await _urlEntryRepository.GetByIdAsync(urlDto.Id);

            url.ShortUrl = await GenerateShortUrlEntryAsync(url.LongUrl);

            await _urlEntryRepository.UpdateAsync(url);
        }
    }
}
