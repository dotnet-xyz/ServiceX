// Dotnet-XYZ © 2021

using System;
using System.Threading;
using System.Threading.Tasks;

namespace DotnetXYZ.ServiceX.Api
{
	public interface IContractADataLayer
	{
		Task CreateAsync(ModelA model, CancellationToken ct);
		Task<ModelA> GetAsync(Guid id, CancellationToken ct);
		Task UpdateAsync(ModelA model, CancellationToken ct);
		Task DeleteAsync(Guid id, CancellationToken ct);
	}
}