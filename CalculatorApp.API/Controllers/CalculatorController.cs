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
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
        var result = await _service.CalculateAsync(request, userId);

        if (result.Message != "OK")
            return BadRequest(result);

        return Ok(result);
    }

    [HttpGet("logs")]
    [Authorize]
    public async Task<IActionResult> GetLogs()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

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
    [Authorize]
    public async Task<IActionResult> ClearLogs()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var logs = _db.CalculationLogs.Where(x => x.UserId == userId);
        _db.CalculationLogs.RemoveRange(logs);
        await _db.SaveChangesAsync();
        return NoContent();
    }
}