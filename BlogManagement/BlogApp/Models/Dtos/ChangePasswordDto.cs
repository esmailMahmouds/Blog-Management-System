using System.ComponentModel.DataAnnotations;

namespace BlogApp.Models.Dtos
{
	public class ChangePasswordDto
	{
		[Required]
		public string OldPassword { get; set; } = string.Empty;

		[Required]
		[StringLength(100, MinimumLength = 6,ErrorMessage = "Password minimum length is 6 characters")]
		public string NewPassword { get; set; } = string.Empty;
	}
}
