namespace ProductFeedbackService.Controllers;

using Microsoft.AspNetCore.Mvc;
using ProductFeedbackService.Domain.Dto;
using ProductFeedbackService.Domain.Models;
using Microsoft.EntityFrameworkCore;
using ProductFeedbackService.Infrastructure;

[ApiController]
[Route("api/[controller]")]
public class FeedbackController : ControllerBase
{
    private readonly AppDbContext _db;
    public FeedbackController(AppDbContext db) => _db = db;
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Feedback>>> GetAll()
    {
        var items = await _db.Feedbacks.ToListAsync();
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
            ReviewText = dto.ReviewText,
            CreatedAt = DateTime.UtcNow
        };

        _db.Feedbacks.Add(model);
        await _db.SaveChangesAsync();
        
        return Created(string.Empty, new { id = model.ReviewId });
    }
}