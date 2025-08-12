using BlogApp.Context;
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

        public AccountController(IAuthService authService, ILogger<AccountController> logger, ApplicationDbContext context)
        {
            _authService = authService;
            _logger = logger;
            _context = context; //change when countries logic is implemented
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
                //for simplicity, just redirect to home but in a real app, you'd set up authentication cookies
                TempData["SuccessMessage"] = "Sign in successful!";

                var cookieOptions = new CookieOptions
                {
                    HttpOnly = true,
                    Expires = DateTimeOffset.UtcNow.AddHours(1)
                };
                Response.Cookies.Append("Jwt", result.Data.AccessToken, cookieOptions);

                return RedirectToAction("PostsDisplay", "HomePage");
            }

            ModelState.AddModelError("", result.Error ?? "Sign in failed");
            return View(model);
        }

        private void LoadCountries()
        {
            var countries = _context.Countries.ToList();
            ViewBag.countries = new SelectList(countries, "Id", "Name");
        }
    }
}