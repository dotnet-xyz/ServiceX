// Dotnet-XYZ © 2021

using DotnetXYZ.ServiceX.Api;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace DotnetXYZ.ServiceX.IntegrationTest
{
	public abstract class ContractATestBase
	{
		protected abstract IServiceProvider ServiceProvider { get; }

		private IContractA ContractA => ServiceProvider.GetService<IContractA>();

		[TestMethod]
		public async Task Create_Ok()
		{
			var model = new ModelA
			{
				Id = Guid.NewGuid(),
				Time = DateTime.UtcNow,
				Data = "ModelA.Data",
			};
			await ContractA.CreateAsync(model, CancellationToken.None);
		}

		[TestMethod]
		public async Task Create_Fail_AlreadyExists()
		{
			var model = new ModelA
			{
				Id = Guid.NewGuid(),
				Time = DateTime.UtcNow,
				Data = "ModelA.Data",
			};
			await ContractA.CreateAsync(model, CancellationToken.None);
			await Assert.ThrowsExceptionAsync<ContractAIdAlreadyExistsException>(
				async () => await ContractA.CreateAsync(model, CancellationToken.None));
		}

		[TestMethod]
		public async Task Get_Ok()
		{
			var model1 = new ModelA
			{
				Id = Guid.NewGuid(),
				Time = DateTime.UtcNow,
				Data = "ModelA.Data",
			};
			await ContractA.CreateAsync(model1, CancellationToken.None);
			ModelA model2 = await ContractA.GetAsync(model1.Id, CancellationToken.None);
			Assert.AreEqual(model1, model2);
		}

		[TestMethod]
		public async Task Get_Ok_NotFound()
		{
			ModelA model = await ContractA.GetAsync(Guid.NewGuid(), CancellationToken.None);
			Assert.IsNull(model);
		}

		[TestMethod]
		public async Task Update_Ok()
		{
			var model1 = new ModelA
			{
				Id = Guid.NewGuid(),
				Time = DateTime.UtcNow,
				Data = "ModelA.Data",
			};
			await ContractA.CreateAsync(model1, CancellationToken.None);
			model1.Time = model1.Time.AddSeconds(1);
			model1.Data += ".Updated";
			int count = await ContractA.UpdateAsync(model1, CancellationToken.None);
			Assert.AreEqual(1, count);
			ModelA model2 = await ContractA.GetAsync(model1.Id, CancellationToken.None);
			Assert.AreEqual(model1, model2);
		}

		[TestMethod]
		public async Task Update_Ok_NotFound()
		{
			var model = new ModelA
			{
				Id = Guid.NewGuid(),
				Time = DateTime.UtcNow,
				Data = "ModelA.Data",
			};
			int count = await ContractA.UpdateAsync(model, CancellationToken.None);
			Assert.AreEqual(0, count);
		}

		[TestMethod]
		public async Task Delete_Ok()
		{
			var model = new ModelA
			{
				Id = Guid.NewGuid(),
				Time = DateTime.UtcNow,
				Data = "ModelA.Data",
			};
			await ContractA.CreateAsync(model, CancellationToken.None);
			int count = await ContractA.DeleteAsync(model.Id, CancellationToken.None);
			Assert.AreEqual(1, count);
			model = await ContractA.GetAsync(model.Id, CancellationToken.None);
			Assert.IsNull(model);
		}

		[TestMethod]
		public async Task Delete_Ok_NotFound()
		{
			int count = await ContractA.DeleteAsync(Guid.NewGuid(), CancellationToken.None);
			Assert.AreEqual(0, count);
		}
	}
}