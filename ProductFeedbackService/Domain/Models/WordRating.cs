namespace ProductFeedbackService.Domain.Models;

public class WordRating
{
    public int WordRatingId { get; set; }
    public int Score { get; set; }
    public string Phrase { get; set; } = "";
}