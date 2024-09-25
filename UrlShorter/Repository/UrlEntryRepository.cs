using Microsoft.EntityFrameworkCore;
using UrlShorter.Data;
using UrlShorter.Models;
using UrlShorter.Repository.Interfaces;

namespace UrlShorter.Repository
{
    public class UrlEntryRepository : IUrlEntryRepository
    {

        private readonly ApplicationDbContext _context;

        public UrlEntryRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<bool> AddAsync(UrlEntry url)
        {
            _context.Add(url);
            return await SaveAsync();
        }

        public async Task<bool> DeleteAsync(UrlEntry url)
        {
            _context.Remove(url);
            return await SaveAsync();
        }

        public async Task<IEnumerable<UrlEntry>> GetAllAsync()
        {
            return await _context.UrlEntry.ToListAsync();
        }

        public async Task<UrlEntry> GetByIdAsync(int id)
        {
            var urlEntry = await _context.UrlEntry.FirstOrDefaultAsync(x => x.Id == id);

            if (urlEntry == null)
            {
                throw new KeyNotFoundException($"URL entry with ID {id} not found.");
            }

            return urlEntry;
        }

        public async Task<UrlEntry> GetByLongUrlAsync(string longUrl)
        {
            var urlEntry = await _context.UrlEntry.FirstOrDefaultAsync(x => x.LongUrl == longUrl);

            if (urlEntry == null)
            {
                throw new KeyNotFoundException($"URL entry with Long URL '{longUrl}' not found.");
            }

            return urlEntry;
        }

        public async Task<UrlEntry> GetByShortUrlAsync(string shortUrl)
        {
            var urlEntry = await _context.UrlEntry.FirstOrDefaultAsync(x => x.ShortUrl == shortUrl);

            if (urlEntry == null)
            {
                throw new KeyNotFoundException($"URL entry with Short URL '{shortUrl}' not found.");
            }

            return urlEntry;
        }

        public async Task<bool> IsExistByLongUrlAsync(string longUrl)
        {
            return await _context.UrlEntry.FirstOrDefaultAsync(x => x.LongUrl == longUrl) != null;
        }

        public async Task<bool> IsExistByShortUrlAsync(string shortUrl)
        {
            return await _context.UrlEntry.FirstOrDefaultAsync(x => x.ShortUrl == shortUrl) != null;
        }

        public async Task<bool> SaveAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> UpdateAsync(UrlEntry url)
        {
            var trackedEntity = await _context.UrlEntry.AsNoTracking().FirstOrDefaultAsync(u => u.Id == url.Id);
            if (trackedEntity != null)
            {
                _context.Update(url);
                return await SaveAsync();
            }
            return false;
        }
    }
}
