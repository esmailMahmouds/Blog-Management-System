using BlogApp.Enums;
using BlogApp.Models.Dtos;
using BlogApp.Services.Interfaces;
using BlogApp.Filters;
using Microsoft.AspNetCore.Mvc;

namespace BlogApp.Controllers
{
    [AdminAuthorize]
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

        public async Task<IActionResult> Dashboard()
        {
            try
            {
                // Get dashboard statistics
                var pendingPosts = await _postService.GetPendingPosts();
                var allPosts = await _postService.GetAllPostsForAdmin();
                var allUsers = await _userService.GetAllUsers();

                ViewBag.PendingPostsCount = pendingPosts.Count();
                ViewBag.TotalPostsCount = allPosts.Count();
                ViewBag.TotalUsersCount = allUsers.Count();
                ViewBag.ApprovedPostsCount = allPosts.Count(p => p.Status == PostStatus.Approved);

                return View();
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Error loading dashboard data: " + ex.Message;
                return View();
            }
        }

        public async Task<IActionResult> PendingPosts()
        {
            try
            {
                var pendingPosts = await _postService.GetPendingPosts();
                return View(pendingPosts);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Error loading pending posts: " + ex.Message;
                return View(new List<BlogApp.Models.DomainClasses.Post>());
            }
        }

        public async Task<IActionResult> AllPosts()
        {
            try
            {
                var allPosts = await _postService.GetAllPostsForAdmin();
                return View(allPosts);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Error loading posts: " + ex.Message;
                return View(new List<BlogApp.Models.DomainClasses.Post>());
            }
        }

        public async Task<IActionResult> ManageUsers()
        {
            try
            {
                var users = await _userService.GetAllUsers();
                return View(users);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Error loading users: " + ex.Message;
                return View(new List<BlogApp.Models.DomainClasses.User>());
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ApprovePost([FromBody] AdminPostActionDto request)
        {
            try
            {
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
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Error approving post: " + ex.Message });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RejectPost([FromBody] AdminPostActionDto request)
        {
            try
            {
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
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Error rejecting post: " + ex.Message });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeletePost([FromBody] AdminPostActionDto request)
        {
            try
            {
                if (request.Id <= 0)
                {
                    return Json(new { success = false, message = "Invalid post ID." });
                }

                var deleted = await _postService.AdminDeletePost(request.Id);
                if (deleted)
                {
                    return Json(new { success = true, message = "Post deleted successfully." });
                }

                return Json(new { success = false, message = "Failed to delete post." });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Error deleting post: " + ex.Message });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteUser([FromBody] AdminUserActionDto request)
        {
            try
            {
                if (request.Id <= 0)
                {
                    return Json(new { success = false, message = "Invalid user ID." });
                }

                // Prevent admin from deleting themselves
                var currentUserId = GetCurrentUserId();
                if (request.Id == currentUserId)
                {
                    return Json(new { success = false, message = "You cannot delete your own account." });
                }

                var deleted = await _userService.DeleteUser(request.Id);
                if (deleted)
                {
                    return Json(new { success = true, message = "User deleted successfully." });
                }

                return Json(new { success = false, message = "Failed to delete user." });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Error deleting user: " + ex.Message });
            }
        }

        private int GetCurrentUserId()
        {
            Request.Cookies.TryGetValue("Jwt", out string? jwtToken);
            if (string.IsNullOrEmpty(jwtToken))
                throw new UnauthorizedAccessException("User not authenticated");

            try
            {
                return _jwtService.GetUserIdFromToken(jwtToken);
            }
            catch
            {
                throw new UnauthorizedAccessException("Invalid authentication token");
            }
        }
    }
}