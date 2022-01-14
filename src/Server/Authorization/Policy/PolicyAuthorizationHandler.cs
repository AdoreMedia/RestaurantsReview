using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace RestaurantsReview.Server.Authorization.Policy
{
    public class PolicyAuthorizationHandler : AuthorizationHandler<PolicyAuthorizationRequirement>
    {
        public PolicyAuthorizationHandler()
        {
        }

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, PolicyAuthorizationRequirement requirement)
        {
            // you may get endpoint metadata:
            // ((Microsoft.AspNetCore.Routing.RouteEndpoint)context.Resource).Metadata
            // and so, access all attributes from Metadata

            if (requirement.FuncIsAuthorized.Invoke(context, requirement))
            {
                context.Succeed(requirement);
            }
            else
            {
                context.Fail();
            }
            return Task.CompletedTask;
        }
    }
}
