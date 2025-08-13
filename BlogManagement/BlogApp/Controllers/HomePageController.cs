using BlogApp.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BlogApp.Controllers
{
    public class HomePageController : Controller
    {
        private readonly IPostService _postService;
        private readonly IJwtService _jwtService;

        public HomePageController(IPostService postService, IJwtService jwtService)
        {
            _postService = postService;
            _jwtService = jwtService;
        }

        public async Task<IActionResult> PostsDisplay()
        {
            var posts = await _postService.GetAllPosts();


            Request.Cookies.TryGetValue("Jwt", out string? jwtToken);
            if (!string.IsNullOrEmpty(jwtToken))
            {
                try
                {
                    var userId = _jwtService.GetUserIdFromToken(jwtToken);
                    ViewBag.CurrentUserId = userId;
                }
                catch
                {
                    ViewBag.CurrentUserId = null;
                }
            }
            else
            {
                ViewBag.CurrentUserId = null;
            }

            return View(posts);
        }

        public async Task<IActionResult> ViewPost(int id)
        {
            var post = await _postService.GetPostById(id);
            if (post == null)
            {
                return NotFound();
            }

            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            int? userId = userIdClaim != null ? int.Parse(userIdClaim) : (int?)null;

            ViewBag.CurrentUserId = userId;

            return View(post);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> LikePost(int postId)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (!int.TryParse(userIdClaim, out int userId))
            {
                return Unauthorized();
            }

            await _postService.LikePost(postId, userId);
            return RedirectToAction("ViewPost", new { id = postId });
        }
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> RatePost(int postId, double rating)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (!int.TryParse(userIdClaim, out int userId))
            {
                return Unauthorized();
            }
            await _postService.RatePost(postId, userId, rating);
            return RedirectToAction("ViewPost", new { id = postId });
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> AddComment(int postId, string content)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (!int.TryParse(userIdClaim, out int userId))
            {
                return Unauthorized();
            }
            await _postService.AddComment(postId, userId, content);
            return RedirectToAction("ViewPost", new { id = postId });
        }
    }
}
