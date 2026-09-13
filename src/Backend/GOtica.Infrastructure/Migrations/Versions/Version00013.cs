using FluentMigrator;
using GOtica.Infrastructure.Migrations.Services;

namespace GOtica.Infrastructure.Migrations.Versions;

[Migration(DatabaseVersions.TABLE_TREATMENT, "Create treatments table")]
public class Version00013 : ForwardOnlyMigration
{
    public override void Up()
    {
        Create.Table("Treatments")
            .WithColumn("Id").AsGuid().PrimaryKey().NotNullable()
            .WithColumn("Name").AsString(255).NotNullable()
            .WithColumn("BasePrice").AsDecimal(10, 2).NotNullable()
            .WithColumn("IsActive").AsBoolean().NotNullable().WithDefaultValue(true)

            .WithColumn("OpticalStoreId").AsGuid().NotNullable()
                .ForeignKey(
                    "FK_Treatments_OpticalStores_OpticalStoreId",
                    "OpticalStores",
                    "Id");

        Create.Index("UX_Treatments_OpticalStoreId_Name")
            .OnTable("Treatments")
            .OnColumn("OpticalStoreId").Ascending()
            .OnColumn("Name").Ascending()
            .WithOptions().Unique();
    }
}
