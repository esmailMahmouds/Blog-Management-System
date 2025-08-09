using BlogApp.Models.Dtos;
using BlogApp.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BlogApp.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAuthService _authService;
        private readonly ILogger<AccountController> _logger;

        public AccountController(IAuthService authService, ILogger<AccountController> logger)
        {
            _authService = authService;
            _logger = logger;
        }

        [HttpGet]
        public IActionResult SignUp()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SignUp(UserSignUpDto model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var result = await _authService.UserSignUpAsync(model);

            if (result.Success)
            {
                TempData["SuccessMessage"] = "Registration successful! You can now sign in.";
                return RedirectToAction("SignIn");
            }

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
                return RedirectToAction("Index", "Home");
            }

            ModelState.AddModelError("", result.Error ?? "Sign in failed");
            return View(model);
        }
    }
}