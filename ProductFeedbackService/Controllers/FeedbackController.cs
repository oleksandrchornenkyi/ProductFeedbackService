namespace ProductFeedbackService.Controllers;

using Microsoft.AspNetCore.Mvc;
using ProductFeedbackService.Domain.Dto;
using ProductFeedbackService.Domain.Models;
using ProductFeedbackService.Infrastructure.Storage;

[ApiController]
[Route("api/[controller]")]
public class FeedbackController : ControllerBase
{
    private readonly FeedbackStorage _storage;
    public FeedbackController(FeedbackStorage storage) => _storage = storage;
    [HttpGet]
    public ActionResult<IEnumerable<Feedback>> GetAll()
    {
        var items = _storage.GetAll();
        return Ok(items);
    }
    [HttpPost]
    public IActionResult Create([FromBody] FeedbackCreateDto dto)
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
            ReviewText = dto.ReviewText
        };
        var id = _storage.Add(model);
        return Created(string.Empty, new { id });
    }
}