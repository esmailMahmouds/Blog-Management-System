using BlogApp.Context;
using BlogApp.Models.DomainClasses;
using BlogApp.Models.Dtos;
using BlogApp.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Security.Claims;

namespace BlogApp.Controllers
{
    [Authorize]
    public class ProfileController : Controller
    {
        private readonly ILogger<ProfileController> _logger;
        private readonly IProfileService _profileService;
        private readonly IJwtService _jwtService;

        public ProfileController(ILogger<ProfileController> logger,IProfileService profileService, IJwtService jwtService)
        {
            _logger = logger;
            _profileService = profileService;
            _jwtService = jwtService;
        }

        public async Task<IActionResult> Index()
        {
            User? currentUser = null;

            if (Request.Cookies.TryGetValue("Jwt", out string? jwtToken) && !string.IsNullOrEmpty(jwtToken))
            {
                var userId = _jwtService.GetUserIdFromToken(jwtToken);
                currentUser = await _profileService.GetCurrentUser(userId);
            }

            if (currentUser != null)
            {
                List<Country> countries = await _profileService.GetCountries();
                ViewBag.countries = new SelectList(countries, "Id", "Name");

                return View(currentUser);
            }
            else
            {
                _logger.LogError("User not Registered");
                return RedirectToAction("Index", "Home");
            }
        }


        [HttpPost]
        public async Task<IActionResult> EditUserInfoAsync(UserInfoDto userInfoDto)
        {
            
            if (Request.Cookies.TryGetValue("Jwt", out string? jwtToken) && !string.IsNullOrEmpty(jwtToken))
            {
                var userId = _jwtService.GetUserIdFromToken(jwtToken);
               
                var result = await _profileService.EditUserInfoAsync(userInfoDto, userId);

                if (result.Success)
                    TempData["SuccessMessage"] = "User Data updated successfully";
                else
                    TempData["ErrorMessage"] = result.Error;
            }
            else
            {
                TempData["ErrorMessage"] = "Token not valid";
                _logger.LogError("Token not Valid");
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> ChangePasswordAsync(ChangePasswordDto changePasswordDto)
        {
			if (Request.Cookies.TryGetValue("Jwt", out string? jwtToken) && !string.IsNullOrEmpty(jwtToken))
			{
				var userId = _jwtService.GetUserIdFromToken(jwtToken);

				var  result = await _profileService.ChangePasswordAsync(changePasswordDto, userId);

				if (result.Success)
					TempData["SuccessMessage"] = "Password is changed successfully";
				else
					TempData["ErrorMessage"] = "Failed to Change Password\nOld Password is Wrong";
			}
			else
			{
				TempData["ErrorMessage"] = "Token not valid";
				_logger.LogWarning("Token not Valid");
			}

			return RedirectToAction("Index");
		}
	}
}
