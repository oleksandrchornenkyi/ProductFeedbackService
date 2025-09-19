namespace ProductFeedbackService.Infrastructure;

using Microsoft.EntityFrameworkCore;
using ProductFeedbackService.Domain.Models;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) {}
    public DbSet<Product> Products => Set<Product>();
    public DbSet<Feedback> Feedbacks => Set<Feedback>();
    public DbSet<WordRating> WordRatings => Set<WordRating>();
    public DbSet<ProductRating> ProductRatings => Set<ProductRating>();
}
