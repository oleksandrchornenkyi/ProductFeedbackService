namespace ProductFeedbackService.Controllers;

using Microsoft.AspNetCore.Mvc;
using ProductFeedbackService.Domain.Dto;
using ProductFeedbackService.Domain.Models;
using Microsoft.EntityFrameworkCore;
using ProductFeedbackService.Infrastructure;
using ProductFeedbackService.Domain.Services;

[ApiController]
[Route("api/[controller]")]
public class FeedbackController : ControllerBase
{
    private readonly AppDbContext _db;
    private readonly IRatingCalculator _calc;
    public FeedbackController(AppDbContext db, IRatingCalculator calc)
    {
        _db = db;
        _calc = calc;
    }
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Feedback>>> GetAll()
    {
        var items = await _db.Feedbacks
            .AsNoTracking()
            .ToListAsync();
        return Ok(items);
    }
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] FeedbackCreateDto dto)
    {
        if (dto.ProductId <= 0)
            return BadRequest(new { error = "productId must be > 0." });
        if (string.IsNullOrWhiteSpace(dto.ReviewText))
            return BadRequest(new { error = "text must not be empty." });
        if (dto.ReviewText.Length > 1000)
            return BadRequest(new { error = "text is too long (max 1000)." });
        if (dto == null)
            return BadRequest(new { error = "Body is required." });
        var model = new Feedback
        {
            ProductId = dto.ProductId,
            ReviewText = dto.ReviewText.Trim(),
            CreatedAt = DateTime.UtcNow
        };
        _db.Feedbacks.Add(model);
        await _db.SaveChangesAsync();
        var dict = await _db.WordRatings.AsNoTracking().ToListAsync();
        var score = _calc.CalculateReviewScore(model.ReviewText, dict);
        return Created(string.Empty, new { id = model.FeedbackId, score });
    }
    [HttpGet("rating/{productId:int}")]
    public async Task<ActionResult<double>> GetProductRating(int productId)
    {
        if (productId <= 0) return BadRequest("productId must be > 0");
        var rating = await _db.ProductRatings.FirstOrDefaultAsync(r => r.ProductId == productId);
        return Ok(rating?.AverageScore ?? 0.0);
    }
}