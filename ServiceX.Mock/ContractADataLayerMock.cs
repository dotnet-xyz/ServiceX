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

		public delegate void CreateAsyncEvent(ContractADataLayerMock mock, ModelA model, CancellationToken ct);

		public event CreateAsyncEvent BeforeCreateAsync;

		public Task CreateAsync(ModelA model, CancellationToken ct)
		{
			BeforeCreateAsync?.Invoke(this, model, ct);

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

		public delegate void GetAsyncEvent(ContractADataLayerMock mock, Guid id, CancellationToken ct);

		public event GetAsyncEvent BeforeGetAsync;

		public Task<ModelA> GetAsync(Guid id, CancellationToken ct)
		{
			BeforeGetAsync?.Invoke(this, id, ct);

			ModelA model;
			lock (_Storage)
			{
				_Storage.TryGetValue(id, out model);
			}
			return Task.FromResult<ModelA>(model?.Clone());
		}

		public delegate void UpdateAsyncEvent(ContractADataLayerMock mock, ModelA model, CancellationToken ct);

		public event UpdateAsyncEvent BeforeUpdateAsync;

		public Task<int> UpdateAsync(ModelA model, CancellationToken ct)
		{
			BeforeUpdateAsync?.Invoke(this, model, ct);

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

		public delegate void DeleteAsyncEvent(ContractADataLayerMock mock, Guid id, CancellationToken ct);

		public event DeleteAsyncEvent BeforeDeleteAsync;

		public Task<int> DeleteAsync(Guid id, CancellationToken ct)
		{
			BeforeDeleteAsync?.Invoke(this, id, ct);

			int count = 0;
			lock (_Storage)
			{
				count = _Storage.Remove(id) ? 1 : 0;
			}
			return Task.FromResult(count);
		}
	}
}