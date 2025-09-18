namespace ProductFeedbackService.Infrastructure.Storage;

using ProductFeedbackService.Domain.Models;
using System.Collections.Generic;

public class FeedbackStorage
{
    private readonly List<Feedback> _reviews = new();
    private int _nextId = 1;

    public int Add(Feedback feedback)
    {
        feedback.ReviewId = _nextId++;
        feedback.CreatedAt = DateTime.UtcNow;
        _reviews.Add(feedback);
        return feedback.ReviewId;
    }

    public IEnumerable<Feedback> GetAll()
    {
        return _reviews.ToArray();
    }
}
