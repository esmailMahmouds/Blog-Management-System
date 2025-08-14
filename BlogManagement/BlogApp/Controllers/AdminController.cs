using BlogApp.Enums;
using BlogApp.Models.Dtos;
using BlogApp.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BlogApp.Controllers
{
    public class AdminController : Controller
    {
        private readonly IPostService _postService;
        private readonly IUserService _userService;
        private readonly IJwtService _jwtService;

        public AdminController(IPostService postService, IUserService userService, IJwtService jwtService)
        {
            _postService = postService;
            _userService = userService;
            _jwtService = jwtService;
        }

        private bool IsAdmin()
        {
            Request.Cookies.TryGetValue("Jwt", out string? jwtToken);
            if (string.IsNullOrEmpty(jwtToken))
                return false;

            try
            {
                Role userRole = _jwtService.GetUserRoleFromToken(jwtToken);
                return userRole == Role.Admin;
            }
            catch
            {
                return false;
            }
        }

        public IActionResult Dashboard()
        {
            if (!IsAdmin())
            {
                TempData["ErrorMessage"] = "Access denied. Admin privileges required.";
                return RedirectToAction("Index", "Home");
            }

            return View();
        }

        public async Task<IActionResult> PendingPosts()
        {
            if (!IsAdmin())
            {
                TempData["ErrorMessage"] = "Access denied. Admin privileges required.";
                return RedirectToAction("Index", "Home");
            }

            var pendingPosts = await _postService.GetPendingPosts();
            return View(pendingPosts);
        }

        public async Task<IActionResult> AllPosts()
        {
            if (!IsAdmin())
            {
                TempData["ErrorMessage"] = "Access denied. Admin privileges required.";
                return RedirectToAction("Index", "Home");
            }

            var allPosts = await _postService.GetAllPostsForAdmin();
            return View(allPosts);
        }

        public async Task<IActionResult> ManageUsers()
        {
            if (!IsAdmin())
            {
                TempData["ErrorMessage"] = "Access denied. Admin privileges required.";
                return RedirectToAction("Index", "Home");
            }

            var users = await _userService.GetAllUsers();
            return View(users);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ApprovePost([FromBody] AdminPostActionDto request)
        {
            if (!IsAdmin())
            {
                return Json(new { success = false, message = "Access denied." });
            }

            if (request.Id <= 0)
            {
                return Json(new { success = false, message = "Invalid post ID." });
            }

            var approved = await _postService.ApprovePost(request.Id);
            if (approved)
            {
                return Json(new { success = true, message = "Post approved successfully." });
            }

            return Json(new { success = false, message = "Failed to approve post." });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RejectPost([FromBody] AdminPostActionDto request)
        {
            if (!IsAdmin())
            {
                return Json(new { success = false, message = "Access denied." });
            }

            if (request.Id <= 0)
            {
                return Json(new { success = false, message = "Invalid post ID." });
            }

            var rejected = await _postService.RejectPost(request.Id);
            if (rejected)
            {
                return Json(new { success = true, message = "Post rejected successfully." });
            }

            return Json(new { success = false, message = "Failed to reject post." });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeletePost([FromBody] AdminPostActionDto request)
        {
            if (!IsAdmin())
            {
                return Json(new { success = false, message = "Access denied." });
            }

            if (request.Id <= 0)
            {
                return Json(new { success = false, message = "Invalid post ID." });
            }

            var deleted = await _postService.AdminDeletePost(request.Id);
            if (deleted)
            {
                TempData["SuccessMessage"] = "Post deleted successfully.";
                return Json(new { success = true, message = "Post deleted successfully." });
            }

            return Json(new { success = false, message = "Failed to delete post." });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteUser([FromBody] AdminPostActionDto request)
        {
            if (!IsAdmin())
            {
                return Json(new { success = false, message = "Access denied." });
            }

            if (request.Id <= 0)
            {
                return Json(new { success = false, message = "Invalid user ID." });
            }

            // Get current admin ID to prevent self-deletion
            Request.Cookies.TryGetValue("Jwt", out string? jwtToken);
            var currentUserId = _jwtService.GetUserIdFromToken(jwtToken);

            if (currentUserId == request.Id)
            {
                return Json(new { success = false, message = "Cannot delete your own account." });
            }

            var deleted = await _userService.DeleteUser(request.Id);
            if (deleted)
            {
                TempData["SuccessMessage"] = "User deleted successfully.";
                return Json(new { success = true, message = "User deleted successfully." });
            }

            return Json(new { success = false, message = "Failed to delete user." });
        }
    }
}