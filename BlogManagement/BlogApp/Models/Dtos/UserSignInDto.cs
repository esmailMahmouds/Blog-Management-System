using System.ComponentModel.DataAnnotations;

namespace BlogApp.Models.Dtos
{
    public class UserSignInDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        [MinLength(6)]
        public string Password { get; set; } = string.Empty;
    }
}
