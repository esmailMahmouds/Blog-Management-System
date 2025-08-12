using BlogApp.Context;
using BlogApp.Enums;
using BlogApp.Models.DomainClasses;
using BlogApp.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BlogApp.Repositories.Implementation
{
    public class PostRepository : IPostRepository
    {
        private readonly ApplicationDbContext _context;

        public PostRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<Post>> GetAllPosts()
        {
            return await _context.Posts
                .Where(p => p.Status == PostStatus.Approved)
                .Include(p => p.User)
                .Include(p => p.Category)
                .Include(p => p.Comments)
                .Include(p => p.Ratings)
                .Include(p => p.Likes)
                .OrderByDescending(p => p.CreateDate)
                .ToListAsync();
        }
        public async Task<Post?> GetPostById(int id)
        {
            return await _context.Posts
                .Include(p => p.User)
                .Include(p => p.Category)
                .Include(p => p.Comments).ThenInclude(c => c.User)
                .Include(p => p.Ratings)
                .Include(p => p.Likes)
                .FirstOrDefaultAsync(p => p.Id == id);
        }
        public async Task<bool> AddLike(int postId, int userId)
        {
            var existingLike = await _context.Likes
                .FirstOrDefaultAsync(l => l.PostId == postId && l.UserId == userId);

            var post = await _context.Posts.FirstOrDefaultAsync(p => p.Id == postId);
            if (post == null)
                return false;

            if (existingLike != null)
            {
                // Unlike
                post.LikeCount = Math.Max(0, post.LikeCount - 1);
                _context.Likes.Remove(existingLike);
            }
            else
            {
                // Like
                post.LikeCount++;
                _context.Likes.Add(new Like { PostId = postId, UserId = userId });
            }

            return true;
        }
        public async Task<bool> AddRating(int postId, int userId, double rating)
        {
            var post = await _context.Posts
                .Include(p => p.Ratings)
                .FirstOrDefaultAsync(p => p.Id == postId);

            if (post == null)
                return false;

            var existingRating = post.Ratings.FirstOrDefault(r => r.UserId == userId);

            if (existingRating != null)
            {
                // Update rating
                existingRating.Value = rating;
            }
            else
            {
                // Add new rating
                var newRating = new Rating { PostId = postId, UserId = userId, Value = rating };
                _context.Ratings.Add(newRating);
                post.RateCount++;
                post.Ratings = post.Ratings.Append(newRating).ToList();
            }

            // Recalculate average
            post.AverageRate = post.Ratings.Average(r => r.Value);

            return true;
        }
        public async Task<bool> AddComment(int postId, int userId, string content)
        {
            var post = await _context.Posts.FirstOrDefaultAsync(p => p.Id == postId);
            if (post == null)
                return false;

            _context.Comments.Add(new Comment
            {
                PostId = postId,
                UserId = userId,
                Content = content
            });

            return true;
        }
    }
}
