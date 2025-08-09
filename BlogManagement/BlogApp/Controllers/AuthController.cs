using BlogApp.Models;
using BlogApp.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BlogApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(User request) //apply Dto
        {
            throw new NotImplementedException();
        }

        [HttpPost("login")]
        public async Task<IActionResult> LogIn(User request) //apply Dto
        {
            throw new NotImplementedException();
        }


        //testing endpoint
        [HttpGet("testjwt")]
        [Authorize(Roles = "User, Admin")]
        public IActionResult GetUserIdFromToken()
        {
            throw new NotImplementedException();
        }
    }
}
