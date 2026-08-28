using FluentMigrator;
using GOtica.Infrastructure.Migrations.Services;

namespace GOtica.Infrastructure.Migrations.Versions;

[Migration(DatabaseVersions.TABLE_STOCK_MOVEMENT, "Create table stock movement")]
public class Version00006 : ForwardOnlyMigration
{
    public override void Up()
    {
        Create.Table("StockMovements")
        .WithColumn("Id").AsGuid().PrimaryKey().NotNullable()
        .WithColumn("QuantityChange").AsInt32().NotNullable()
        .WithColumn("Type").AsInt32().NotNullable()
        .WithColumn("Reason").AsString(500).Nullable()
        .WithColumn("CreatedAt").AsUtcDateTime().NotNullable().WithDefault(SystemMethods.CurrentUTCDateTime)

        .WithColumn("ProductId").AsGuid().NotNullable().ForeignKey(
                "FK_StockMovements_Products_ProductId",
                "Products",
                "Id")

        .WithColumn("UserId").AsGuid().NotNullable().ForeignKey(
                "FK_StockMovements_Users_UserId",
                "Users",
                "Id");

        Create.Index("IX_StockMovements_ProductId_CreatedAt")
            .OnTable("StockMovements")
            .OnColumn("ProductId").Ascending()
            .OnColumn("CreatedAt").Descending();
    }
}
