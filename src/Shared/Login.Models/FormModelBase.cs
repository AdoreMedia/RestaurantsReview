using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace RestaurantsReview.Client.Pages.Authentication.Login.Models
{
	public abstract class FormModelBase
	{
		public string GetDisplayName(string propertyname)
		{
			MemberInfo myprop = this.GetType().GetProperty(propertyname) as MemberInfo;
			var dd = myprop.GetCustomAttribute(typeof(DisplayAttribute)) as DisplayAttribute;
			return dd?.Name ?? "";
		}
	}
}
