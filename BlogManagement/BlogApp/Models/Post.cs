using BlogApp.Enums;

namespace BlogApp.Models
{
	public class Post
	{
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public int LikeCount { get; set; }
        public double AverageRate { get; set; }
        public int RateCount { get; set; }
        public DateTime CreateDate { get; set; } = DateTime.Now;
        public PostStatus Status { get; set; }

		public int UserId { get; set; }
		public User User { get; set; }
        public int CategoryId { get; set; }
        public Category Category { get; set; }
		public IEnumerable<Comment> Comments { get; set; }
		public IEnumerable<Rating> Ratings { get; set; }
		public IEnumerable<Like> Likes { get; set; }
	}
}
