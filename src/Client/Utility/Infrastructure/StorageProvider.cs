using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestaurantsReview.Client.Utility.Infrastructure
{
	public class StorageProvider
	{
		private IJSRuntime _jSRuntime;
		private string _storageName;

		public StorageProvider(IJSRuntime jSRuntime, string storageName)
		{
			_jSRuntime = jSRuntime;
			_storageName = storageName;
		}

		public ValueTask SetItem<TItem>(string key, TItem value)
		{
			string json = System.Text.Json.JsonSerializer.Serialize(value);
			return _jSRuntime.InvokeVoidAsync($"{_storageName}.setItem", key, json);
		}

		public async ValueTask<TItem> GetItem<TItem>(string key)
		{
			string json = await _jSRuntime.InvokeAsync<string>($"{_storageName}.getItem", key);
			if (string.IsNullOrEmpty(json))
				return await ValueTask.FromResult<TItem>(default);
			else
				return System.Text.Json.JsonSerializer.Deserialize<TItem>(json);
		}

		public ValueTask Clear()
		{
			return _jSRuntime.InvokeVoidAsync($"{_storageName}.clear");
		}

		public ValueTask RemoveItem(string key)
		{
			return _jSRuntime.InvokeVoidAsync($"{_storageName}.removeItem", key);
		}
	}
}
