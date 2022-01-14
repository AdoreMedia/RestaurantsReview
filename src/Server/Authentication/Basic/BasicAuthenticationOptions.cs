using Microsoft.AspNetCore.Authentication;
using RestaurantsReview.Database;
using System;
using System.Collections.Generic;
using System.Security.Claims;

namespace RestaurantsReview.Server.Authentication.Basic
{ 
    public class BasicAuthenticationOptions : AuthenticationSchemeOptions
    {
        public string Realm { get; set; }

        public Func<string, string, IEnumerable<Claim>> ValidateUsername { get; set; }

    }
}