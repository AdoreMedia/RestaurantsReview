using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.JSInterop;
using RestaurantsReview.Client.Utility.Infrastructure;

namespace RestaurantsReview.Client.Utility
{
	public class DomElement
	{
		public string tagName { get; set; }
		public string id { get; set; }
		public string name { get; set; }
		public string style { get; set; }
		public string className { get; set; }
		public int offsetWidth { get; set; }
		public int offsetHeight { get; set; }
		public string value { get; set; }
		public string type { get; set; }
	}

	public class BrowserContext
	{
		IJSRuntime _jSRuntime;

		public BrowserContext(IJSRuntime jSRuntime)
		{
			_jSRuntime = jSRuntime;
		}

		StorageProvider _sessionStorage;
		
		public StorageProvider SessionStorage => _sessionStorage ?? (_sessionStorage = new StorageProvider(_jSRuntime, "sessionStorage"));

		StorageProvider _localStorage;

		public StorageProvider LocalStorage => _localStorage ?? (_localStorage = new StorageProvider(_jSRuntime, "localStorage"));

		public async Task<DomElement> GetElementByIdAsync(string id)
		{
			return await _jSRuntime.InvokeAsync<DomElement>("Calysto.Blazor.Interop.GetElementById", id);
		}

		public async Task<IEnumerable<DomElement>> GetElementsByTagNameAsync(string tagName, int skip = 0, int take = int.MaxValue)
		{
			return await _jSRuntime.InvokeAsync<List<DomElement>>("Calysto.Blazor.Interop.GetElementsByTagName", tagName, skip, take);
		}

		public async Task<IEnumerable<DomElement>> GetElementsByClassNameAsync(string clasName, int skip = 0, int take = int.MaxValue)
		{
			return await _jSRuntime.InvokeAsync<List<DomElement>>("Calysto.Blazor.Interop.GetElementsByClassName", clasName, skip, take);
		}

		public async Task<IEnumerable<DomElement>> GetElementsByNameAsync(string name, int skip = 0, int take = int.MaxValue)
		{
			return await _jSRuntime.InvokeAsync<List<DomElement>>("Calysto.Blazor.Interop.GetElementsByName", name, skip, take);
		}

		public async Task<KeyboardEventArgs> WaitForKeyboardEventAsync(string eventName, string targetElementId = null)
		{
			return await _jSRuntime.InvokeAsync<KeyboardEventArgs> ("Calysto.Blazor.Interop.WaitForKeyboardEventAsync", eventName, targetElementId);
		}

		public async Task<MouseEventArgs> WaitForMouseEventAsync(string eventName, string targetElementId = null)
		{
			return await _jSRuntime.InvokeAsync<MouseEventArgs>("Calysto.Blazor.Interop.WaitForMouseEventAsync", eventName, targetElementId);
		}

		public async Task RemoveBlazorOfflineCacheAndReloadAsync()
		{
			await _jSRuntime.InvokeVoidAsync("Calysto.Blazor.Interop.RemoveBlazorOfflineCacheAndReloadAsync");
		}

		public async Task LocationReloadAsync()
		{
			await _jSRuntime.InvokeVoidAsync("location.reaload", true);
		}

	}
}
