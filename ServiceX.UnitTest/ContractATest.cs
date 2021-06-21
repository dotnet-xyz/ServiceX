// Dotnet-XYZ © 2021

using DotnetXYZ.ServiceX.Api;
using DotnetXYZ.ServiceX.Mock;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace DotnetXYZ.ServiceX.UnitTest
{
	[TestClass]
	public class ContractATest
	{
		public class Context
		{
			public Context()
			{
				DataLayerMock = new ContractADataLayerMock();
				Contract = new ContractA(DataLayerMock);
			}

			public IContractA Contract { get; }
			public ContractADataLayerMock DataLayerMock { get; }
		}

		[TestMethod]
		public async Task Create_Ok()
		{
			var context = new Context();

			ModelA model = CreateDefaultModel();
			await context.Contract.CreateAsync(model, CancellationToken.None);
		}

		[TestMethod]
		public async Task Create_Fail_ModelIsNull()
		{
			var context = new Context();

			async Task action()
			{
				var model = new ModelA
				{
					Id = Guid.NewGuid(),
					Time = DateTime.UtcNow,
					Data = null,
				};
				await context.Contract.CreateAsync(model, CancellationToken.None);
			};

			await Assert.ThrowsExceptionAsync<ArgumentException>(action);
		}

		[TestMethod]
		public async Task Create_Fail_DataIsNull()
		{
			var context = new Context();

			async Task action() => await context.Contract.CreateAsync(null, CancellationToken.None);

			await Assert.ThrowsExceptionAsync<ArgumentNullException>(action);
		}

		[TestMethod]
		public async Task Create_Fail_AlreadyExists()
		{
			var context = new Context();

			ModelA model = CreateDefaultModel();
			await context.Contract.CreateAsync(model, CancellationToken.None);

			async Task action() => await context.Contract.CreateAsync(model, CancellationToken.None);

			await Assert.ThrowsExceptionAsync<ContractAIdAlreadyExistsException>(action);
		}

		[TestMethod]
		public async Task Create_Fail_Canceled()
		{
			using var cts = new CancellationTokenSource();
			var context = new Context();
			context.DataLayerMock.BeforeCreateAsync += (mock, model, ct) =>
			{
				cts.Cancel();
				ct.ThrowIfCancellationRequested();
			};

			async Task action()
			{
				ModelA model = CreateDefaultModel();
				await context.Contract.CreateAsync(model, cts.Token);
			}

			await Assert.ThrowsExceptionAsync<OperationCanceledException>(action);
		}

		[TestMethod]
		public async Task Create_Fail_Timeout()
		{
			var context = new Context();
			context.DataLayerMock.BeforeCreateAsync += (mock, id, ct) => throw new TimeoutException();

			async Task action()
			{
				ModelA model = CreateDefaultModel();
				await context.Contract.CreateAsync(model, CancellationToken.None);
			}

			await Assert.ThrowsExceptionAsync<TimeoutException>(action);
		}

		[TestMethod]
		public async Task Get_Ok()
		{
			var context = new Context();

			ModelA model1 = CreateDefaultModel();
			await context.Contract.CreateAsync(model1, CancellationToken.None);

			ModelA model2 = await context.Contract.GetAsync(model1.Id, CancellationToken.None);

			Assert.AreEqual(model1, model2);
		}

		[TestMethod]
		public async Task Get_Ok_NotFound()
		{
			var context = new Context();

			ModelA model = await context.Contract.GetAsync(Guid.NewGuid(), CancellationToken.None);
			Assert.IsNull(model);
		}

		[TestMethod]
		public async Task Get_Fail_Canceled()
		{
			using var cts = new CancellationTokenSource();
			var context = new Context();
			context.DataLayerMock.BeforeGetAsync += (mock, id, ct) =>
			{
				cts.Cancel();
				ct.ThrowIfCancellationRequested();
			};

			async Task action() => await context.Contract.GetAsync(Guid.NewGuid(), cts.Token);

			await Assert.ThrowsExceptionAsync<OperationCanceledException>(action);
		}

		[TestMethod]
		public async Task Get_Fail_Timeout()
		{
			var context = new Context();
			context.DataLayerMock.BeforeGetAsync += (mock, id, ct) => throw new TimeoutException();

			async Task action() => await context.Contract.GetAsync(Guid.NewGuid(), CancellationToken.None);

			await Assert.ThrowsExceptionAsync<TimeoutException>(action);
		}

		[TestMethod]
		public async Task Update_Ok()
		{
			var context = new Context();

			ModelA model1 = CreateDefaultModel();
			await context.Contract.CreateAsync(model1, CancellationToken.None);
			model1.Time = model1.Time.AddSeconds(1);
			model1.Data += ".Updated";

			int count = await context.Contract.UpdateAsync(model1, CancellationToken.None);

			Assert.AreEqual(1, count);
			ModelA model2 = await context.Contract.GetAsync(model1.Id, CancellationToken.None);
			Assert.AreEqual(model1, model2);
		}

		[TestMethod]
		public async Task Update_Ok_NotFound()
		{
			var context = new Context();

			var model = new ModelA
			{
				Id = Guid.NewGuid(),
				Time = DateTime.UtcNow,
				Data = "ModelA.Data",
			};
			int count = await context.Contract.UpdateAsync(model, CancellationToken.None);
			Assert.AreEqual(0, count);
		}

		[TestMethod]
		public async Task Update_Fail_ModelIsNull()
		{
			var context = new Context();

			async Task action()
			{
				var model = new ModelA
				{
					Id = Guid.NewGuid(),
					Time = DateTime.UtcNow,
					Data = null,
				};
				await context.Contract.UpdateAsync(model, CancellationToken.None);
			};

			await Assert.ThrowsExceptionAsync<ArgumentException>(action);
		}

		[TestMethod]
		public async Task Update_Fail_DataIsNull()
		{
			var context = new Context();

			async Task action() => await context.Contract.UpdateAsync(null, CancellationToken.None);

			await Assert.ThrowsExceptionAsync<ArgumentNullException>(action);
		}

		[TestMethod]
		public async Task Update_Fail_Canceled()
		{
			using var cts = new CancellationTokenSource();
			var context = new Context();
			context.DataLayerMock.BeforeUpdateAsync += (mock, model, ct) =>
			{
				cts.Cancel();
				ct.ThrowIfCancellationRequested();
			};

			async Task action()
			{
				ModelA model = CreateDefaultModel();
				await context.Contract.UpdateAsync(model, cts.Token);
			}

			await Assert.ThrowsExceptionAsync<OperationCanceledException>(action);
		}

		[TestMethod]
		public async Task Update_Fail_Timeout()
		{
			var context = new Context();
			context.DataLayerMock.BeforeUpdateAsync += (mock, model, ct) => throw new TimeoutException();

			async Task action()
			{
				ModelA model = CreateDefaultModel();
				await context.Contract.UpdateAsync(model, CancellationToken.None);
			}

			await Assert.ThrowsExceptionAsync<TimeoutException>(action);
		}

		[TestMethod]
		public async Task Delete_Ok()
		{
			var context = new Context();

			ModelA model = CreateDefaultModel();
			await context.Contract.CreateAsync(model, CancellationToken.None);

			int count = await context.Contract.DeleteAsync(model.Id, CancellationToken.None);

			Assert.AreEqual(1, count);
			model = await context.Contract.GetAsync(model.Id, CancellationToken.None);
			Assert.IsNull(model);
		}

		[TestMethod]
		public async Task Delete_Ok_NotFound()
		{
			var context = new Context();

			int count = await context.Contract.DeleteAsync(Guid.NewGuid(), CancellationToken.None);
			Assert.AreEqual(0, count);
		}

		[TestMethod]
		public async Task Delete_Fail_Canceled()
		{
			using var cts = new CancellationTokenSource();
			var context = new Context();
			context.DataLayerMock.BeforeDeleteAsync += (mock, id, ct) =>
			{
				cts.Cancel();
				ct.ThrowIfCancellationRequested();
			};

			async Task action() => await context.Contract.DeleteAsync(Guid.NewGuid(), cts.Token);

			await Assert.ThrowsExceptionAsync<OperationCanceledException>(action);
		}

		[TestMethod]
		public async Task Delete_Fail_Timeout()
		{
			var context = new Context();
			context.DataLayerMock.BeforeDeleteAsync += (mock, id, ct) => throw new TimeoutException();

			async Task action() => await context.Contract.DeleteAsync(Guid.NewGuid(), CancellationToken.None);

			await Assert.ThrowsExceptionAsync<TimeoutException>(action);
		}

		private static ModelA CreateDefaultModel()
		{
			return new ModelA
			{
				Id = Guid.NewGuid(),
				Time = DateTime.UtcNow,
				Data = "ModelA.Default"
			};
		}
	}
}