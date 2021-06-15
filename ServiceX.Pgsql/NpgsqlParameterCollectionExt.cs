// Dotnet-XYZ © 2021

using Npgsql;
using NpgsqlTypes;
using System;

namespace DotnetXYZ.ServiceX.Pgsql
{
	public static class NpgsqlParameterCollectionExt
	{
		public static NpgsqlParameter Add(this NpgsqlParameterCollection collection, string name, DateTime value)
		{
			if (value.Kind != DateTimeKind.Utc)
			{
				throw new ArgumentException("DateTime must have UTC kind.", nameof(value));
			}
			return collection.AddWithValue(name, NpgsqlDbType.Timestamp, value);
		}

		public static NpgsqlParameter Add(this NpgsqlParameterCollection collection, string name, Guid value)
		{
			return collection.AddWithValue(name, NpgsqlDbType.Uuid, value);
		}

		public static NpgsqlParameter Add(this NpgsqlParameterCollection collection, string name, string value)
		{
			return collection.AddWithValue(name, NpgsqlDbType.Varchar, value);
		}
	}
}