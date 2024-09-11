using System.ComponentModel.DataAnnotations;

public class LoginViewModel
{
	[Required]
	public string UserName { get; set; } = default!;

	[Required]
	public string Password { get; set; } = default!;
}