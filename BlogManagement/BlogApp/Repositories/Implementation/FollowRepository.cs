using BlogApp.Context;
using BlogApp.Models.DomainClasses;
using BlogApp.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BlogApp.Repositories.Implementation
{
    public class FollowRepository : IFollowRepository
    {
		private readonly ApplicationDbContext _context;

		public FollowRepository(ApplicationDbContext context)
        {
			_context = context;
		}
        public async Task AddFollower(Follow follow)
        {
            await _context.Follows.AddAsync(follow);
        }

        public async Task DeleteFollower(int id)
        {
            var follow = await _context.Follows.FirstOrDefaultAsync(f => f.Id == id);
            if(follow != null)
            {
                _context.Remove(follow);
            }
        }

        public async Task<IEnumerable<Follow>> GetFollowersByUserId(int userId)
        {
            return await _context.Follows.Where(f => f.FollowingUserId == userId).ToListAsync();
        }

		public async Task<IEnumerable<Follow>> GetFollowingsByUserId(int userId)
		{
			return await _context.Follows.Where(f => f.FollowerUserId == userId).ToListAsync();
		}
	}
}
