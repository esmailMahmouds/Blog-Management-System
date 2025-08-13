using BlogApp.Models.Dtos;
using BlogApp.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BlogApp.Controllers
{
    [Authorize]
    public class PostsController : Controller
    {
        private readonly IPostService _postService;
        private readonly IJwtService _jwtService;

        public PostsController(IPostService postService, IJwtService jwtService)
        {
            _postService = postService;
            _jwtService = jwtService;
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var categories = await _postService.GetAllCategories();
            ViewBag.Categories = categories;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreatePostDto createPostDto)
        {
            if (!ModelState.IsValid)
            {
                var categories = await _postService.GetAllCategories();
                ViewBag.Categories = categories;
                return View(createPostDto);
            }


            try
            {
                Request.Cookies.TryGetValue("Jwt", out string? jwtToken);

                if (string.IsNullOrEmpty(jwtToken))
                {
                    ModelState.AddModelError("", "You must be logged in to create a post.");
                    var categories = await _postService.GetAllCategories();
                    ViewBag.Categories = categories;
                    return View(createPostDto);
                }

                var userId = _jwtService.GetUserIdFromToken(jwtToken);

                await _postService.CreatePost(createPostDto, userId);
                TempData["SuccessMessage"] = "Post created successfully! It will be reviewed before being published.";
                return RedirectToAction("PostsDisplay", "HomePage");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "An error occurred while creating the post. Please try again.");
                var categories = await _postService.GetAllCategories();
                ViewBag.Categories = categories;
                return View(createPostDto);
            }
        }
    }
}