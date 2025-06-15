using CalculatorApp.Persistence;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class CalculatorController : ControllerBase
{
    private readonly ICalculatorService _service;
    private readonly ApplicationDbContext _db;

    public CalculatorController(ICalculatorService service, ApplicationDbContext db)
    {
        _service = service;
        _db = db;
    }

    [HttpPost]
    public async Task<IActionResult> Calculate([FromBody] CalculatorRequest request)
    {
        var userId = GetUserId();
        if (userId == null)
            return Unauthorized();

        var result = await _service.CalculateAsync(request, userId);

        if (result.Message != "OK")
            return BadRequest(result);

        return Ok(result);
    }

    [HttpGet("logs")]
    public async Task<IActionResult> GetLogs()
    {
        var userId = GetUserId();
        if (userId == null)
            return Unauthorized();

        var logs = await _db.CalculationLogs
            .Where(x => x.UserId == userId)
            .OrderByDescending(x => x.Timestamp)
            .Select(x => new
            {
                x.Operation,
                x.Operand1,
                x.Operand2,
                x.Result,
                x.Error,
                x.Timestamp
            })
            .ToListAsync();

        return Ok(logs);
    }

    [HttpDelete("logs")]
    public async Task<IActionResult> ClearLogs()
    {
        var userId = GetUserId();
        if (userId == null)
            return Unauthorized();

        var logs = await _db.CalculationLogs
            .Where(x => x.UserId == userId)
            .ToListAsync();

        if (logs.Count == 0)
            return NoContent();

        _db.CalculationLogs.RemoveRange(logs);
        await _db.SaveChangesAsync();

        return NoContent();
    }

    private string? GetUserId()
    {
        return User.FindFirstValue(ClaimTypes.NameIdentifier);
    }
}