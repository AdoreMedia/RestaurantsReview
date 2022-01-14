using RestaurantsReview.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using RestaurantsReview.Client.Shared.Navigation;
using Microsoft.AspNetCore.Components;
using RestaurantsReview.Client.Models;
using RestaurantsReview.Client.Utility;

namespace RestaurantsReview.ApiClient
{
	public partial class RestaurantsReviewClient
	{
		private const string XApiKey = "x-api-key";
		private const string XClientVersion = "x-client-version";

		partial void PrepareRequest(System.Net.Http.HttpClient client, System.Net.Http.HttpRequestMessage request, string url)
		{
			AppModel model = Program.Host.Services.GetRequiredService<AppModel>();

			var items1 = new[] { model.User?.Username, model.User?.Password }.Where(o => !string.IsNullOrEmpty(o)).ToList();
			if (items1.Count == 2)
			{
				string key1 = string.Join("|", items1);
				request.Headers.Add(XApiKey, key1);
			}

			request.Headers.Add(XClientVersion, BlazorClientInfo.Version);
		}

		partial void ProcessResponse(System.Net.Http.HttpClient client, System.Net.Http.HttpResponseMessage response)
		{
			if(response.StatusCode == System.Net.HttpStatusCode.Gone)
			{
				response.StatusCode = System.Net.HttpStatusCode.OK;

				BrowserContext model = Program.Host.Services.GetRequiredService<BrowserContext>();
				_ = model.RemoveBlazorOfflineCacheAndReloadAsync();
			}
			else if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized || response.StatusCode == System.Net.HttpStatusCode.Forbidden)
			{
				response.StatusCode = System.Net.HttpStatusCode.OK;

				AppModel model = Program.Host.Services.GetRequiredService<AppModel>();
				_ = model.LogOutUserAsync();
			}
		}

	}
}
