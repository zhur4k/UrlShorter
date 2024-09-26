using Microsoft.AspNetCore.Mvc;
using Moq;
using UrlShorter.Controllers;
using UrlShorter.Dto;
using UrlShorter.Models;
using UrlShorter.Services.Interfaces;

namespace UrlShorter.Tests.Controller
{
    public class UrlEntryControllerTests
    {
        private readonly Mock<IUrlEntryService> _urlEntryServiceMok;
        private readonly UrlEntryController _urlEntryController;

        public UrlEntryControllerTests()
        {
            _urlEntryServiceMok = new Mock<IUrlEntryService>();
            _urlEntryController = new UrlEntryController(_urlEntryServiceMok.Object);
        }

        [Fact]
        public async Task GetAllUrlEntry_ReturnsOkResult_WithUrlEntries()
        {
            // Arrange
            var mockUrlEntries = new List<UrlEntry>
            {
                new UrlEntry
                {
                    Id = 1,
                    ShortUrl = "abc123",
                    LongUrl = "http://example.com",
                    ClickCount = 5
                }
            };

            _urlEntryServiceMok.Setup(service => service.GetUrlEntriesToTableAsync()).ReturnsAsync(mockUrlEntries);

            // Act
            var result = await _urlEntryController.GetAllUrlEntry();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var resultValue = Assert.IsType<List<UrlEntry>>(okResult.Value);
            Assert.Single(resultValue);
        }
        [Fact]
        public async Task GetAllUrlEntry_ReturnsInternalServerError_OnException()
        {
            // Arrange
            _urlEntryServiceMok.Setup(service => service.GetUrlEntriesToTableAsync()).ThrowsAsync(new Exception("Database error"));

            // Act
            var result = await _urlEntryController.GetAllUrlEntry();

            // Assert
            var statusCodeResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, statusCodeResult.StatusCode);
            Assert.Equal("Internal Server Error: Database error", statusCodeResult.Value);
        }

        [Fact]
        public async Task UrlEntryRedirect_ReturnsRedirectResult_WithLongUrl()
        {
            // Arrange
            var shortUrl = "abc123";
            var longUrl = "http://example.com";

            _urlEntryServiceMok.Setup(service => service.GetLongUrlEntryAsync(shortUrl))
                               .ReturnsAsync(longUrl);

            // Act
            var result = await _urlEntryController.UrlEntryRedirect(shortUrl);

            // Assert
            var redirectResult = Assert.IsType<RedirectResult>(result);
            Assert.Equal(longUrl, redirectResult.Url);  // Убедитесь, что редирект ведет на правильный URL
        }

        [Fact]
        public async Task UrlEntryRedirect_ReturnsNotFound_WhenUrlNotFound()
        {
            // Arrange
            var shortUrl = "abc123";

            _urlEntryServiceMok.Setup(service => service.GetLongUrlEntryAsync(shortUrl))
                               .ThrowsAsync(new ArgumentNullException());

            // Act
            var result = await _urlEntryController.UrlEntryRedirect(shortUrl);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
        }

        [Fact]
        public async Task Create_ReturnsCreatedStatus_WhenUrlCreated()
        {
            // Arrange
            var urlDto = new UrlEntryCreateDto { LongUrl = "http://example.com" };

            _urlEntryServiceMok.Setup(service => service.AddUrlEntryAsync(urlDto.LongUrl))
                               .Returns(Task.CompletedTask);

            // Act
            var result = await _urlEntryController.Create(urlDto);

            // Assert
            var statusCodeResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(201, statusCodeResult.StatusCode);
        }

        [Fact]
        public async Task Create_ReturnsInternalServerError_OnException()
        {
            // Arrange
            var urlDto = new UrlEntryCreateDto { LongUrl = "http://example.com" };

            _urlEntryServiceMok.Setup(service => service.AddUrlEntryAsync(urlDto.LongUrl))
                               .ThrowsAsync(new Exception("Database error"));

            // Act
            var result = await _urlEntryController.Create(urlDto);

            // Assert
            var statusCodeResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, statusCodeResult.StatusCode);  // Убедитесь, что вернулся статус 500
            Assert.Equal("Internal Server Error: Database error", statusCodeResult.Value);
        }

        [Fact]
        public async Task Edit_ReturnsSuccessStatus_WhenUrlEdited()
        {
            // Arrange
            var urlDto = new UrlEntryUpdateDto { Id = 1, LongUrl = "http://newexample.com" };

            _urlEntryServiceMok.Setup(service => service.UpdateUrlEntryAsync(urlDto))
                               .Returns(Task.CompletedTask);

            // Act
            var result = await _urlEntryController.Edit(urlDto);

            // Assert
            var statusCodeResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(201, statusCodeResult.StatusCode);
        }

        [Fact]
        public async Task Edit_ReturnsInternalServerError_OnException()
        {
            // Arrange
            var urlDto = new UrlEntryUpdateDto { Id = 1, LongUrl = "http://newexample.com" };

            _urlEntryServiceMok.Setup(service => service.UpdateUrlEntryAsync(urlDto))
                               .ThrowsAsync(new Exception("Database error"));

            // Act
            var result = await _urlEntryController.Edit(urlDto);

            // Assert
            var statusCodeResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, statusCodeResult.StatusCode);
            Assert.Equal("Internal Server Error: Database error", statusCodeResult.Value);
        }

        [Fact]
        public async Task Delete_ReturnsSuccessStatus_WhenUrlDeleted()
        {
            // Arrange
            var id = 1;

            _urlEntryServiceMok.Setup(service => service.DeleteUrlEntryAsync(id))
                               .Returns(Task.CompletedTask);

            // Act
            var result = await _urlEntryController.Delete(id);

            // Assert
            var objectResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(201, objectResult.StatusCode);
            Assert.Equal("The URL was deleted successfully.", objectResult.Value);
        }

        [Fact]
        public async Task Delete_ReturnsInternalServerError_OnException()
        {
            // Arrange
            var id = 1;

            _urlEntryServiceMok.Setup(service => service.DeleteUrlEntryAsync(id))
                               .ThrowsAsync(new Exception("Database error"));

            // Act
            var result = await _urlEntryController.Delete(id);

            // Assert
            var statusCodeResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, statusCodeResult.StatusCode);
            Assert.Equal("Internal Server Error: Database error", statusCodeResult.Value);
        }
    }
}
