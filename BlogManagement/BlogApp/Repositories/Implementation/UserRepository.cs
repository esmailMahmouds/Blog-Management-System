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

            await _context.Comments.Where(c => c.UserId == userId).ExecuteDeleteAsync();
            await _context.Ratings.Where(r => r.UserId == userId).ExecuteDeleteAsync();
            await _context.Likes.Where(l => l.UserId == userId).ExecuteDeleteAsync();
            await _context.Follows.Where(f => f.FollowerUserId == userId || f.FollowingUserId == userId).ExecuteDeleteAsync();


            var userPostIds = await _context.Posts.Where(p => p.UserId == userId).Select(p => p.Id).ToListAsync();

            if (userPostIds.Any())
            {
                await _context.Comments.Where(c => userPostIds.Contains(c.PostId)).ExecuteDeleteAsync();
                await _context.Ratings.Where(r => userPostIds.Contains(r.PostId)).ExecuteDeleteAsync();
                await _context.Likes.Where(l => userPostIds.Contains(l.PostId)).ExecuteDeleteAsync();
                await _context.Posts.Where(p => userPostIds.Contains(p.Id)).ExecuteDeleteAsync();
            }


            var result = await _context.Users.Where(u => u.Id == userId).ExecuteDeleteAsync();
            return result > 0;
        }


        public async Task<int> GetTotalUsersCount()
        {
            return await _context.Users.CountAsync();
        }
    }
}
