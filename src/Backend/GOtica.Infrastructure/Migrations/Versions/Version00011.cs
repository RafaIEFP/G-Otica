using FluentMigrator;
using GOtica.Infrastructure.Migrations.Services;

namespace GOtica.Infrastructure.Migrations.Versions;

[Migration(DatabaseVersions.TABLE_SALE_ITEM, "Create sale items table")]
public class Version00011 : ForwardOnlyMigration
{
    public override void Up()
    {
        Create.Table("SaleItems")
            .WithColumn("Id").AsGuid().PrimaryKey().NotNullable()
            .WithColumn("Quantity").AsInt32().NotNullable()
            .WithColumn("UnitPrice").AsDecimal(10, 2).NotNullable()
            .WithColumn("DiscountAmount").AsDecimal(10, 2).NotNullable().WithDefaultValue(0)
            .WithColumn("TotalAmount").AsDecimal(10, 2).NotNullable()
            .WithColumn("Notes").AsString(500).Nullable()

            .WithColumn("SaleId").AsGuid().NotNullable()
                .ForeignKey(
                    "FK_SaleItems_Sales_SaleId",
                    "Sales",
                    "Id")

            .WithColumn("ProductId").AsGuid().NotNullable()
                .ForeignKey(
                    "FK_SaleItems_Products_ProductId",
                    "Products",
                    "Id");

        Create.Index("IX_SaleItems_SaleId")
            .OnTable("SaleItems")
            .OnColumn("SaleId").Ascending();
    }
}
