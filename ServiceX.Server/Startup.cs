// Dotnet-XYZ © 2021

using DotnetXYZ.ServiceX.Pgsql;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DotnetXYZ.ServiceX.Server
{
	public class Startup
	{
		public Startup(IConfiguration configuration)
		{
			Configuration = configuration;
		}

		public IConfiguration Configuration { get; }

		public void ConfigureServices(IServiceCollection sc)
		{
			sc.AddControllers();

			sc.AddServiceX();
			sc.AddServiceXDataLayerPgsql(new Database.Options
			{
				Host = "localhost",
				Name = "ServiceX.IntegrationTest",
				User = "postgres",
				Password = "P@ssw0rd"
			});
		}

		public void Configure(IApplicationBuilder builder)
		{
			builder.UseRouting();

			builder.UseEndpoints(endpoints =>
			{
				endpoints.MapControllers();
			});
		}
	}
}