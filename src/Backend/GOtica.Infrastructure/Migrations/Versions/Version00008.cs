using FluentMigrator;
using GOtica.Infrastructure.Migrations.Services;

namespace GOtica.Infrastructure.Migrations.Versions;

[Migration(DatabaseVersions.TABLE_PURCHASE_AND_PURCHASE_ITEMS, "Create purchases and purchase items tables")]
public class Version00008 : ForwardOnlyMigration
{
    public override void Up()
    {
        Create.Table("Purchases")
            .WithColumn("Id").AsGuid().PrimaryKey().NotNullable()
            .WithColumn("CreatedAt").AsDateTime().NotNullable()
            .WithColumn("TotalAmount").AsDecimal(10, 2).NotNullable()

            .WithColumn("SupplierId").AsGuid().NotNullable()
                .ForeignKey(
                    "FK_Purchases_Suppliers_SupplierId",
                    "Suppliers",
                    "Id")

            .WithColumn("UserId").AsGuid().NotNullable()
                .ForeignKey(
                    "FK_Purchases_Users_UserId",
                    "Users",
                    "Id")

            .WithColumn("OpticalStoreId").AsGuid().NotNullable()
                .ForeignKey(
                    "FK_Purchases_OpticalStores_OpticalStoreId",
                    "OpticalStores",
                    "Id");

        Create.Table("PurchaseItems")
            .WithColumn("Id").AsGuid().PrimaryKey().NotNullable()
            .WithColumn("Quantity").AsInt32().NotNullable()
            .WithColumn("UnitPrice").AsDecimal(10, 2).NotNullable()
            .WithColumn("TotalAmount").AsDecimal(10, 2).NotNullable()

            .WithColumn("PurchaseId").AsGuid().NotNullable()
                .ForeignKey(
                    "FK_PurchaseItems_Purchases_PurchaseId",
                    "Purchases",
                    "Id")

            .WithColumn("ProductId").AsGuid().NotNullable()
                .ForeignKey(
                    "FK_PurchaseItems_Products_ProductId",
                    "Products",
                    "Id");
    }
}
