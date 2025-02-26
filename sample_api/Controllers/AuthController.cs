using Microsoft.AspNetCore.Mvc;
using sample_api.Models;
using sample_api.Services;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace sample_api.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly AuthServices _authServices;
        private const string JwtSecretKey = "key is enough secret for 16-bytes";
        private const string Issuer = "http://localhost";
        private const string Audience = "http://localhost";

        public AuthController(AuthServices authServices)
        {
            _authServices = authServices;
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequest request)
        {
            try
            {
                if (request == null || string.IsNullOrEmpty(request.Username) || string.IsNullOrEmpty(request.Password))
                    return BadRequest(new { status = 400, message = "Invalid request" });

                var UserDTO = _authServices.Authenticate(request.Username, request.Password);
                if (UserDTO == null)
                    return Unauthorized(new { status = 401, message = "Invalid credentials" });
                var claims = new[]
                {
                    new Claim(ClaimTypes.Name, UserDTO.Username),
                    new Claim(ClaimTypes.Email, UserDTO.Email)
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
                var jwtToken = new JwtSecurityTokenHandler().WriteToken(token);
                return Ok(new { status = 200, message = "Login successful", token = jwtToken });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { status = 500, message = "Internal server error", error = ex.Message });
            }

        }

        [HttpPost("register")]
        public IActionResult Register([FromBody] User user)
        {
            if (user == null || string.IsNullOrWhiteSpace(user.Username) || string.IsNullOrWhiteSpace(user.Password))
            {
                return BadRequest(new { status = 400, message = "Username and Password are required." });
            }

            try
            {
                var registeredUser = _authServices.Register(user);
                return Ok(new { status = 200, message = "Registered Successfully", user = registeredUser });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { status = 500, message = "Registration failed", error = ex.Message });
            }
        }
    }
}
