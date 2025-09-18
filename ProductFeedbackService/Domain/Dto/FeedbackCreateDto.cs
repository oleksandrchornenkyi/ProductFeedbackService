namespace ProductFeedbackService.Domain.Dto;

public class FeedbackCreateDto
{
    public int ProductId { get; set; }
    public string ReviewText { get; set; } = "";
}