// Dotnet-XYZ © 2021

using DotnetXYZ.ServiceX.Api;
using FluentMigrator.Runner;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;

namespace DotnetXYZ.ServiceX.Pgsql
{
	public static class ServiceCollectionExt
	{
		public static void AddServiceXDataLayerPgsql(this IServiceCollection sc)
		{
			sc.AddFluentMigratorCore();
			sc.ConfigureRunner(builder =>
			{
				builder.AddPostgres();
				builder.WithGlobalConnectionString(sp =>
				{
					IOptions<Database.Options> options = sp.GetService<IOptions<Database.Options>>();
					return options?.Value.BuildConnectionString();
				});
				builder.ScanIn(typeof(Migrations.Migration01).Assembly).For.Migrations();
			});
			sc.AddSingleton(sp =>
			{
				IOptions<Database.Options> options = sp.GetService<IOptions<Database.Options>>();

				using IServiceScope scope = sp.CreateScope();
				var migrator = scope.ServiceProvider.GetService<IMigrationRunner>();

				var database = new Database(options, migrator);
				database.Deploy();
				return database;
			});
			sc.AddSingleton<IContractADataLayer, ContractADataLayer>();
		}

		public static void AddServiceXDataLayerPgsql(this IServiceCollection sc, Action<Database.Options> options)
		{
			sc.Configure(options);
			sc.AddServiceXDataLayerPgsql();
		}

		public static void AddServiceXDataLayerPgsql(this IServiceCollection sc, IConfiguration configuration)
		{
			sc.Configure<Database.Options>(configuration);
			sc.AddServiceXDataLayerPgsql();
		}
	}
}