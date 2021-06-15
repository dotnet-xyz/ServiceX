// Dotnet-XYZ © 2021

using System;
using System.Data;

namespace DotnetXYZ.ServiceX.Pgsql
{
	public static class DataRecordExt
	{
		public static DateTime GetDateTimeUtc(this IDataRecord record, int ordinal)
		{
			DateTime time = record.GetDateTime(ordinal);
			return new DateTime(time.Ticks, DateTimeKind.Utc);
		}
	}
}