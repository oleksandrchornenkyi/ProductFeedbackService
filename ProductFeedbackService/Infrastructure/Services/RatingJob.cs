namespace ProductFeedbackService.Infrastructure.Services;

using Microsoft.EntityFrameworkCore;
using ProductFeedbackService.Domain.Services;
using ProductFeedbackService.Domain.Models;

public class RatingsJob : BackgroundService
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly IRatingCalculator _ratingCalculator;
    public RatingsJob(IServiceScopeFactory scopeFactory, IRatingCalculator ratingCalculator)
    {
        _scopeFactory = scopeFactory;
        _ratingCalculator = ratingCalculator;
    }
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var timer = new PeriodicTimer(TimeSpan.FromMinutes(1));

        while (await timer.WaitForNextTickAsync(stoppingToken))
        {
            using var scope = _scopeFactory.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            var dictionary = await db.WordRatings.ToListAsync(stoppingToken);
            var feedbacks = await db.Feedbacks.ToListAsync(stoppingToken);

            var groups = feedbacks.GroupBy(f => f.ProductId);

            foreach (var group in groups)
            {
                var scores = group
                    .Select(f => _ratingCalculator.CalculateReviewScore(f.ReviewText, dictionary))
                    .ToList();

                var average = scores.Count == 0 ? 0.0 : scores.Average();

                var rating = await db.ProductRatings
                    .FirstOrDefaultAsync(r => r.ProductId == group.Key, stoppingToken);
                if (rating == null)
                {
                    db.ProductRatings.Add(new ProductRating
                    {
                        ProductId = group.Key,
                        AverageScore = average,
                        UpdatedAt = DateTime.UtcNow
                    });
                }
                else
                {
                    rating.AverageScore = average;
                    rating.UpdatedAt = DateTime.UtcNow;
                }
            }
            await db.SaveChangesAsync(stoppingToken);
        }
    }
}
