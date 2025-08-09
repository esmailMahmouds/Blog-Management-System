using BlogApp.Models;

namespace BlogApp.Repositories.Interfaces
{
    public interface IUserRepository
    {
        public Task AddUser(User user);
        public Task<User?> GetUserById(int id);
    }
}
