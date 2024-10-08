using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using UrlShortener.Api;
using UrlShortener.Api.Endpoints;
using UrlShortener.Api.Entities;
using UrlShortener.Api.Models;
using UrlShortener.Api.Services;

namespace UrlShortener.Test;

public class EndpointDefinitionsTests
{
    [Fact]
    public async Task ShortenUrl_WithValidUrl_ShouldReturnShortUrl()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDb")
            .Options;

        await using var context = new ApplicationDbContext(options);
        var urlShorteningService = new UrlShorteningService(context);

        var httpContext = new DefaultHttpContext();
        httpContext.Request.Scheme = "https";
        httpContext.Request.Host = new HostString("example.com");

        var request = new ShortenUrlRequest { Url = "https://www.example.com/long-url" };

        // Act
        var result = await EndpointDefinitions.ShortenUrl(request, urlShorteningService, context, httpContext);

        // Assert
        result.Should().BeOfType<Ok<string>>();
        var okResult = (Ok<string>)result;
        okResult.Value.Should().StartWith("https://example.com/api/");
    }

    [Fact]
    public async Task ShortenUrl_WithInvalidUrl_ShouldReturnBadRequest()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDb")
            .Options;

        await using var context = new ApplicationDbContext(options);
        var urlShorteningService = new UrlShorteningService(context);

        var httpContext = new DefaultHttpContext();
        var request = new ShortenUrlRequest { Url = "invalid-url" };

        // Act
        var result = await EndpointDefinitions.ShortenUrl(request, urlShorteningService, context, httpContext);

        // Assert
        result.Should().BeOfType<BadRequest<string>>();
    }

    [Fact]
    public async Task RedirectToLongUrl_WithValidCode_ShouldRedirect()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDb")
            .Options;

        await using var context = new ApplicationDbContext(options);
        var code = "ABCDEFG";
        var longUrl = "https://www.example.com/long-url";
        context.ShortenedUrls.Add(new ShortenedUrl { Code = code, LongUrl = longUrl });
        await context.SaveChangesAsync();

        // Act
        var result = await EndpointDefinitions.RedirectToLongUrl(code, context);

        // Assert
        result.Should().BeOfType<RedirectHttpResult>();
        var redirectResult = (RedirectHttpResult)result;
        redirectResult.Url.Should().Be(longUrl);
    }

    [Fact]
    public async Task RedirectToLongUrl_WithInvalidCode_ShouldReturnNotFound()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDb")
            .Options;

        await using var context = new ApplicationDbContext(options);

        // Act
        var result = await EndpointDefinitions.RedirectToLongUrl("INVALID", context);

        // Assert
        result.Should().BeOfType<NotFound>();
    }
}