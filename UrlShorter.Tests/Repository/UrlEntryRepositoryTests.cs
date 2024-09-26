using Microsoft.EntityFrameworkCore;
using UrlShorter.Data;
using UrlShorter.Models;
using UrlShorter.Repository.Interfaces;
using UrlShorter.Repository;

namespace UrlShorter.Tests.Repository
{
    public class UrlEntryRepositoryTests
    {
        private readonly DbContextOptions<ApplicationDbContext> _dbContextOptions;
        private UrlEntry _urlEntry;

        public UrlEntryRepositoryTests()
        {
            _dbContextOptions = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "UrlShorterTestDb")
                .Options;
            _urlEntry = new UrlEntry { LongUrl = "http://example.com", ShortUrl = "abc123" };
        }

        private async Task<IUrlEntryRepository> CreateRepository()
        {
            var context = new ApplicationDbContext(_dbContextOptions);
            await context.Database.EnsureDeletedAsync();
            await context.Database.EnsureCreatedAsync();
            return new UrlEntryRepository(context);
        }

        [Fact]
        public async Task AddAsync_ShouldAddUrlEntry()
        {
            // Arrange
            var urlEntryRepository = await CreateRepository();

            // Act
            var result = await urlEntryRepository.AddAsync(_urlEntry);

            // Assert
            Assert.True(result);
            Assert.Equal(1, (await urlEntryRepository.GetAllAsync()).Count());
        }

        [Fact]
        public async Task DeleteAsync_ShouldRemoveUrlEntry()
        {
            // Arrange
            var urlEntryRepository = await CreateRepository();
            await urlEntryRepository.AddAsync(_urlEntry);

            // Act
            var result = await urlEntryRepository.DeleteAsync(_urlEntry);

            // Assert
            Assert.True(result);
            Assert.Empty(await urlEntryRepository.GetAllAsync());
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnUrlEntry_WhenExists()
        {
            // Arrange
            var urlEntryRepository = await CreateRepository();
            await urlEntryRepository.AddAsync(_urlEntry);

            // Act
            var result = await urlEntryRepository.GetByIdAsync(_urlEntry.Id);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(_urlEntry.LongUrl, result.LongUrl);
        }

        [Fact]
        public async Task GetByIdAsync_ShouldThrowKeyNotFoundException_WhenNotExists()
        {
            // Arrange
            var urlEntryRepository = await CreateRepository();
            // Act Assert
            await Assert.ThrowsAsync<KeyNotFoundException>(() => urlEntryRepository.GetByIdAsync(99));
        }

        [Fact]
        public async Task IsExistByLongUrlAsync_ShouldReturnTrue_WhenExists()
        {
            // Arrange
            var urlEntryRepository = await CreateRepository();
            await urlEntryRepository.AddAsync(_urlEntry);

            // Act
            var exists = await urlEntryRepository.IsExistByLongUrlAsync(_urlEntry.LongUrl);

            // Assert
            Assert.True(exists);
        }

        [Fact]
        public async Task IsExistByLongUrlAsync_ShouldReturnFalse_WhenNotExists()
        {
            // Arrange
            var urlEntryRepository = await CreateRepository();

            // Act
            var exists = await urlEntryRepository.IsExistByLongUrlAsync("http://nonexistenturl.com");

            // Assert
            Assert.False(exists);
        }
    }
}
