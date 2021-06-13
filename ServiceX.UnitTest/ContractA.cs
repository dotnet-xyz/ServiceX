// Dotnet-XYZ © 2021

using DotnetXYZ.ServiceX;
using DotnetXYZ.ServiceX.Api;
using DotnetXYZ.ServiceX.Mock;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace DotnetXYZ.ServiceX.UnitTest
{
	[TestClass]
	public class ContractAUnitTest
	{
		private static IServiceProvider _ServiceProvider;

		private static IContractA ContractA => _ServiceProvider.GetService<IContractA>();

		[ClassInitialize]
		public static void Init(TestContext context)
		{
			IServiceCollection services = new ServiceCollection();
			services.AddServiceX();
			services.AddServiceXDataLayerMock();
			_ServiceProvider = services.BuildServiceProvider();
		}

		[ClassCleanup]
		public static void Cleanup()
		{
			_ServiceProvider = null;
		}

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
			model1.Data += ".Updated";
			await ContractA.UpdateAsync(model1, CancellationToken.None);
			ModelA model2 = await ContractA.GetAsync(model1.Id, CancellationToken.None);
			Assert.AreEqual(model1, model2);
		}

		[TestMethod]
		public async Task Update_Fail_NotFound()
		{
			var model = new ModelA
			{
				Id = Guid.NewGuid(),
				Time = DateTime.UtcNow,
				Data = "ModelA.Data",
			};
			await Assert.ThrowsExceptionAsync<ContractAIdNotFoundException>(
				async () => await ContractA.UpdateAsync(model, CancellationToken.None));
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
			await ContractA.DeleteAsync(model.Id, CancellationToken.None);
			model = await ContractA.GetAsync(model.Id, CancellationToken.None);
			Assert.IsNull(model);
		}

		[TestMethod]
		public async Task Delete_Fail_NotFound()
		{
			await Assert.ThrowsExceptionAsync<ContractAIdNotFoundException>(
				async () => await ContractA.DeleteAsync(Guid.NewGuid(), CancellationToken.None));
		}
	}
}