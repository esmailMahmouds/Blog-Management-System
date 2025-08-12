using System.ComponentModel.DataAnnotations;

namespace BlogApp.Models.Dtos
{
    public class UserInfoDto
    {
        [Required]
        public string Name { get; set; }

        [Required]
        [EmailAddress]
        [StringLength(254, MinimumLength = 3)]
        public string Email { get; set; } = string.Empty;
        
        [Required]
        public DateOnly DateOfBirth { get; set; }

        [Required]
        public int CountryId { get; set; }

        public IFormFile? ProfileImage { get; set; }
        public byte[]? ProfileImageBytes { get; set; }
        public string? ProfileImageBase64
        {
            get
            {
                if(ProfileImageBytes != null && ProfileImageBytes.Length > 0)
                {
                    return Convert.ToBase64String(ProfileImageBytes);
                }
                return null;
            }
        }
    }
}
