using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class CalculatorController : ControllerBase
{
    private readonly ICalculatorService _service;

    public CalculatorController(ICalculatorService service)
    {
        _service = service;
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
}