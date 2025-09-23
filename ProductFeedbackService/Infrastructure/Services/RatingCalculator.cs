using System.Text.RegularExpressions;
using ProductFeedbackService.Domain.Models;
using ProductFeedbackService.Domain.Services;

namespace ProductFeedbackService.Infrastructure.Services;

public class RatingCalculator : IRatingCalculator
{
    public double CalculateReviewScore(string text, IEnumerable<WordRating> dict)
    {
        if (string.IsNullOrWhiteSpace(text) || dict is null)
            return 0.0;
        // normalization
        var lower = text.ToLowerInvariant();
        lower = Regex.Replace(lower, @"[^a-z0-9\s]+", " ");
        lower = Regex.Replace(lower, @"\s+", " ").Trim();
        var tokens = lower.Length == 0 ? Array.Empty<string>() : lower.Split(' ');
        if (tokens.Length == 0) return 0.0;
        // dictionaries
        var dict1 = new Dictionary<string, int>(); 
        var dict2 = new Dictionary<string, int>();
        foreach (var wr in dict)
        {
            var phrase = (wr.Phrase ?? string.Empty).Trim().ToLowerInvariant();
            if (string.IsNullOrWhiteSpace(phrase)) continue;
            if (phrase.Contains(' '))
                dict2[phrase] = wr.Score;
            else
                dict1[phrase] = wr.Score;
        }
        // make two arrays with pairs of word: one starting from first word, another - starting from the second
        var pairs0 = BuildPairs(tokens, startOffset: 0);
        var pairs1 = BuildPairs(tokens, startOffset: 1);
        var scores = new List<int>();
        var used = new HashSet<int>();
        // pairs starting from first
        foreach (var (phrase, i, j) in pairs0)
        {
            if (used.Contains(i) || used.Contains(j)) continue;
            if (dict2.TryGetValue(phrase, out var s))
            {
                scores.Add(s);
                used.Add(i);
                used.Add(j);
            }
        }
        // pairs starting from second
        foreach (var (phrase, i, j) in pairs1)
        {
            if (used.Contains(i) || used.Contains(j)) continue;
            if (dict2.TryGetValue(phrase, out var s))
            {
                scores.Add(s);
                used.Add(i);
                used.Add(j);
            }
        }
        // one-word
        for (int i = 0; i < tokens.Length; i++)
        {
            if (used.Contains(i)) continue;
            if (dict1.TryGetValue(tokens[i], out var s))
                scores.Add(s);
        }
        // raring
        return scores.Count == 0 ? 0.0 : scores.Average();
    }
    // build list with pairs
    private static List<(string phrase, int i, int j)> BuildPairs(string[] tokens, int startOffset)
    {
        var res = new List<(string, int, int)>();
        for (int i = startOffset; i + 1 < tokens.Length; i++)
        {
            var j = i + 1;
            res.Add(($"{tokens[i]} {tokens[j]}", i, j));
        }
        return res;
    }
}