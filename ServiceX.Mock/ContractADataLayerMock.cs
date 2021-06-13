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
				_storage.Add(model.Id, model.Clone());
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

		public Task UpdateAsync(ModelA model, CancellationToken ct)
		{
			lock (_storage)
			{
				if (!_storage.ContainsKey(model.Id))
				{
					throw new ContractAIdNotFoundException(model.Id, null);
				}
				_storage[model.Id] = model.Clone();
			}
			return Task.CompletedTask;
		}

		public Task DeleteAsync(Guid id, CancellationToken ct)
		{
			lock (_storage)
			{
				if (!_storage.Remove(id))
				{
					throw new ContractAIdNotFoundException(id, null);
				}
			}
			return Task.CompletedTask;
		}
	}
}