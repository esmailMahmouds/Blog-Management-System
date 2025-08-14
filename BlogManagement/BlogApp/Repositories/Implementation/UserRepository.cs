using BlogApp.Context;
using BlogApp.Models.DomainClasses;
using BlogApp.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BlogApp.Repositories.Implementation
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _context;

        public UserRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddUser(User user)
        {
            await _context.Users.AddAsync(user);
        }

        public async Task<User?> GetUserByEmail(string email)
        {
            return await _context.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<User?> GetUserById(int id)
        {
            return await _context.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Id == id);
        }

        public User? UpdateUser(User user)
        {
            var entityEntry = _context.Users.Update(user);
            return entityEntry.Entity;
        }

        //admin specific methods
        public async Task<IEnumerable<User>> GetAllUsers()
        {
            return await _context.Users
                .AsNoTracking()
                .Include(u => u.Country)
                .OrderBy(u => u.Name)
                .ToListAsync();
        }

        public async Task<bool> DeleteUser(int userId)
        {
            var user = await _context.Users
                .Include(u => u.Comments)
                .Include(u => u.Ratings)
                .Include(u => u.Likes)
                .Include(u => u.Followers)
                .Include(u => u.Followings)
                .FirstOrDefaultAsync(u => u.Id == userId);

            if (user == null)
                return false;

            if (user.Comments?.Any() == true)
            {
                _context.Comments.RemoveRange(user.Comments);
            }

            if (user.Ratings?.Any() == true)
            {
                _context.Ratings.RemoveRange(user.Ratings);
            }

            if (user.Likes?.Any() == true)
            {
                _context.Likes.RemoveRange(user.Likes);
            }

            if (user.Followers?.Any() == true)
            {
                _context.Follows.RemoveRange(user.Followers);
            }

            if (user.Followings?.Any() == true)
            {
                _context.Follows.RemoveRange(user.Followings);
            }


            var userPosts = await _context.Posts
                .Include(p => p.Comments)
                .Include(p => p.Ratings)
                .Include(p => p.Likes)
                .Where(p => p.UserId == userId)
                .ToListAsync();

            foreach (var post in userPosts)
            {
                if (post.Comments?.Any() == true)
                {
                    _context.Comments.RemoveRange(post.Comments);
                }

                if (post.Ratings?.Any() == true)
                {
                    _context.Ratings.RemoveRange(post.Ratings);
                }

                if (post.Likes?.Any() == true)
                {
                    _context.Likes.RemoveRange(post.Likes);
                }
            }

            _context.Posts.RemoveRange(userPosts);
            _context.Users.Remove(user);
            return true;
        }
    }
}
