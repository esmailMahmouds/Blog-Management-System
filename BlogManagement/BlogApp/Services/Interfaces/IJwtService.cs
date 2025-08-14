using BlogApp.Models.DomainClasses;
using BlogApp.Enums;

namespace BlogApp.Services.Interfaces
{
    public interface IJwtService
    {
        string GenerateToken(User user);
        int GetUserIdFromToken(string token);
        Role GetUserRoleFromToken(string token);
    }
}
