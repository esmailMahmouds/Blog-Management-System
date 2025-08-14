using System.ComponentModel.DataAnnotations;

namespace BlogApp.Models.Dtos
{
	public class FollowListingDto
	{
        public int UserId { get; set; }
        public string Name { get; set; }
		public byte[]? ProfileImageBytes { get; set; }
		public string? ProfileImageBase64
		{
			get
			{
				if (ProfileImageBytes != null && ProfileImageBytes.Length > 0)
				{
					return Convert.ToBase64String(ProfileImageBytes);
				}
				return null;
			}
		}
	}
}
