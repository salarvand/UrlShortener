using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using UrlShortener.Api;
using UrlShortener.Api.Entities;
using UrlShortener.Api.Services;

namespace UrlShortener.Test;

public class UrlShorteningServiceTests
{
    [Fact]
    public async Task GenerateUniqueCode_ShouldReturnUniqueCode()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDb")
            .Options;

        await using var context = new ApplicationDbContext(options);
        var service = new UrlShorteningService(context);

        // Act
        var code = await service.GenerateUniquecode();

        // Assert
        code.Should().NotBeNullOrEmpty();
        code.Length.Should().Be(UrlShorteningService.NumberOfCharsInShortLink);
    }

    [Fact]
    public async Task GenerateUniqueCode_ShouldRetryIfCodeExists()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDb")
            .Options;

        await using var context = new ApplicationDbContext(options);
        var existingCode = "ABCDEFG";
        context.ShortenedUrls.Add(new ShortenedUrl { Code = existingCode });
        await context.SaveChangesAsync();

        var service = new UrlShorteningService(context);

        // Act
        var code = await service.GenerateUniquecode();

        // Assert
        code.Should().NotBe(existingCode);
        code.Length.Should().Be(UrlShorteningService.NumberOfCharsInShortLink);
    }
}