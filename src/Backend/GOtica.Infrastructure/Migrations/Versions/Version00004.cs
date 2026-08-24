using FluentMigrator;
using GOtica.Infrastructure.Migrations.Services;

namespace GOtica.Infrastructure.Migrations.Versions;

[Migration(DatabaseVersions.TABLE_CLIENT, "Create table client")]
public class Version00004 : ForwardOnlyMigration
{
    public override void Up()
    {
        Create.Table("Clients")
            .WithColumn("Id").AsGuid().PrimaryKey().NotNullable()
            .WithColumn("Name").AsString(255).NotNullable()
            .WithColumn("PhoneNumber").AsString(20).NotNullable()
            .WithColumn("Email").AsString(255).Nullable()
            .WithColumn("DateOfBirth").AsDate().Nullable()
            .WithColumn("IsActive").AsBoolean().NotNullable().WithDefaultValue(true)
            .WithColumn("OpticalStoreId").AsGuid().NotNullable()
                .ForeignKey(
                    "FK_Clients_OpticalStores_OpticalStoreId",
                    "OpticalStores",
                    "Id");
    }
}
