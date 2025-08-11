using BlogApp.Enums;

namespace BlogApp.Models.Dtos
{
    public class UserSignUpResponseDto
    {
        public int UserId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public Role Role { get; set; } = Role.Author;
    }
}
