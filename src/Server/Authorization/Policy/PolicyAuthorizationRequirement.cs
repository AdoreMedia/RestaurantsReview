using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace RestaurantsReview.Server.Authorization.Policy
{
	public class PolicyAuthorizationRequirement : IAuthorizationRequirement
	{
		public string Policy { get; set; }

		public Func<AuthorizationHandlerContext, PolicyAuthorizationRequirement, bool> FuncIsAuthorized { get; set; }
	}
}
