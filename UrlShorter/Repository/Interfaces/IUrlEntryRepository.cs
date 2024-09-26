using UrlShorter.Models;

namespace UrlShorter.Repository.Interfaces
{
    /// <summary>
    /// Interface for managing URL entries in the repository.
    /// </summary>
    public interface IUrlEntryRepository
    {
        /// <summary>
        /// Retrieves all URL entries.
        /// </summary>
        /// <returns>A collection of <see cref="UrlEntry"/> objects.</returns>
        Task<IEnumerable<UrlEntry>> GetAllAsync();

        /// <summary>
        /// Retrieves a URL entry by long URL.
        /// </summary>
        /// <param name="longUrl">The long URL to search for.</param>
        /// <returns>The <see cref="UrlEntry"/> associated with the long URL.</returns>
        Task<UrlEntry> GetByLongUrlAsync(string longUrl);

        /// <summary>
        /// Retrieves a URL entry by short URL.
        /// </summary>
        /// <param name="shortUrl">The short URL to search for.</param>
        /// <returns>The <see cref="UrlEntry"/> associated with the short URL.</returns>
        Task<UrlEntry> GetByShortUrlAsync(string shortUrl);

        /// <summary>
        /// Adds a new URL entry.
        /// </summary>
        /// <param name="url">The <see cref="UrlEntry"/> to add.</param>
        /// <returns>A boolean indicating whether the addition was successful.</returns>
        Task<bool> AddAsync(UrlEntry url);

        /// <summary>
        /// Checks if a URL entry exists by short URL.
        /// </summary>
        /// <param name="shortUrl">The short URL to check.</param>
        /// <returns>A boolean indicating whether the URL entry exists.</returns>
        Task<bool> IsExistByShortUrlAsync(string shortUrl);

        /// <summary>
        /// Checks if a URL entry exists by long URL.
        /// </summary>
        /// <param name="longUrl">The long URL to check.</param>
        /// <returns>A boolean indicating whether the URL entry exists.</returns>
        Task<bool> IsExistByLongUrlAsync(string longUrl);

        /// <summary>
        /// Updates an existing URL entry in the repository.
        /// </summary>
        /// <param name="url">The <see cref="UrlEntry"/> to update.</param>
        /// <returns>A boolean indicating whether the update was successful.</returns>
        Task<bool> UpdateAsync(UrlEntry url);

        /// <summary>
        /// Deletes a URL entry from the repository.
        /// </summary>
        /// <param name="url">The <see cref="UrlEntry"/> to delete.</param>
        /// <returns>A boolean indicating whether the deletion was successful.</returns>
        Task<bool> DeleteAsync(UrlEntry url);

        /// <summary>
        /// Saves changes to the repository.
        /// </summary>
        /// <returns>A boolean indicating whether the save operation was successful.</returns>
        Task<bool> SaveAsync();

        /// <summary>
        /// Retrieves a URL entry by ID.
        /// </summary>
        /// <param name="id">The ID of the URL entry.</param>
        /// <returns>The <see cref="UrlEntry"/> associated with the ID.</returns>
        Task<UrlEntry> GetByIdAsync(int id);
    }
}
