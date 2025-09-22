using ProductFeedbackService.Domain.Models;

namespace ProductFeedbackService.Domain.Services;

public interface IRatingCalculator
{
    double CalculateReviewScore(string text, IEnumerable<WordRating> dict);
}
