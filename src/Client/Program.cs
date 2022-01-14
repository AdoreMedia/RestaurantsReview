using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using RestaurantsReview.ApiClient;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using RestaurantsReview.Client.Models;
using RestaurantsReview.Client.Utility;
using System.Globalization;

namespace RestaurantsReview.Client
{
	public class Program
	{
		public static WebAssemblyHost Host { get; private set; }

		public static async Task Main(string[] args)
		{
			CultureInfo _hr = CultureInfo.GetCultureInfo("hr-HR");
			CultureInfo.DefaultThreadCurrentCulture = _hr;
			CultureInfo.DefaultThreadCurrentUICulture = _hr;
			CultureInfo.CurrentCulture = _hr;
			CultureInfo.CurrentUICulture = _hr;

			var builder = WebAssemblyHostBuilder.CreateDefault(args);
			builder.RootComponents.Add<App>("#app");

			builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

			builder.Services.AddScoped(sp => new RestaurantsReviewClient(builder.HostEnvironment.BaseAddress, new HttpClient()));

			builder.Services.AddSingleton<BrowserContext>();
			
			builder.Services.AddSingleton<AppModel>();

			Host = builder.Build();
			
			await Host.RunAsync();
		}
	}
}
