namespace ProductFeedbackService.Infrastructure.Storage;

using ProductFeedbackService.Domain.Models;
using System.Collections.Generic;

public class FeedbackStorage
{
    private readonly List<Feedback> _items = new();
    private int _nextId = 1;

    public int Add(Feedback feedback)
    {
        feedback.Id = _nextId++;
        feedback.CreatedAt = DateTime.UtcNow;
        _items.Add(feedback);
        return feedback.Id;
    }

    public IEnumerable<Feedback> GetAll()
    {
        return _items.ToArray();
    }
}
