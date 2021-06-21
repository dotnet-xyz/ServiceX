// Dotnet-XYZ © 2021

using System;
using System.Data;

namespace DotnetXYZ.ServiceX.Pgsql
{
	public static class DataRecordExt
	{
		public static DateTime GetDateTimeUtc(this IDataRecord record, int ordinal)
		{
			long ticks = record.GetInt64(ordinal);
			return new DateTime(ticks, DateTimeKind.Utc);
		}
	}
}