// Dotnet-XYZ © 2021

using DotnetXYZ.ServiceX;
using DotnetXYZ.ServiceX.Pgsql;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace ServiceX.IntegrationTest
{
	[TestClass]
	public class ContractAPgsqlTest : ContractATestBase
	{
		private static IServiceProvider _ServiceProvider;

		protected override IServiceProvider ServiceProvider => _ServiceProvider;

		[ClassInitialize]
		public static void Init(TestContext context)
		{
			IServiceCollection services = new ServiceCollection();
			services.AddServiceX();
			services.AddServiceXDataLayerPgsql(new Database.Options
			{
				Host = "localhost",
				Name = "ServiceX.IntegrationTest",
				User = "postgres",
				Password = "P@ssw0rd"
			});
			_ServiceProvider = services.BuildServiceProvider();
		}

		[ClassCleanup]
		public static void Cleanup()
		{
			_ServiceProvider = null;
		}
	}
}