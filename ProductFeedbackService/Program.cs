using ProductFeedbackService.Infrastructure;
using Microsoft.EntityFrameworkCore;
using ProductFeedbackService.Domain.Models;
using ProductFeedbackService.Infrastructure.Services;
using ProductFeedbackService.Domain.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContextFactory<AppDbContext>(options => options.UseInMemoryDatabase("InMemoryDb"));
builder.Services.AddScoped(sp => sp.GetRequiredService<IDbContextFactory<AppDbContext>>().CreateDbContext());
builder.Services.AddSingleton<IRatingCalculator, RatingCalculator>();
builder.Services.AddHostedService<RatingsJob>();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

    if (!db.WordRatings.Any())
    {
        db.WordRatings.AddRange(
            new WordRating { Phrase = "excellent".ToLowerInvariant(), Score = 5 },
            new WordRating { Phrase = "good".ToLowerInvariant(), Score = 4 },
            new WordRating { Phrase = "average".ToLowerInvariant(), Score = 3 },
            new WordRating { Phrase = "bad".ToLowerInvariant(), Score = 2 },
            new WordRating { Phrase = "very bad".ToLowerInvariant(), Score = 1 }
        );
        db.SaveChanges();
    }
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();