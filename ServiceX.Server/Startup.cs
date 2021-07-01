// Dotnet-XYZ © 2021

using DotnetXYZ.ServiceX.Pgsql;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DotnetXYZ.ServiceX.Server
{
	public class Startup
	{
		private readonly IConfigurationRoot _ConfigurationRoot;

		public Startup(IWebHostEnvironment env)
		{
			_ConfigurationRoot = BuildConfigurationRoot(env);
		}

		private static IConfigurationRoot BuildConfigurationRoot(IWebHostEnvironment env)
		{
			var builder = new ConfigurationBuilder();
			builder.SetBasePath(env.ContentRootPath);
			builder.AddJsonFile(
				"DotnetXYZ.ServiceX.Server.json",
				optional: false,
				reloadOnChange: false);
			builder.AddJsonFile(
				$"DotnetXYZ.ServiceX.Server.{env.EnvironmentName}.json",
				optional: true,
				reloadOnChange: false);
			builder.AddEnvironmentVariables();
			return builder.Build();
		}

		public void ConfigureServices(IServiceCollection sc)
		{
			sc.AddOptions();

			sc.AddControllers();

			sc.AddServiceX();
			sc.AddServiceXDataLayerPgsql(
				_ConfigurationRoot.GetSection("DotnetXYZ.ServiceX.Pgsql:Database"));
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