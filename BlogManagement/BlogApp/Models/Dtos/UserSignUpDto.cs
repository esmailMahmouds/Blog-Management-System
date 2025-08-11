using BlogApp.Enums;
using BlogApp.Models.DomainClasses;
using System.ComponentModel.DataAnnotations;

namespace BlogApp.Models.Dtos
{
    public class UserSignUpDto
    {

        [Required]
        [StringLength(100, MinimumLength = 2)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        [StringLength(254, MinimumLength = 3)]
        public string Email { get; set; } = string.Empty;

        [Required]
        [StringLength(100, MinimumLength = 6)]
        public string Password { get; set; } = string.Empty;

        [Required]
        public DateTime DateOfBirth { get; set; } = DateTime.Now;

        [Required]
        public int CountryId { get; set; }

        public User ToUser()
        {
            return new User
            {
                Name = Name,
                Email = Email,
                Password = Password,
                DateOfBirth = DateOnly.FromDateTime(DateOfBirth),
                Role = Role.Author,
                AverageRate = 0,
                CountryId = 1 //for now, set default country id
            };
        }
    }
}
