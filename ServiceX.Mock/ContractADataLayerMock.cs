// Dotnet-XYZ © 2021

using DotnetXYZ.ServiceX.Api;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace DotnetXYZ.ServiceX.Mock
{
	public class ContractADataLayerMock : IContractADataLayer
	{
		private readonly Dictionary<Guid, ModelA> _Storage = new();

		public Task CreateAsync(ModelA model, CancellationToken ct)
		{
			try
			{
				lock (_Storage)
				{
					_Storage.Add(model.Id, model.Clone());
				}
			}
			catch (ArgumentException e)
			{
				throw new ContractAIdAlreadyExistsException(model.Id, e);
			}
			return Task.CompletedTask;
		}

		public Task<ModelA> GetAsync(Guid id, CancellationToken ct)
		{
			ModelA model;
			lock (_Storage)
			{
				_Storage.TryGetValue(id, out model);
			}
			return Task.FromResult<ModelA>(model?.Clone());
		}

		public Task<int> UpdateAsync(ModelA model, CancellationToken ct)
		{
			int count = 0;
			lock (_Storage)
			{
				if (_Storage.ContainsKey(model.Id))
				{
					_Storage[model.Id] = model.Clone();
					count = 1;
				}
			}
			return Task.FromResult(count);
		}

		public Task<int> DeleteAsync(Guid id, CancellationToken ct)
		{
			int count = 0;
			lock (_Storage)
			{
				count = _Storage.Remove(id) ? 1 : 0;
			}
			return Task.FromResult(count);
		}
	}
}