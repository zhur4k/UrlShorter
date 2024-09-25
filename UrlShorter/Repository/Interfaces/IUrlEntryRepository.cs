using UrlShorter.Models;

namespace UrlShorter.Repository.Interfaces
{
    public interface IUrlEntryRepository
    {
        Task<IEnumerable<UrlEntry>> GetAllAsync();

        Task<bool> AddAsync(UrlEntry url);

        Task<bool> UpdateAsync(UrlEntry url);

        Task<bool> DeleteAsync(UrlEntry url);

        Task<bool> SaveAsync();
    }
}
