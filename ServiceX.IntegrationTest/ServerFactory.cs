// Dotnet-XYZ © 2021

using DotnetXYZ.ServiceX.Mock;
using DotnetXYZ.ServiceX.Server;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;

namespace DotnetXYZ.ServiceX.IntegrationTest
{
	public class ServerFactory : WebApplicationFactory<Startup>
	{
		protected override void ConfigureWebHost(IWebHostBuilder builder)
		{
			builder.ConfigureServices(sc =>
			{
				sc.AddServiceXDataLayerMock();
			});
		}
	}
}