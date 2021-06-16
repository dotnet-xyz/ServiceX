// Dotnet-XYZ © 2021

using DotnetXYZ.ServiceX.Api;
using Microsoft.AspNetCore.WebUtilities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading;
using System.Threading.Tasks;

namespace DotnetXYZ.ServiceX.Client
{
	public class ContractAClient : IContractA
	{
		private readonly HttpClient _Client;

		public ContractAClient(HttpClient client)
		{
			_Client = client ?? throw new ArgumentNullException(nameof(client));
		}

		public async Task CreateAsync(ModelA model, CancellationToken ct)
		{
			using var content = JsonContent.Create(model);
			using HttpResponseMessage response = await _Client.PostAsync("ContractA", content, ct);
			if (response.StatusCode == HttpStatusCode.Conflict)
			{
				throw new ContractAIdAlreadyExistsException(model.Id, null);
			}
			response.EnsureSuccessStatusCode();
		}

		public async Task<ModelA> GetAsync(Guid id, CancellationToken ct)
		{
			string url = QueryHelpers.AddQueryString(
				"ContractA",
				new Dictionary<string, string>
				{
					{ "id", id.ToString() }
				});
			using HttpResponseMessage response = await _Client.GetAsync(url, ct);
			if (response.StatusCode == HttpStatusCode.NotFound)
			{
				return null;
			}
			response.EnsureSuccessStatusCode();
			string body = await response.Content.ReadAsStringAsync(ct);
			return JsonConvert.DeserializeObject<ModelA>(body);
		}

		public async Task<int> UpdateAsync(ModelA model, CancellationToken ct)
		{
			using var content = JsonContent.Create(model);
			using HttpResponseMessage response = await _Client.PutAsync("ContractA", content, ct);
			if (response.StatusCode == HttpStatusCode.NotFound)
			{
				return 0;
			}
			response.EnsureSuccessStatusCode();
			return 1;
		}

		public async Task<int> DeleteAsync(Guid id, CancellationToken ct)
		{
			string url = QueryHelpers.AddQueryString(
				"ContractA",
				new Dictionary<string, string>
				{
					{ "id", id.ToString() }
				});
			using HttpResponseMessage response = await _Client.DeleteAsync(url, ct);
			if (response.StatusCode == HttpStatusCode.NotFound)
			{
				return 0;
			}
			response.EnsureSuccessStatusCode();
			return 1;
		}
	}
}