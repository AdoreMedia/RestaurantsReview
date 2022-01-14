using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using RestaurantsReview.Database;
using System.Linq;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using RestaurantsReview.Server.Authentication;
using RestaurantsReview.Server.Authentication.Basic;
using System.Collections.Concurrent;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using RestaurantsReview.Server.Authorization.Policy;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Http;
using RestaurantsReview.Configuration;
using System.Threading;
using System.Net;

namespace RestaurantsReview.Server
{
	public class Startup
	{
		public Startup(IConfiguration configuration)
		{
			Configuration = configuration;
		}

		public IConfiguration Configuration { get; }

		IServiceScopeFactory ScopeFactory { get; set; }

		// This method gets called by the runtime. Use this method to add services to the container.
		// For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
		public void ConfigureServices(IServiceCollection services)
		{
			services.AddDbContext<dbRestaurantsReviewDataContext>(options =>
				options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"))
				//options.UseSqlServer(SiteHelper.CurrentSiteSetting.DatabaseConnectionString)
			);

			services.AddControllersWithViews();
			services.AddRazorPages();
			services.AddSwaggerGen(c =>
			{
				c.SwaggerDoc("v1", new OpenApiInfo { Title = "RestaurantsReivew", Version = "v1" });
			});

			services.AddAuthentication(CalystoAuthentication.SchemeName)
			.AddScheme<BasicAuthenticationOptions, BasicAuthenticationHandler>(CalystoAuthentication.SchemeName, options =>
			{
				options.Realm = CalystoAuthentication.Realm;
				options.ValidateUsername = (username, pass) =>
				{
					if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(pass))
						return null;

					using (var scope = this.ScopeFactory.CreateScope())
					{
						dbRestaurantsReviewDataContext dbcontext = scope.ServiceProvider.GetRequiredService<dbRestaurantsReviewDataContext>();
						tblMember member = dbcontext.tblMember.Where(o => o.Username == username)
							.Include(p => p.tblMemberRoleList)
							.ThenInclude(p => p.tblRole)
							.FirstOrDefault();

						if (member == null)
							return null;

						var roles = member.tblMemberRoleList.Select(r => r.tblRole).ToList();

						List<Claim> claims = new List<Claim>() {
							new Claim(ClaimTypes.Name, username),
							new Claim(nameof(tblMember.MemberID), member.MemberID.ToString())
						};

						roles.ForEach(r => claims.Add(new Claim(ClaimTypes.Role, r.RoleName)));

						return claims;
					}
				};
			});

			services.AddSingleton<IAuthorizationHandler, PolicyAuthorizationHandler>();
			services.AddAuthorization(options =>
			{
				options.AddPolicy(CalystoAuthentication.PolicyName, policy => policy.Requirements.Add(new PolicyAuthorizationRequirement()
				{
					Policy = CalystoAuthentication.PolicyName,
					FuncIsAuthorized = (AuthorizationHandlerContext context, PolicyAuthorizationRequirement requirement) =>
					{
						if (!context.User.Identity.IsAuthenticated)
							return false;

						if (string.IsNullOrWhiteSpace(context.User.Identity.Name))
							return false;

						using (var scope = this.ScopeFactory.CreateScope())
						{
							int memberId = Convert.ToInt32(context.User.FindFirst(nameof(tblMember.MemberID)).Value);
							dbRestaurantsReviewDataContext dbcontext = scope.ServiceProvider.GetRequiredService<dbRestaurantsReviewDataContext>();
							tblMember member = dbcontext.tblMember.Where(o => o.MemberID == memberId)
								.Include(p => p.tblMemberRoleList)
								.ThenInclude(p => p.tblRole)
								.FirstOrDefault();
							var roles = member.tblMemberRoleList.Select(r => r.tblRole).ToList();

							List<CalystoAuthorizeAttribute> attrList = null;

							if (context.Resource is DefaultHttpContext cx)
							{
								// net5.0
								attrList = cx.GetEndpoint().Metadata.OfType<CalystoAuthorizeAttribute>().Where(a=>a.Permission != null).ToList();
							}
							else
							{
								return false;
							}

							if(attrList == null || !attrList.Any())
							{
								// There is no Authorize attribute on action.
								return true;
							}
							else
							{
								// Permission is property name in tblRole
								// we're testing if user has tblRole.[Permission] property == true
								return attrList.SelectMany(attr => 
									roles.Select(role => (bool) role.GetType().GetProperty(attr.Permission).GetValue(role))
								).Where(v=>v)
								.Any();
							}
						}
					}
				}));
			});
		}

		private const string XClientVersion = "x-client-version";

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IServiceScopeFactory scopeFactory)
		{
			this.ScopeFactory = scopeFactory;

			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
				app.UseWebAssemblyDebugging();
				app.UseSwagger();
				// https://localhost:44337/swagger/v1/swagger.json
				app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "RestaurantsReivew v1"));
			}
			else
			{
				app.UseExceptionHandler("/Error");
				// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
				app.UseHsts();
			}

			app.Use(async (context, next) =>
			{
				string client1 = RestaurantsReview.Client.BlazorClientInfo.Version;

				if (context.Request.Headers.TryGetValue(XClientVersion, out var value) && client1 != value)
				{
					context.Response.StatusCode = (int)HttpStatusCode.Gone;
					context.Response.GetTypedHeaders().CacheControl = new Microsoft.Net.Http.Headers.CacheControlHeaderValue()
					{
						NoCache = true,
						NoStore = true
					};
				}
				else
				{
					await next();
				}
			});

			app.UseHttpsRedirection();
			app.UseBlazorFrameworkFiles();
			app.UseStaticFiles();

			app.UseAuthentication(); // check for default scheme only

			app.Use(async (contect, next) =>
			{
				await next();
			});

			app.UseRouting();

			app.Use(async (context, next) =>
			{
				await next();
			});

			app.UseAuthorization();

			app.UseEndpoints(endpoints =>
			{
				endpoints.MapRazorPages();
				endpoints.MapControllers();
				endpoints.MapFallbackToPage("/_home");
				//endpoints.MapFallbackToFile("index.html");
			});
		}
	}
}
