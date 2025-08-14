using BlogApp.Models.DomainClasses;

namespace BlogApp.Repositories.Interfaces
{
	public interface IFollowRepository
	{
		public Task AddFollower(Follow follow);
		public Task DeleteFollower(int id);
		public Task<IEnumerable<Follow>> GetFollowersByUserId(int userId);
		public Task<IEnumerable<Follow>> GetFollowingsByUserId(int userId);
	}
}
