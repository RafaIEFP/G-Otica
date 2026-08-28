using FluentMigrator;
using GOtica.Infrastructure.Migrations.Services;

namespace GOtica.Infrastructure.Migrations.Versions;

[Migration(DatabaseVersions.TABLE_SUPPLIER, "Create suppliers table")]
public class Version00007 : ForwardOnlyMigration
{
    public override void Up()
    {
        Create.Table("Suppliers")
            .WithColumn("Id").AsGuid().PrimaryKey().NotNullable()
            .WithColumn("Name").AsString(255).NotNullable()
            .WithColumn("PhoneNumber").AsString(20).Nullable()
            .WithColumn("Email").AsString(255).Nullable()
            .WithColumn("IsActive").AsBoolean().NotNullable().WithDefaultValue(true)

            .WithColumn("OpticalStoreId").AsGuid().NotNullable()
                .ForeignKey(
                    "FK_Suppliers_OpticalStores_OpticalStoreId",
                    "OpticalStores",
                    "Id");
    }
}
