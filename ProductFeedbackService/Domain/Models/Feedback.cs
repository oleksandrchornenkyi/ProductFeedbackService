namespace ProductFeedbackService.Domain.Models;

public class Feedback
{
    public int ReviewId { get; set; }
    public int ProductId { get; set; }
    public string ReviewText { get; set; } = "";
    public DateTime CreatedAt { get; set; }
}