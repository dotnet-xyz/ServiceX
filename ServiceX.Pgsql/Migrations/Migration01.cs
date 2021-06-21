// Dotnet-XYZ © 2021

using FluentMigrator;

namespace DotnetXYZ.ServiceX.Pgsql.Migrations
{
	[Migration(1)]
	public class Migration01 : Migration
	{
		public override void Up()
		{
			Create.Table("ModelA")
				.WithColumn("Id").AsGuid().PrimaryKey()
				// Store Time as Int64 to keep .NET DateTime resolution.
				.WithColumn("Time").AsInt64()
				.WithColumn("Data").AsString();
		}

		public override void Down()
		{
			Delete.Table("ModelA");
		}
	}
}