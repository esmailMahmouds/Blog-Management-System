using System.ComponentModel.DataAnnotations.Schema;

namespace BlogApp.Models.DomainClasses
{
	public class Follow
	{
        public int Id { get; set; }
        public int FollowerUserId { get; set; }
        public User FollowerUser { get; set; }
		public int FollowingUserId { get; set; }
        public User FollowingUser { get; set; }
        public DateTime CreateDate { get; set; } = DateTime.Now;
    }
}
