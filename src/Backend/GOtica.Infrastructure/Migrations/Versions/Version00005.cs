using FluentMigrator;
using GOtica.Infrastructure.Migrations.Services;

namespace GOtica.Infrastructure.Migrations.Versions;

[Migration(DatabaseVersions.TABLE_PRODUCT, "Create table product")]
public class Version00005 : ForwardOnlyMigration
{
    public override void Up()
    {
        Create.Table("Products")
            .WithColumn("Id").AsGuid().PrimaryKey().NotNullable()
            .WithColumn("Name").AsString(255).NotNullable()
            .WithColumn("ProductType").AsInt32().NotNullable()
            .WithColumn("ProductCode").AsString(100).NotNullable()
            .WithColumn("BasePrice").AsDecimal(18, 2).NotNullable()
            .WithColumn("StockQuantity").AsInt32().NotNullable()
            .WithColumn("IsActive").AsBoolean().NotNullable().WithDefaultValue(true)
            .WithColumn("OpticalStoreId").AsGuid().NotNullable()
                .ForeignKey(
                    "FK_Products_OpticalStores_OpticalStoreId",
                    "OpticalStores",
                    "Id");

        Create.UniqueConstraint("UQ_Products_OpticalStoreId_ProductCode")
            .OnTable("Products")
            .Columns("OpticalStoreId", "ProductCode");
    }
}
