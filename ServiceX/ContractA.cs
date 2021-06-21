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
			_DataLayer = dataLayer ?? throw new ArgumentNullException(nameof(dataLayer));
		}

		public async Task CreateAsync(ModelA model, CancellationToken ct)
		{
			if (model == null)
			{
				throw new ArgumentNullException(nameof(model));
			}
			if (model.Data == null)
			{
				throw new ArgumentException("Data is null", nameof(model));
			}

			await _DataLayer.CreateAsync(model, ct);
		}

		public async Task<ModelA> GetAsync(Guid id, CancellationToken ct)
		{
			return await _DataLayer.GetAsync(id, ct);
		}

		public async Task<int> UpdateAsync(ModelA model, CancellationToken ct)
		{
			if (model == null)
			{
				throw new ArgumentNullException(nameof(model));
			}
			if (model.Data == null)
			{
				throw new ArgumentException("Data is null", nameof(model));
			}

			return await _DataLayer.UpdateAsync(model, ct);
		}

		public async Task<int> DeleteAsync(Guid id, CancellationToken ct)
		{
			return await _DataLayer.DeleteAsync(id, ct);
		}
	}
}