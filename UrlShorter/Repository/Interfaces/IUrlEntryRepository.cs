using UrlShorter.Models;

namespace UrlShorter.Repository.Interfaces
{
    public interface IUrlEntryRepository
    {
        Task<IEnumerable<UrlEntry>> GetAllAsync();

        Task<UrlEntry> GetByLongUrlAsync(string longUrl);

        Task<UrlEntry> GetByShortUrlAsync(string longUrl);

        Task<bool> AddAsync(UrlEntry url);

        Task<bool> IsExistByShortUrlAsync(string shortUrl);

        Task<bool> IsExistByLongUrlAsync(string longUrl);

        Task<bool> UpdateAsync(UrlEntry url);

        Task<bool> DeleteAsync(UrlEntry url);

        Task<bool> SaveAsync();

        Task<UrlEntry> GetByIdAsync(int id);
    }
}
