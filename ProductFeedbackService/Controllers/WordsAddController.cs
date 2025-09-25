namespace ProductFeedbackService.Controllers;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProductFeedbackService.Infrastructure;
using ProductFeedbackService.Domain.Models;

[ApiController]
[Route("api/[controller]")]
public class WordsAddController : ControllerBase
{
    private readonly AppDbContext _db;
    public WordsAddController(AppDbContext db) { _db = db; }
    [HttpGet]
    public async Task<ActionResult<IEnumerable<WordRating>>> GetAll()
    {
        var items = await _db.WordRatings
            .OrderBy(w => w.Phrase)
            .ToListAsync();
        return Ok(items);
    }
    public record UpsertWordRequest(string Phrase, int Score);
    [HttpPost]
    public async Task<IActionResult> AddOrUpdate([FromBody] UpsertWordRequest body)
    {
        if (body is null) return BadRequest("body is required");
        if (string.IsNullOrWhiteSpace(body.Phrase)) return BadRequest("phrase must not be empty");
        if (body.Score < 1 || body.Score > 5) return BadRequest("score must be between 1 and 5");

        var phrase = body.Phrase.Trim().ToLowerInvariant();

        var existing = await _db.WordRatings.FirstOrDefaultAsync(w => w.Phrase == phrase);
        if (existing == null)
            _db.WordRatings.Add(new WordRating { Phrase = phrase, Score = body.Score });
        else
            existing.Score = body.Score;
        await _db.SaveChangesAsync();
        return Ok(new { phrase, score = body.Score });
    }
}
