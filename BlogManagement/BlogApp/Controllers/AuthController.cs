using BlogApp.Models.Dtos;
using BlogApp.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BlogApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly ILogger<AuthController> _logger;

        public AuthController(IAuthService authService, ILogger<AuthController> logger)
        {
            _authService = authService;
            _logger = logger;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(UserSignUpDto request)
        {
            _logger.LogInformation("Registration request received");

            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Registration failed: Invalid model state");
                return BadRequest(ModelState);
            }

            var result = await _authService.UserSignUpAsync(request);

            if (result.Success)
            {
                _logger.LogInformation("Registration successful");
                return Ok(result.Data);
            }

            _logger.LogWarning("Registration failed: {Error}", result.Error);
            return BadRequest(new { error = result.Error });
        }

        [HttpPost("login")]
        public async Task<IActionResult> LogIn(UserSignInDto request)
        {
            _logger.LogInformation("Login request received");

            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Login failed: Invalid model state");
                return BadRequest(ModelState);
            }

            var result = await _authService.UserSignInAsync(request);

            if (result.Success)
            {
                _logger.LogInformation("Login successful");
                return Ok(result.Data);
            }

            _logger.LogWarning("Login failed: {Error}", result.Error);
            return BadRequest(new { error = result.Error });
        }


        //testing endpoint
        [HttpGet("TestJWT")]
        [Authorize(Roles = "Author, Admin")]
        public IActionResult GetUserIdFromToken()
        {
            _logger.LogInformation("JWT test endpoint accessed");

            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (userIdClaim != null && int.TryParse(userIdClaim, out int userId))
            {
                var response = new
                {
                    UserId = userId,
                    Role = User.FindFirst(ClaimTypes.Role)?.Value,
                    Name = User.FindFirst(ClaimTypes.Name)?.Value,
                    Email = User.FindFirst(ClaimTypes.Email)?.Value
                };

                _logger.LogInformation("JWT test successful");
                return Ok(response);
            }

            _logger.LogWarning("JWT test failed: Unable to retrieve user information from token");
            return BadRequest("Unable to retrieve user information from token.");
        }
    }
}
