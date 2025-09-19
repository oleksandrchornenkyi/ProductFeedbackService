namespace ProductFeedbackService.Domain.Models;

public class ProductRating
{
    public int ProductRatingId { get; set; }
    public int AverageScore { get; set; }
    public int ProductId { get; set; }
    public DateTime UpdatedAt { get; set; }
}