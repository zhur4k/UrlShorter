using UrlShorter.Dto;
using UrlShorter.Models;

namespace UrlShorter.Services.Interfaces
{
    public interface IUrlEntryService
    {
        Task<IEnumerable<UrlEntryTableDto>> GetUrlEntriesToTableAsync();

        Task<bool> AddUrlEntryAsync(UrlEntry url);

        Task<bool> UpdateUrlEntryAsync(UrlEntry url);

        Task<bool> DeleteUrlEntryAsync(UrlEntry url);

        Task<bool> registerClickAsync(UrlEntry url);

        Task<bool> getLongUrlEntryAsync(UrlEntry url);
    }
}
