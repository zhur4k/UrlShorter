using UrlShorter.Dto;
using UrlShorter.Models;

namespace UrlShorter.Services.Interfaces
{
    public interface IUrlEntryService
    {
        Task<IEnumerable<UrlEntry>> GetUrlEntriesToTableAsync();

        Task AddUrlEntryAsync(string longUrl);

        Task UpdateUrlEntryAsync(UrlEntryUpdateDto urlDto);

        Task DeleteUrlEntryAsync(UrlEntry url);

        Task<string> GetLongUrlEntryAsync(string shortUrl);
    }
}
