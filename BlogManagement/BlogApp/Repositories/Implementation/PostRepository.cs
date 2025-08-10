using BlogApp.Context;
using BlogApp.Enums;
using BlogApp.Models;
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
                .ToListAsync();
        }
    }
}
