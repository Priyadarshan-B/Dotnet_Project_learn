using Microsoft.AspNetCore.Mvc;
using sample_api.Models;
using sample_api.Services;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace sample_api.Controllers;

[Route("api/auth")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly AuthService _authService;
    private const string JwtSecretKey = "key is enough secret for 16-bytes";
    private const string Issuer = "http://localhost";
    private const string Audience = "http://localhost";

    public AuthController(AuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request)
    {
        var newUser = await _authService.RegisterUser(request.Username, request.Email, request.Password, request.Phone);
        if (newUser == null)
        {
            return BadRequest(new { status = 400, message = "Registration failed" });
        }

        return Ok(new { 
            status = 200, 
            message = "User registered successfully", 
            user = new 
            {
                Id = newUser.SupabaseId.ToString(),
                Username = newUser.Username,
                Phone = newUser.Phone
            }
        });
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        var user = await _authService.Login(request.Email, request.Password);
        if (user == null)
        {
            return Unauthorized(new { status = 401, message = "Invalid credentials" });
        }

        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.Username),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Role, user.Role)
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(JwtSecretKey));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: Issuer,
            audience: Audience,
            claims: claims,
            expires: DateTime.Now.AddHours(24),
            signingCredentials: creds
        );

        return Ok(new
        {
            status = 200,
            message = "Login successful",
            user,
            token = new JwtSecurityTokenHandler().WriteToken(token)
        });
    }
}
