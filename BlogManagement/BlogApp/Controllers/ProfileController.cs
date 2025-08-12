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
        private readonly ApplicationDbContext _context; //delete this and pass userId to the service 
        private readonly ILogger<ProfileController> _logger;
        private readonly IProfileService _profileService;
        private readonly IJwtService _jwtService;

        public ProfileController(ApplicationDbContext context, ILogger<ProfileController> logger,
            IProfileService profileService, IJwtService jwtService)
        {
            _context = context;
            _logger = logger;
            _profileService = profileService;
            _jwtService = jwtService;
        }

        public IActionResult Index()
        {
            User? currentUser = null;

            if (Request.Cookies.TryGetValue("Jwt", out string? jwtToken) && !string.IsNullOrEmpty(jwtToken))
            {
                var userId = _jwtService.GetUserIdFromToken(jwtToken);
                currentUser = _context.Users.Find(userId); //delete this and pass userId to the service instead
            }

            if (currentUser != null)
            {
                var countries = _context.Countries.ToList(); //delete this and pass userId to the service
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
            string jwtToken;
            if (Request.Cookies.TryGetValue("Jwt", out jwtToken))
            {
                var currentUser = GetCurrentUser();
                bool check = await _profileService.EditUserInfoAsync(userInfoDto, currentUser);

                if (check)
                    TempData["SuccessMessage"] = "User Data updated successfully";
                else
                    TempData["ErrorMessage"] = "User Data Can't be updated";
            }
            else
            {
                TempData["ErrorMessage"] = "Token not valid";
            }
            return RedirectToAction("Index");
        }

        //no need for this method anymore :D
        private User? GetCurrentUser()
        {
            var userIdclaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var user = new User();

            if (userIdclaim != null && int.TryParse(userIdclaim, out int userId))
            {
                user = _context.Users.Find(userId);
            }

            return user;
        }

    }
}
