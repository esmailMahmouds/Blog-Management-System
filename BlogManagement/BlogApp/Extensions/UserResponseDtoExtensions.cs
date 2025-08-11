using BlogApp.Models.DomainClasses;
using BlogApp.Models.Dtos;

namespace BlogApp.Extensions
{
    public static class UserResponseDtoExtensions
    {
        public static UserSignUpResponseDto ToUserSignUpResponseDto(this User user)
        {
            return new UserSignUpResponseDto
            {
                UserId = user.Id,
                Name = user.Name,
                Email = user.Email,
                Role = user.Role
            };
        }
    }
}
