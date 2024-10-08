using Microsoft.EntityFrameworkCore;
using UrlShortener.Api.Entities;
using UrlShortener.Api.Models;
using UrlShortener.Api.Services;

namespace UrlShortener.Api.Endpoints;

public static class EndpointDefinitions
{
    public static void MapEndpoints(this WebApplication app)
    {
        app.MapPost("api/shorten", ShortenUrl);
        app.MapGet("api/{code}", RedirectToLongUrl);
    }

    public static async Task<IResult> ShortenUrl(
        ShortenUrlRequest request,
        UrlShorteningService urlShorteningService,
        ApplicationDbContext dbContext,
        HttpContext httpContext)
    {
        if (!Uri.TryCreate(request.Url, UriKind.Absolute, out _))
        {
            return Results.BadRequest("The specified URL is invalid.");
        }

        var code = await urlShorteningService.GenerateUniquecode();

        var shortenedUrl = new ShortenedUrl
        {
            Id = Guid.NewGuid(),
            LongUrl = request.Url,
            Code = code,
            ShortUrl = $"{httpContext.Request.Scheme}://{httpContext.Request.Host}/api/{code}",
            CreatedOnUtc = DateTime.UtcNow
        };

        dbContext.ShortenedUrls.Add(shortenedUrl);
        await dbContext.SaveChangesAsync();

        return Results.Ok(shortenedUrl.ShortUrl);
    }

    public static async Task<IResult> RedirectToLongUrl(string code, ApplicationDbContext dbContext)
    {
        var shortenedUrl = await dbContext.ShortenedUrls
            .FirstOrDefaultAsync(s => s.Code == code);

        if (shortenedUrl is null)
            return Results.NotFound();

        return Results.Redirect(shortenedUrl.LongUrl);
    }
}
