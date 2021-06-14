// Dotnet-XYZ © 2021

using DotnetXYZ.ServiceX.Api;
using Npgsql;
using NpgsqlTypes;
using System;
using System.Data;
using System.Data.Common;
using System.Threading;
using System.Threading.Tasks;

namespace DotnetXYZ.ServiceX.Pgsql
{
	public class ContractADataLayer : IContractADataLayer
	{
		private readonly Database _Database;

		public ContractADataLayer(Database database)
		{
			_Database = database ?? throw new ArgumentNullException(nameof(database));
		}

		private static ModelA From(IDataRecord record)
		{
			return new ModelA
			{
				Id = record.GetGuid(0),
				Time = new DateTime(record.GetDateTime(1).Ticks, DateTimeKind.Utc),
				Data = record.GetString(2)
			};
		}

		public async Task CreateAsync(ModelA model, CancellationToken ct)
		{
			if (model == null)
			{
				throw new ArgumentNullException(nameof(model));
			}

			using NpgsqlConnection connection = await _Database.ConnectAsync(ct);
			using var command = new NpgsqlCommand(
				@"INSERT INTO ""ModelA""
				(
					""Id"", ""Time"", ""Data""
				)
				VALUES
				(
					@Id, @Time, @Data
				);",
				connection);
			command.Parameters.AddWithValue("Id", NpgsqlDbType.Uuid, model.Id);
			command.Parameters.AddWithValue("Time", NpgsqlDbType.Timestamp, model.Time);
			command.Parameters.AddWithValue("Data", NpgsqlDbType.Varchar, model.Data);
			await command.PrepareAsync(ct);

			try
			{
				await command.ExecuteNonQueryAsync(ct);
			}
			catch (PostgresException e) when (e.SqlState == "23505") // unique_violation
			{
				throw new ContractAIdAlreadyExistsException(model.Id, e);
			}
		}

		public async Task<ModelA> GetAsync(Guid id, CancellationToken ct)
		{
			using NpgsqlConnection connection = await _Database.ConnectAsync(ct);
			using var command = new NpgsqlCommand(
				@"SELECT
					""Id"", ""Time"", ""Data""
				FROM
					""ModelA""
				WHERE
					""Id"" = @Id;",
				connection);
			command.Parameters.AddWithValue("Id", NpgsqlDbType.Uuid, id);
			await command.PrepareAsync(ct);

			using DbDataReader reader = await command.ExecuteReaderAsync(ct);
			if (!(await reader.ReadAsync(ct)))
			{
				return null;
			}
			return From(reader);
		}

		public async Task UpdateAsync(ModelA model, CancellationToken ct)
		{
			using NpgsqlConnection connection = await _Database.ConnectAsync(ct);
			using var command = new NpgsqlCommand(
				@"UPDATE ""ModelA"" SET
					""Time"" = @Time,
					""Data"" = @Data
				WHERE
					""Id"" = @Id;",
				connection);
			command.Parameters.AddWithValue("Id", NpgsqlDbType.Uuid, model.Id);
			command.Parameters.AddWithValue("Time", NpgsqlDbType.Timestamp, model.Time);
			command.Parameters.AddWithValue("Data", NpgsqlDbType.Varchar, model.Data);
			await command.PrepareAsync(ct);

			int count = await command.ExecuteNonQueryAsync(ct);
			if (count == 0)
			{
				throw new ContractAIdNotFoundException(model.Id, null);
			}
		}

		public async Task DeleteAsync(Guid id, CancellationToken ct)
		{
			using NpgsqlConnection connection = await _Database.ConnectAsync(ct);
			using var command = new NpgsqlCommand(
				@"DELETE FROM ""ModelA""
				WHERE
					""Id"" = @Id;",
				connection);
			command.Parameters.AddWithValue("Id", NpgsqlDbType.Uuid, id);
			await command.PrepareAsync(ct);

			int count = await command.ExecuteNonQueryAsync(ct);
			if (count == 0)
			{
				throw new ContractAIdNotFoundException(id, null);
			}
		}
	}
}