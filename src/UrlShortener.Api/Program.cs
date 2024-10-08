using Microsoft.EntityFrameworkCore;
using UrlShortener.Api;
using UrlShortener.Api.Extensions;
using UrlShortener.Api.Services;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<ApplicationDbContext>(o =>
    o.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<UrlShorteningService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();

    app.ApplyMigrations();
}

app.MapEndpoints();

app.Run();


// app.MapPost("api/shorten", async (ShortenUrlRequest request, UrlShorteningService urlShorteningService, ApplicationDbContext dbContext, HttpContext httpContext) =>
// {
//     if(Uri.TryCreate(request.Url, UriKind.Absolute, out _))
//     {
//         return Results.BadRequest("The specified URL is invalid.");
//     }
//
//     var code = await urlShorteningService.GenerateUniquecode();
//
//     var shortenedUrl = new ShortenedUrl
//     {
//         Id = Guid.NewGuid(),
//         LongUrl = request.Url,
//         Code = code,
//         ShortUrl = $"{httpContext.Request.Scheme} ://{httpContext.Request.Host}/api/{code}",
//         CreatedOnUtc = DateTime.Now
//     };
//     
//
//     dbContext.ShortenedUrls.Add(shortenedUrl);
//
//     await dbContext.SaveChangesAsync();
//
//     return Results.Ok(shortenedUrl.ShortUrl);
//
// });
//
// app.MapGet("api/{code}", async (string code, ApplicationDbContext dbContext) =>
// {
//     var shortenedUrl = await dbContext.ShortenedUrls
//             .FirstOrDefaultAsync(s => s.Code == code);
//
//     if (shortenedUrl is null)
//         return Results.NotFound();
//
//     return Results.Redirect(shortenedUrl.LongUrl);
//
// });


