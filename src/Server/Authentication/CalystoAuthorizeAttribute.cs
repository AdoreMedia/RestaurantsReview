namespace RestaurantsReview.Server.Authentication
{
	public class CalystoAuthorizeAttribute : Microsoft.AspNetCore.Authorization.AuthorizeAttribute
	{
		public CalystoAuthorizeAttribute() : base()
		{
			base.Policy = CalystoAuthentication.PolicyName;
			base.AuthenticationSchemes = CalystoAuthentication.SchemeName;
		}

		/// <summary>
		/// Required permission name.
		/// </summary>
		public string Permission { get; set; }
	}
}
