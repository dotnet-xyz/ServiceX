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
		private Dictionary<Guid, ModelA> _storage = new Dictionary<Guid, ModelA>();

		public Task CreateAsync(ModelA model, CancellationToken ct)
		{
			try
			{
				lock (_storage)
				{
					_storage.Add(model.Id, model.Clone());
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
			lock (_storage)
			{
				_storage.TryGetValue(id, out model);
			}
			return Task.FromResult<ModelA>(model?.Clone());
		}

		public Task<int> UpdateAsync(ModelA model, CancellationToken ct)
		{
			int count = 0;
			lock (_storage)
			{
				if (_storage.ContainsKey(model.Id))
				{
					_storage[model.Id] = model.Clone();
					count = 1;
				}
			}
			return Task.FromResult(count);
		}

		public Task<int> DeleteAsync(Guid id, CancellationToken ct)
		{
			int count = 0;
			lock (_storage)
			{
				count = _storage.Remove(id) ? 1 : 0;
			}
			return Task.FromResult(count);
		}
	}
}