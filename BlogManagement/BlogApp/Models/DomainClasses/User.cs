using System.ComponentModel.DataAnnotations.Schema;
using BlogApp.Enums;

namespace BlogApp.Models.DomainClasses
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public DateOnly DateOfBirth { get; set; }
        public Role Role { get; set; } = Role.Author;
        public double? AverageRate { get; set; }
        public string? ImageURL { get; set; }
        public byte[]? ProfileImage { get; set; }

        public int CountryId { get; set; }
        public Country? Country { get; set; }
        public IEnumerable<Comment>? Comments { get; set; }
        public IEnumerable<Rating>? Ratings { get; set; }
        public IEnumerable<Like>? Likes { get; set; }
        public IEnumerable<Follow>? Followers { get; set; }
        public IEnumerable<Follow>? Followings { get; set; }


    }
}
