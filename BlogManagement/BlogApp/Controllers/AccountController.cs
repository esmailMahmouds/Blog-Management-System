using BlogApp.Context;
using BlogApp.Enums;
using BlogApp.Models.Dtos;
using BlogApp.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BlogApp.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAuthService _authService;
        private readonly ILogger<AccountController> _logger;
        private readonly ApplicationDbContext _context; //change when countries logic is implemented
        private readonly IJwtService _jwtService;

        public AccountController(IAuthService authService, ILogger<AccountController> logger, ApplicationDbContext context, IJwtService jwtService)
        {
            _authService = authService;
            _logger = logger;
            _context = context; //change when countries logic is implemented
            _jwtService = jwtService;
        }

        [HttpGet]
        public IActionResult SignUp()
        {
            LoadCountries();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SignUp(UserSignUpDto model)
        {
            if (!ModelState.IsValid)
            {
                LoadCountries();
                return View(model);
            }

            var result = await _authService.UserSignUpAsync(model);

            if (result.Success)
            {
                TempData["SuccessMessage"] = "Registration successful! You can now sign in.";
                return RedirectToAction("SignIn");
            }

            LoadCountries();
            ModelState.AddModelError("", result.Error ?? "Registration failed");
            return View(model);
        }

        [HttpGet]
        public IActionResult SignIn()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SignIn(UserSignInDto model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var result = await _authService.UserSignInAsync(model);

            if (result.Success)
            {
                TempData["SuccessMessage"] = "Sign in successful!";

                var cookieOptions = new CookieOptions
                {
                    HttpOnly = true,
                    Expires = DateTimeOffset.UtcNow.AddHours(1)
                };
                Response.Cookies.Append("Jwt", result.Data?.AccessToken ?? "", cookieOptions);

                //check user role and redirect accordingly
                try
                {
                    var userRole = _jwtService.GetUserRoleFromToken(result.Data?.AccessToken ?? "");
                    if (userRole == Role.Admin)
                    {
                        TempData["SuccessMessage"] = "Welcome back, Admin!";
                        return RedirectToAction("Dashboard", "Admin");
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogWarning("Failed to get user role from token: {Error}", ex.Message);
                }

                // Default redirect for regular users
                return RedirectToAction("PostsDisplay", "HomePage");
            }

            ModelState.AddModelError("", result.Error ?? "Sign in failed");
            return View(model);
        }

        [HttpGet]
        public IActionResult Logout()
        {
            if (!IsUserAuthenticated())
            {
                return RedirectToAction("Index", "Landing");
            }

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult LogoutConfirmed()
        {
            _logger.LogInformation("User logout requested");

            try
            {
                Response.Cookies.Delete("Jwt");

                TempData["SuccessMessage"] = "You have been logged out successfully.";
                _logger.LogInformation("User logged out successfully");

                return RedirectToAction("Index", "Landing");
            }
            catch (Exception e)
            {
                TempData["ErrorMessage"] = "An error occurred during logout : " + e.Message;
                return RedirectToAction("Index", "Landing");
            }
        }

        private void LoadCountries()
        {
            var countries = _context.Countries.ToList();
            ViewBag.countries = new SelectList(countries, "Id", "Name");
        }

        private bool IsUserAuthenticated()
        {
            return Request.Cookies.ContainsKey("Jwt") &&
                   !string.IsNullOrEmpty(Request.Cookies["Jwt"]);
        }
    }
}