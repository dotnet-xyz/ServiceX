// Dotnet-XYZ © 2021

using DotnetXYZ.ServiceX.Api;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace DotnetXYZ.ServiceX
{
	public class ContractA : IContractA
	{
		private IContractADataLayer _DataLayer;

		public ContractA(IContractADataLayer dataLayer)
		{
			_DataLayer = dataLayer;
		}

		public async Task CreateAsync(ModelA model, CancellationToken ct)
		{
			await _DataLayer.CreateAsync(model, ct);
		}

		public async Task<ModelA> GetAsync(Guid id, CancellationToken ct)
		{
			return await _DataLayer.GetAsync(id, ct);
		}

		public async Task UpdateAsync(ModelA model, CancellationToken ct)
		{
			await _DataLayer.UpdateAsync(model, ct);
		}

		public async Task DeleteAsync(Guid id, CancellationToken ct)
		{
			await _DataLayer.DeleteAsync(id, ct);
		}
	}
}