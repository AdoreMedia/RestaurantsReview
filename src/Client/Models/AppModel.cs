using Microsoft.AspNetCore.Components;
using RestaurantsReview.ApiClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.Extensions.DependencyInjection;
using RestaurantsReview.Client.Utility;
using System.Threading.Tasks;
using Calysto.ComponentModel;
using System.Threading;

namespace RestaurantsReview.Client.Models
{
	public class AppModel : BindableBase<AppModel>
	{
		BrowserContext _browser;
		IServiceProvider _serviceProvider;
		IServiceScopeFactory _scopeFactory;
		NavigationManager _nav;

		public AppModel(BrowserContext browser, IServiceProvider serviceProvider, IServiceScopeFactory scopeFactory, NavigationManager nav)
		{
			_browser = browser;
			_serviceProvider = serviceProvider;
			_scopeFactory = scopeFactory;
			_nav = nav;
		}

		public UserInfo User{ get => base.Get(o => o.User); set => base.Set(o => o.User, value); }

		public bool IsAuthenticated => !string.IsNullOrWhiteSpace(this.User?.Username);

		public List<tblRole> Roles { get => base.Get(o => o.Roles); set => base.Set(o => o.Roles, value); }

		private string GetName<TItem, TValue>(Expression<Func<TItem, TValue>> expression)
		{
			return ((MemberExpression)expression.Body).Member.Name;
		}

		public IEnumerable<string> GetAssignedPermissions()
		{
			return (this.Roles ?? new List<tblRole>()).SelectMany(role => role.GetType().GetProperties()
				.Where(p => p.PropertyType == typeof(bool) && (bool)p.GetValue(role))
				.Select(p => p.Name)
			).Distinct();
		}

		public IEnumerable<string> GetAssignedRoles()
		{
			return (this.Roles ?? new List<tblRole>()).Select(o => o.RoleName).Distinct();
		}

		public bool HasPermission(Expression<Func<tblRole, bool>> expression)
		{
			if (!(this.Roles?.Any() == true))
				return false;

			string propName = this.GetName(expression);
			return this.Roles.Select(r => r.GetType().GetProperty(propName)?.GetValue(r)).OfType<bool>().Where(o => o).Any();
		}

		public bool HasRole(RoleEnum name)
		{
			string name1 = name.ToString();
			return this.Roles?.Where(o => o.RoleName == name1).Any() == true;
		}

		public void RequirePermission(Expression<Func<tblRole, bool>> expression)
		{
			if (!this.HasPermission(expression))
			{
				_ = LogOutUserAsync();
			}
		}

		public class UserInfo: BindableBase<UserInfo>
		{
			public string FirstName { get => base.Get(o => o.FirstName); set => base.Set(o => o.FirstName, value); }
			public string LastName { get => base.Get(o => o.LastName); set => base.Set(o => o.LastName, value); }
			public string Username { get => base.Get(o => o.Username); set => base.Set(o => o.Username, value); }
			public string Password { get => base.Get(o => o.Password); set => base.Set(o => o.Password, value); }

			public string GetDisplayName()
			{
				var name1 = new[] { this.FirstName, this.LastName }.Where(o => !string.IsNullOrWhiteSpace(o)).ToList();
				if (name1.Any())
				{
					return string.Join(" ", name1);
				}
				else
				{
					return this.Username;
				}
			}
		}

		private async Task SaveUserInfoAsync(UserInfo user)
		{
			await _browser.SessionStorage.SetItem(nameof(UserInfo), user);
			this.User = user;
		}

		internal async Task LogOutUserAsync()
		{
			await _browser.SessionStorage.RemoveItem(nameof(UserInfo));
			this.User = null;
		}

		internal async Task<LoginUserResult> LoginUserAsync(string username, string password)
		{
			using (var scope = _scopeFactory.CreateScope())
			{
				var api = scope.ServiceProvider.GetRequiredService<RestaurantsReviewClient>();
				LoginUserResult result1 = await api.LoginUserAsync(username, password);
				if (!result1.IsError)
				{
					await this.SaveUserInfoAsync(new UserInfo() { 
						Username = username,
						Password = password,
						FirstName = result1.FirstName,
						LastName = result1.LastName
					});

					this.Roles = (await api.GetCurrentUserPermissionsAsync()).ToList();
				}
				else
				{
					// remove saved user info
					await LogOutUserAsync();
				}
				return result1;
			}
		}

		/// <summary>
		/// Restore user info and login user after page refresh.
		/// Func is invoked from multiple controls at app startup.
		/// </summary>
		/// <returns></returns>
		internal async Task RestoreUserInfoAsync()
		{
			if (!this.IsAuthenticated)
			{
				var usr = await _browser.SessionStorage.GetItem<UserInfo>(nameof(UserInfo));
				if (usr?.Username != null && usr?.Password != null)
				{
					await this.LoginUserAsync(usr.Username, usr.Password);
				}
			}

			if (!this.IsAuthenticated)
			{
				await LogOutUserAsync();
			}
		}
	}
}
