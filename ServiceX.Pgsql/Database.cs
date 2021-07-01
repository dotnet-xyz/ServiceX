// Dotnet-XYZ © 2021

using FluentMigrator.Runner;
using Microsoft.Extensions.Options;
using Npgsql;
using NpgsqlTypes;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace DotnetXYZ.ServiceX.Pgsql
{
	public class Database
	{
		public class Options
		{
			public string Host { get; set; }
			public string Name { get; set; }
			public string User { get; set; }
			public string Password { get; set; }

			public string BuildConnectionString()
			{
				var builder = new NpgsqlConnectionStringBuilder
				{
					Host = Host ?? throw new ArgumentNullException(nameof(Host)),
					Database = Name ?? throw new ArgumentNullException(nameof(Name)),
					Username = User,
					Password = Password
				};
				return builder.ToString();
			}

			public string BuildConnectionStringNoDB()
			{
				var builder = new NpgsqlConnectionStringBuilder
				{
					Host = Host ?? throw new ArgumentNullException(nameof(Host)),
					Database = null,
					Username = User,
					Password = Password
				};
				return builder.ToString();
			}
		}

		private readonly Options _Options;
		private readonly string _ConnectionString;
		private readonly string _ConnectionStringNoDB;
		private readonly IMigrationRunner _MigrationRunner;

		public Database(Options options, IMigrationRunner mr)
		{
			_Options = options ?? throw new ArgumentNullException(nameof(options));
			_ConnectionString = _Options.BuildConnectionString();
			_ConnectionStringNoDB = _Options.BuildConnectionStringNoDB();
			_MigrationRunner = mr ?? throw new ArgumentNullException(nameof(mr));
		}

		public Database(IOptions<Options> options, IMigrationRunner mr) :
			this(options?.Value, mr)
		{
		}

		public string Name => _Options.Name;

		public async Task<NpgsqlConnection> ConnectAsync(CancellationToken ct)
		{
			var connection = new NpgsqlConnection(_ConnectionString);
			await connection.OpenAsync(ct);
			return connection;
		}

		private NpgsqlConnection ConnectNoDB()
		{
			var connection = new NpgsqlConnection(_ConnectionStringNoDB);
			connection.Open();
			return connection;
		}

		private bool IsExists()
		{
			using NpgsqlConnection connection = ConnectNoDB();
			using var command = new NpgsqlCommand(
				@"
				SELECT
					""datname""
				FROM
					""pg_database""
				WHERE
					""datname"" = @database_name
				;",
				connection);
			command.Parameters.AddWithValue("@database_name", NpgsqlDbType.Name, Name);
			using NpgsqlDataReader reader = command.ExecuteReader();
			return reader.Read();
		}

		private void Create()
		{
			using NpgsqlConnection connection = ConnectNoDB();
			using var command = new NpgsqlCommand(
				$@"
				CREATE DATABASE ""{Name}""
					WITH OWNER = postgres
					ENCODING = 'UTF8'
					TABLESPACE = pg_default
					CONNECTION LIMIT = -1
				;",
				connection);
			command.ExecuteNonQuery();
		}

		internal void Deploy()
		{
			if (!IsExists())
			{
				Create();
			}
			_MigrationRunner.MigrateUp();
		}
	}
}