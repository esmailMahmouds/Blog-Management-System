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

        public async Task<IActionResult> ViewPost(int id)
        {
            var post = await _postService.GetPostById(id);
            if (post == null)
            {
                return NotFound();
            }
            return View(post);
        }

        [HttpPost]
        public async Task<IActionResult> LikePost(int postId)
        {
            //todo: replace when merge with authentication
            int userId = 1; 
            await _postService.LikePost(postId, userId);
            return RedirectToAction("ViewPost", new { id = postId });
        }
        [HttpPost]
        public async Task<IActionResult> RatePost(int postId, double rating)
        {
            //todo: replace when merge with authentication
            int userId = 1;
            await _postService.RatePost(postId, userId, rating);
            return RedirectToAction("ViewPost", new { id = postId });
        }

        [HttpPost]
        public async Task<IActionResult> AddComment(int postId, string content)
        {
            //todo: replace when merge with authentication
            int userId = 1;
            await _postService.AddComment(postId, userId, content);
            return RedirectToAction("ViewPost", new { id = postId });
        }

    }
}
