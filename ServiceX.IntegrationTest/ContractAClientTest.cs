// Dotnet-XYZ © 2021

using DotnetXYZ.ServiceX.Api;
using DotnetXYZ.ServiceX.Client;
using DotnetXYZ.ServiceX.Server;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Net.Http;

namespace ServiceX.IntegrationTest
{
	[TestClass]
	public class ContractAClientTest : ContractATestBase
	{
		private static IServiceProvider _ServiceProvider;
		private static WebApplicationFactory<Startup> _ServerFactory;

		protected override IServiceProvider ServiceProvider => _ServiceProvider;

		[ClassInitialize]
		public static void Init(TestContext context)
		{
			_ServerFactory = new WebApplicationFactory<Startup>();
			IServiceCollection services = new ServiceCollection();
			services.AddSingleton<IContractA>(sp =>
			{
				HttpClient client = _ServerFactory.CreateClient();
				return new ContractAClient(client);
			});
			_ServiceProvider = services.BuildServiceProvider();
		}

		[ClassCleanup]
		public static void Cleanup()
		{
			_ServiceProvider = null;
			_ServerFactory = null;
		}
	}
}