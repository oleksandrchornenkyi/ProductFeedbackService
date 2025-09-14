namespace ProductFeedbackservice.Controllers;

using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class HelloController : ControllerBase
{
    [HttpGet]
    public string GetHello()
    {
        return "Hello from API!";
    }
    [HttpGet("ping")]
    public string Ping() => "pong";
} 