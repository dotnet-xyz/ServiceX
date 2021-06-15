// Dotnet-XYZ © 2021

using DotnetXYZ.ServiceX.Api;
using FluentMigrator.Runner;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace DotnetXYZ.ServiceX.Pgsql
{
	public static class ServiceCollectionExt
	{
		public static void AddServiceXDataLayerPgsql(this IServiceCollection sc, Database.Options options)
		{
			if (options == null)
			{
				throw new ArgumentNullException(nameof(options));
			}

			sc.AddFluentMigratorCore();
			sc.ConfigureRunner(builder =>
			{
				builder.AddPostgres();
				builder.WithGlobalConnectionString(options.BuildConnectionString());
				builder.ScanIn(typeof(Migrations.Migration01).Assembly).For.Migrations();
			});
			sc.AddSingleton(sp =>
			{
				var database = new Database(options, sp.GetService<IMigrationRunner>());
				database.Deploy();
				return database;
			});
			sc.AddSingleton<IContractADataLayer, ContractADataLayer>();
		}
	}
}