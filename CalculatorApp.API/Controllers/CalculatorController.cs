using CalculatorApp.Application.Helpers;
using CalculatorApp.Domain.Entities;
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

    [Authorize]
    [HttpPost("randomData")]
    public async Task<IActionResult> Fill([FromBody] string userId)
    {
        var logs = new List<CalculationLog>();
        Random random = new Random();
        var arr = new[] { "+", "-", "*", "/", "pow", "root" };
        var date = DateTime.UtcNow;
        for (int i = 0; i < 1_000_000; i++)
        {
            date.AddHours(random.Next(-1000, 1000));
            var log = new CalculationLog()
            {
                Operand1 = random.Next(0, 1000),
                Operand2 = random.Next(0, 1000),
                Result = 228,
                Operation = arr[random.Next(0, arr.Length - 1)],
                UserId = userId,
                Timestamp = date,
            };
            logs.Add(log);
            Console.WriteLine($"Операция {i}");
        }
        await _service.LogAsync(logs);

        return Ok();
    }

    [HttpGet("logs")]
    public async Task<IActionResult> GetLogs([FromQuery] int offset = 0, [FromQuery] int pageSize = 10)
    {
        var userId = GetUserId();
        if (userId == null)
            return Unauthorized();

        IQueryable<CalculationLog> query = _db.CalculationLogs
            .Where(x => x.UserId == userId)
            .OrderByDescending(x => x.Timestamp);

        query = PaginationHelper.OffsetPagination(query, offset, pageSize);

        var logs = await query
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

    [HttpGet("logs/cursor")]
    public async Task<IActionResult> GetLogsCursor(
    [FromQuery] DateTime? cursor = null,
    [FromQuery] int pageSize = 10)
    {
        var userId = GetUserId();
        if (userId == null)
            return Unauthorized();

        var query = _db.CalculationLogs
            .Where(x => x.UserId == userId);

        query = PaginationHelper.CursorPagination(
            query,
            x => x.Timestamp,
            cursor,
            pageSize);

        var logs = await query
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