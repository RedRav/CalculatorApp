using CalculatorApp.Application;
using CalculatorApp.Application.Auth;
using CalculatorApp.Application.Extensions;
using CalculatorApp.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly JwtOptions _jwtOptions;

    public AuthController(UserManager<ApplicationUser> userManager, IConfiguration config, IOptions<JwtOptions> options)
    {
        _userManager = userManager;
        _jwtOptions = options.Value;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterDto dto)
    {
        var user = new ApplicationUser { UserName = dto.Email, Email = dto.Email };
        var result = await _userManager.CreateAsync(user, dto.Password);

        if (!result.Succeeded)
            return BadRequest(result.Errors);

        return Ok("Регистрация успешна");
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginDto dto)
    {
        var user = await _userManager.FindByEmailAsync(dto.Email);
        if (user == null || !await _userManager.CheckPasswordAsync(user, dto.Password))
            return Unauthorized("Неверный email или пароль");

        var token = GenerateJwtToken(user, _jwtOptions, "user");
        return Ok(new { token });
    }

    private string GenerateJwtToken(ApplicationUser user, JwtOptions jwtOptions, string role)
    {
        return user.GenerateJWTAuthorizeToken(jwtOptions, role);
    }
}