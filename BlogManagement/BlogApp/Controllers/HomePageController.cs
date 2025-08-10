using BlogApp.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BlogApp.Controllers
{
    public class HomePageController : Controller
    {
        private readonly IPostService _postService;

        public HomePageController(IPostService postService)
        {
            _postService = postService;
        }

        public async Task<IActionResult> PostsDisplay()
        {
            var posts = await _postService.GetAllPosts();
            return View(posts);
        }
    }
}
