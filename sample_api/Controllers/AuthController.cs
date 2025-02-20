using Microsoft.AspNetCore.Mvc;
using sample_api.Models;
using sample_api.Services;

namespace sample_api.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly AuthServices _authServices;

        public AuthController(AuthServices authServices)
        {
            _authServices = authServices;
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequest request)
        {
            try
            {
                bool isAuthenticated = _authServices.Authenticate(request.Username, request.Password);
                if (request == null || string.IsNullOrEmpty(request.Username) || string.IsNullOrEmpty(request.Password))
                    return BadRequest(new { status = 400, message = "Invalid request" });
                if (!isAuthenticated)
                    return Unauthorized(new { status = 401, message = "Invalid credentials" });

                return Ok(new { status = 200, message = "Login successful" });
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
