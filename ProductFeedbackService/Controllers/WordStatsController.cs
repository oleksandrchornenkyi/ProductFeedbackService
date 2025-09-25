namespace ProductFeedbackService.Controllers;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProductFeedbackService.Infrastructure;
using ProductFeedbackService.Domain.Models;

[ApiController]
[Route("api/[controller]")]
public class WordStatsController : ControllerBase
{
    private readonly AppDbContext _db;
    public WordStatsController(AppDbContext db) { _db = db; }
    [HttpGet]
    public async Task<ActionResult<IEnumerable<WordRating>>> GetAll()
    {
        var words = await _db.WordRatings.OrderBy(w => w.Phrase).ToListAsync();
        return Ok(words);
    }
}
