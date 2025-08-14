using BlogApp.Models.DomainClasses;

namespace BlogApp.Services.Interfaces
{
    public interface IUserService
    {
        Task<User?> GetUserById(int id);
        Task<User?> GetUserByEmail(string email);

        //admin specific methods
        Task<IEnumerable<User>> GetAllUsers();
        Task<bool> DeleteUser(int userId);
    }
}