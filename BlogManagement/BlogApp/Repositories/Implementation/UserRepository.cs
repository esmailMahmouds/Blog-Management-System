using BlogApp.Context;
using BlogApp.Models;
using BlogApp.Repositories.Interfaces;

namespace BlogApp.Repositories.Implementation
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _context;

        public UserRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public Task AddUser(User user)
        {
            throw new NotImplementedException();
        }

        public Task<User?> GetUserById(int id)
        {
            throw new NotImplementedException();
        }
    }
}
