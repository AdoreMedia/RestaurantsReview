using RestaurantsReview.Client.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RestaurantsReview.Client.Pages.Authentication.Login.Models
{
	public class LoginFormModel : FormModelBase
	{
		[Display(Name = "Username")]
		[Required]
		[MinLength(3)]
		public string Username { get; set; }

		[Display(Name = "Password")]
		[Required]
		[MinLength(3)]
		public string Password { get; set; }
	}
}
