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
				.WithColumn("Time").AsDateTime()
				.WithColumn("Data").AsString();
		}

		public override void Down()
		{
			Delete.Table("ModelA");
		}
	}
}