using UrlShorter.Dto;
using UrlShorter.Models;

namespace UrlShorter.Services.Interfaces
{
    /// <summary>
    /// Interface for URL entry services, providing methods for managing URL entries.
    /// </summary>
    public interface IUrlEntryService
    {
        /// <summary>
        /// Retrieves all URL entries for display in a table format.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation. The task result contains a collection of <see cref="UrlEntry"/>.</returns>
        Task<IEnumerable<UrlEntry>> GetUrlEntriesToTableAsync();

        /// <summary>
        /// Adds a new URL entry with the specified long URL.
        /// </summary>
        /// <param name="longUrl">The long URL to add.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task AddUrlEntryAsync(string longUrl);

        /// <summary>
        /// Updates an existing URL entry.
        /// </summary>
        /// <param name="urlDto">The <see cref="UrlEntryUpdateDto"/> containing the updated URL entry information.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task UpdateUrlEntryAsync(UrlEntryUpdateDto urlDto);

        /// <summary>
        /// Deletes a URL entry by its ID.
        /// </summary>
        /// <param name="id">The ID of the URL entry to delete.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task DeleteUrlEntryAsync(int id);

        /// <summary>
        /// Retrieves the long URL converted to the short URL.
        /// </summary>
        /// <param name="shortUrl">The short URL to resolve.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the long URL.</returns>
        Task<string> GetLongUrlEntryAsync(string shortUrl);
    }
}
