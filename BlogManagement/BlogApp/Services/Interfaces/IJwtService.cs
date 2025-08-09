using BlogApp.Models.DomainClasses;

namespace BlogApp.Services.Interfaces
{
    public interface IJwtService
    {
        string GenerateToken(User user);
        int GetUserIdFromToken(string token);
    }
}
