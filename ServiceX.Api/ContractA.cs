// Dotnet-XYZ © 2021

using System;
using System.Threading;
using System.Threading.Tasks;

namespace DotnetXYZ.ServiceX.Api
{
	public interface IContractA
	{
		Task CreateAsync(ModelA model, CancellationToken ct);
		Task<ModelA> GetAsync(Guid id, CancellationToken ct);
		Task<int> UpdateAsync(ModelA model, CancellationToken ct);
		Task<int> DeleteAsync(Guid id, CancellationToken ct);
	}
}