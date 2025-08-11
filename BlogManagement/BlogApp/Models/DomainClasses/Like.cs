namespace BlogApp.Models.DomainClasses
{
	public class Like
	{
        public int Id { get; set; }
        public DateTime CreateDate { get; set; } = DateTime.Now;

		public int PostId { get; set; }
		public Post Post { get; set; }
		public int UserId { get; set; }
		public User User { get; set; }
	}
}
