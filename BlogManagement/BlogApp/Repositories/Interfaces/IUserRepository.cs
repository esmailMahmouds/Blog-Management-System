using BlogApp.Models.DomainClasses;

namespace BlogApp.Repositories.Interfaces
{
    public interface IUserRepository
    {
        public Task AddUser(User user);
        public Task<User?> GetUserById(int id);
        public Task<User?> GetUserByEmail(string email);
        public User? UpdateUser(User user);
        public Task<User?> GetUserByIdWithFollow(int id);

	}
}
