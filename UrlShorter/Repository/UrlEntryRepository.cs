using Microsoft.EntityFrameworkCore;
using UrlShorter.Data;
using UrlShorter.Models;
using UrlShorter.Repository.Interfaces;

namespace UrlShorter.Repository
{
    /// <summary>
    /// Repository for managing URL entries in the database.
    /// </summary>
    public class UrlEntryRepository : IUrlEntryRepository
    {
        private readonly ApplicationDbContext _context;

        /// <summary>
        /// Initializes a new instance of the <see cref="UrlEntryRepository"/> class.
        /// </summary>
        /// <param name="context">The <see cref="ApplicationDbContext"/> instance for database operations.</param>
        public UrlEntryRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Adds a new URL entry to the repository.
        /// </summary>
        /// <param name="url">The <see cref="UrlEntry"/> to add.</param>
        /// <returns>A task that represents the asynchronous add operation. The task result contains a boolean indicating success.</returns>
        public async Task<bool> AddAsync(UrlEntry url)
        {
            _context.Add(url);
            return await SaveAsync();
        }

        /// <summary>
        /// Deletes an existing URL entry from the repository.
        /// </summary>
        /// <param name="url">The <see cref="UrlEntry"/> to delete.</param>
        /// <returns>A task that represents the asynchronous delete operation. The task result contains a boolean indicating success.</returns>
        public async Task<bool> DeleteAsync(UrlEntry url)
        {
            _context.Remove(url);
            return await SaveAsync();
        }

        /// <summary>
        /// Retrieves all URL entries from the repository.
        /// </summary>
        /// <returns>A task that represents the asynchronous get operation. The task result contains a collection of <see cref="UrlEntry"/>.</returns>
        public async Task<IEnumerable<UrlEntry>> GetAllAsync()
        {
            return await _context.UrlEntry.ToListAsync();
        }

        /// <summary>
        /// Retrieves a URL entry by its ID.
        /// </summary>
        /// <param name="id">The ID of the URL entry.</param>
        /// <returns>A task that represents the asynchronous get operation. The task result contains the <see cref="UrlEntry"/> associated with the ID.</returns>
        /// <exception cref="KeyNotFoundException">Thrown when the URL entry with the specified ID is not found.</exception>
        public async Task<UrlEntry> GetByIdAsync(int id)
        {
            var urlEntry = await _context.UrlEntry.FirstOrDefaultAsync(x => x.Id == id);

            if (urlEntry == null)
            {
                throw new KeyNotFoundException($"URL entry with ID {id} not found.");
            }

            return urlEntry;
        }

        /// <summary>
        /// Retrieves a URL entry by its long URL.
        /// </summary>
        /// <param name="longUrl">The long URL to search for.</param>
        /// <returns>A task that represents the asynchronous get operation. The task result contains the <see cref="UrlEntry"/> associated with the long URL.</returns>
        /// <exception cref="KeyNotFoundException">Thrown when the URL entry with the specified long URL is not found.</exception>
        public async Task<UrlEntry> GetByLongUrlAsync(string longUrl)
        {
            var urlEntry = await _context.UrlEntry.FirstOrDefaultAsync(x => x.LongUrl == longUrl);

            if (urlEntry == null)
            {
                throw new KeyNotFoundException($"URL entry with Long URL '{longUrl}' not found.");
            }

            return urlEntry;
        }

        /// <summary>
        /// Retrieves a URL entry by its short URL.
        /// </summary>
        /// <param name="shortUrl">The short URL to search for.</param>
        /// <returns>A task that represents the asynchronous get operation. The task result contains the <see cref="UrlEntry"/> associated with the short URL.</returns>
        /// <exception cref="KeyNotFoundException">Thrown when the URL entry with the specified short URL is not found.</exception>
        public async Task<UrlEntry> GetByShortUrlAsync(string shortUrl)
        {
            var urlEntry = await _context.UrlEntry.FirstOrDefaultAsync(x => x.ShortUrl == shortUrl);

            if (urlEntry == null)
            {
                throw new KeyNotFoundException($"URL entry with Short URL '{shortUrl}' not found.");
            }

            return urlEntry;
        }

        /// <summary>
        /// Checks if a URL entry exists by its long URL.
        /// </summary>
        /// <param name="longUrl">The long URL to check.</param>
        /// <returns>A task that represents the asynchronous existence check. The task result contains a boolean indicating whether the URL entry exists.</returns>
        public async Task<bool> IsExistByLongUrlAsync(string longUrl)
        {
            return await _context.UrlEntry.FirstOrDefaultAsync(x => x.LongUrl == longUrl) != null;
        }

        /// <summary>
        /// Checks if a URL entry exists by its short URL.
        /// </summary>
        /// <param name="shortUrl">The short URL to check.</param>
        /// <returns>A task that represents the asynchronous existence check. The task result contains a boolean indicating whether the URL entry exists.</returns>
        public async Task<bool> IsExistByShortUrlAsync(string shortUrl)
        {
            return await _context.UrlEntry.FirstOrDefaultAsync(x => x.ShortUrl == shortUrl) != null;
        }

        /// <summary>
        /// Saves changes made to the repository.
        /// </summary>
        /// <returns>A task that represents the asynchronous save operation. The task result contains a boolean indicating whether the save operation was successful.</returns>
        public async Task<bool> SaveAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }

        /// <summary>
        /// Updates an existing URL entry in the repository.
        /// </summary>
        /// <param name="url">The <see cref="UrlEntry"/> to update.</param>
        /// <returns>A task that represents the asynchronous update operation. The task result contains a boolean indicating whether the update was successful.</returns>
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
