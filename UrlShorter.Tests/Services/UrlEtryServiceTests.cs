using Moq;
using UrlShorter.Models;
using UrlShorter.Services;
using UrlShorter.Dto;
using UrlShorter.Repository.Interfaces;

namespace UrlShorter.Tests.Services
{
    public class UrlEntryServiceTests
    {
        private readonly Mock<IUrlEntryRepository> _urlEntryRepositoryMock;
        private readonly UrlEntryService _urlEntryService;

        public UrlEntryServiceTests()
        {
            _urlEntryRepositoryMock = new Mock<IUrlEntryRepository>();
            _urlEntryService = new UrlEntryService(_urlEntryRepositoryMock.Object);
        }

        [Fact]
        public async Task AddUrlEntryAsync_ShouldAddUrl_WhenUrlDoesNotExist()
        {
            // Arrange
            var longUrl = "http://example.com";
            _urlEntryRepositoryMock.Setup(r => r.IsExistByLongUrlAsync(longUrl)).ReturnsAsync(false);

            // Act
            await _urlEntryService.AddUrlEntryAsync(longUrl);

            // Assert
            _urlEntryRepositoryMock.Verify(r => r.AddAsync(It.IsAny<UrlEntry>()), Times.Once);
        }

        [Fact]
        public async Task AddUrlEntryAsync_ShouldThrowException_WhenUrlAlreadyExists()
        {
            // Arrange
            var longUrl = "http://example.com";
            _urlEntryRepositoryMock.Setup(r => r.IsExistByLongUrlAsync(longUrl)).ReturnsAsync(true);

            // Act  Assert
            await Assert.ThrowsAsync<InvalidOperationException>(() => _urlEntryService.AddUrlEntryAsync(longUrl));
        }

        [Fact]
        public async Task AddUrlEntryAsync_ShouldThrowException_WhenUrlIsInvalid()
        {
            // Arrange
            var longUrl = "hample.com";

            // Act  Assert
            await Assert.ThrowsAsync<ArgumentException>(() => _urlEntryService.AddUrlEntryAsync(longUrl));
        }

        [Fact]
        public async Task DeleteUrlEntryAsync_ShouldDeleteUrl_WhenUrlExists()
        {
            // Arrange
            var id = 1;
            var urlEntry = new UrlEntry { Id = id, LongUrl = "http://example.com", ShortUrl = "abc123" };
            _urlEntryRepositoryMock.Setup(r => r.GetByIdAsync(id)).ReturnsAsync(urlEntry);

            // Act
            await _urlEntryService.DeleteUrlEntryAsync(id);

            // Assert
            _urlEntryRepositoryMock.Verify(r => r.DeleteAsync(urlEntry), Times.Once);
        }

        [Fact]
        public async Task DeleteUrlEntryAsync_ShouldThrowException_WhenUrlNotFound()
        {
            // Arrange
            var id = 1;
            _urlEntryRepositoryMock.Setup(r => r.GetByIdAsync(id)).ReturnsAsync((UrlEntry)null);

            // Act  Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => _urlEntryService.DeleteUrlEntryAsync(id));
        }

        [Fact]
        public async Task GetLongUrlEntryAsync_ShouldReturnLongUrl_WhenShortUrlExists()
        {
            // Arrange
            var shortUrl = "abc123";
            var longUrl = "http://example.com";
            var urlEntry = new UrlEntry { LongUrl = longUrl, ShortUrl = shortUrl, ClickCount = 0 };

            _urlEntryRepositoryMock.Setup(r => r.GetByShortUrlAsync(shortUrl)).ReturnsAsync(urlEntry);

            // Act
            var result = await _urlEntryService.GetLongUrlEntryAsync(shortUrl);

            // Assert
            Assert.Equal(longUrl, result);
            _urlEntryRepositoryMock.Verify(r => r.UpdateAsync(urlEntry), Times.Once);
        }

        [Fact]
        public async Task GetLongUrlEntryAsync_ShouldThrowException_WhenUrlNotFound()
        {
            // Arrange
            var shortUrl = "abc123";
            var urlEntery = new UrlEntry
            {
                ShortUrl = "abc123",
                LongUrl = null
            };
            _urlEntryRepositoryMock.Setup(r => r.GetByShortUrlAsync(shortUrl)).ReturnsAsync(urlEntery);

            // Act Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => _urlEntryService.GetLongUrlEntryAsync(shortUrl));
        }

        [Fact]
        public async Task GetUrlEntriesToTableAsync_ShouldReturnUrlEntries()
        {
            // Arrange
            var urlEntries = new List<UrlEntry>
            {
                new UrlEntry { Id = 1, LongUrl = "http://example.com", ShortUrl = "abc123" },
                new UrlEntry { Id = 2, LongUrl = "http://example2.com", ShortUrl = "xyz456" }
            };

            _urlEntryRepositoryMock.Setup(r => r.GetAllAsync()).ReturnsAsync(urlEntries);

            // Act
            var result = await _urlEntryService.GetUrlEntriesToTableAsync();

            // Assert
            Assert.Equal(2, result.Count());
        }

        [Fact]
        public async Task UpdateUrlEntryAsync_ShouldUpdateUrl_WhenUrlExists()
        {
            // Arrange
            var urlDto = new UrlEntryUpdateDto { Id = 1, LongUrl = "http://updated.com" };
            var urlEntry = new UrlEntry { Id = 1, LongUrl = "http://example.com", ShortUrl = "abc123" };

            _urlEntryRepositoryMock.Setup(r => r.GetByIdAsync(urlDto.Id)).ReturnsAsync(urlEntry);

            // Act
            await _urlEntryService.UpdateUrlEntryAsync(urlDto);

            // Assert
            Assert.Equal("http://updated.com", urlEntry.LongUrl);
            _urlEntryRepositoryMock.Verify(r => r.UpdateAsync(urlEntry), Times.Once);
        }

        [Fact]
        public async Task UpdateUrlEntryAsync_ShouldThrowException_WhenUrlIsInvalid()
        {
            // Arrange
            var longUrl = "hample.com";

            // Act  Assert
            await Assert.ThrowsAsync<ArgumentException>(() => _urlEntryService.AddUrlEntryAsync(longUrl));
        }

        [Fact]
        public async Task UpdateUrlEntryAsync_ShouldThrowException_WhenDtoIsNull()
        {
            // Act  Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => _urlEntryService.UpdateUrlEntryAsync(null));
        }
    }
}