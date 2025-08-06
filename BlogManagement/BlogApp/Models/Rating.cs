namespace BlogApp.Models
{
	public class Rating
	{
        public int Id { get; set; }
        public double Value { get; set; }

		public int PostId { get; set; }
		public Post Post { get; set; }
		public int UserId { get; set; }
		public User User { get; set; }
	}
}
