using Calysto.Serialization.Json;
using Calysto.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Net.Http.Headers;
using System;
using System.Linq;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace RestaurantsReview.Server.Authentication.Basic
{
	public class BasicAuthenticationHandler : AuthenticationHandler<BasicAuthenticationOptions>
    {
        public BasicAuthenticationHandler(
			IOptionsMonitor<BasicAuthenticationOptions> options,
			ILoggerFactory logger,
			UrlEncoder encoder,
			ISystemClock clock) : base(options, logger, encoder, clock)
        {
		}

		protected override Task<AuthenticateResult> HandleAuthenticateAsync()
		{
            return Task.FromResult(this.AuthenticateWorker());
        }

		protected override Task HandleChallengeAsync(AuthenticationProperties properties)
		{
            //Response.Headers["WWW-Authenticate"] = $"{AUTH_SCHEME_NAME} realm=\"{Options.Realm}\", charset=\"UTF-8\"";
            Response.StatusCode = 401;
            Response.GetTypedHeaders().CacheControl = new Microsoft.Net.Http.Headers.CacheControlHeaderValue()
            {
                NoCache = true,
                NoStore = true
            };
            return base.HandleChallengeAsync(properties);
		}

        protected override Task HandleForbiddenAsync(AuthenticationProperties properties)
        {
            //Response.Headers["WWW-Authenticate"] = $"{AUTH_SCHEME_NAME} realm=\"{Options.Realm}\", charset=\"UTF-8\"";
            Response.StatusCode = 403;
            Response.GetTypedHeaders().CacheControl = new Microsoft.Net.Http.Headers.CacheControlHeaderValue()
            {
                NoCache = true,
                NoStore = true
            };
            return base.HandleChallengeAsync(properties); // return 401, show dialog with user & pass again
        }

        private const string XApiKey = "x-api-key";

        private AuthenticateResult AuthenticateWorker()
        {
            if (!Request.Headers.TryGetValue(XApiKey, out var apiKey))
            {
                //Authorization header not in request
                return AuthenticateResult.NoResult();
            }

            try
            {
                string[] parts = apiKey.ToString().Split('|');
                string username = parts[0];
                string password = parts[1];

                var claims = this.Options.ValidateUsername.Invoke(username, password)?.ToArray();

                if (claims?.Any() != true)
                {
                    return AuthenticateResult.Fail("Invalid username or password");
                }

                return AuthenticateResult.Success(CreateTicket(claims));
            }
			catch(Exception ex)
			{
                return AuthenticateResult.Fail(ex.Message);
            }
        }

        private AuthenticationTicket CreateTicket(Claim[] claims)
        {
            //var claims = new[] { new Claim(ClaimTypes.Name, user), new Claim(ClaimTypes.Role, "role1") };
            var identity = new ClaimsIdentity(claims, Scheme.Name);
            var principal = new ClaimsPrincipal(identity);
            var ticket = new AuthenticationTicket(principal, Scheme.Name);
            return ticket;
        }
    }
}