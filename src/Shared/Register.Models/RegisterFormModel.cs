using RestaurantsReview.Client.Pages.Authentication.Login.Models;
using System.ComponentModel.DataAnnotations;

namespace RestaurantsReview.Client.Pages.Authentication.Register.Models
{
	public class RegisterFormModel : LoginFormModel
	{
		[Display(Name = "Email")]
		[Required]
		[EmailAddress]
		[MinLength(3)]
		public string Email { get; set; }

		[Display(Name = "Repeat password")]
		[Required]
		[MinLength(3)]
		public string RepeatPassword { get; set; }
	}
}
