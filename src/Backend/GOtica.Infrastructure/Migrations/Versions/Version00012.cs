using FluentMigrator;
using GOtica.Infrastructure.Migrations.Services;

namespace GOtica.Infrastructure.Migrations.Versions;

[Migration(DatabaseVersions.TABLE_PAYMENT, "Create payments table")]
public class Version00012 : ForwardOnlyMigration
{
    public override void Up()
    {
        Create.Table("Payments")
            .WithColumn("Id").AsGuid().PrimaryKey().NotNullable()
            .WithColumn("Amount").AsDecimal(10, 2).NotNullable()
            .WithColumn("PaymentMethod").AsInt32().Nullable()
            .WithColumn("Status").AsInt32().NotNullable()
            .WithColumn("ReceivedAt").AsUtcDateTime().Nullable()

            .WithColumn("SaleId").AsGuid().NotNullable()
                .ForeignKey(
                    "FK_Payments_Sales_SaleId",
                    "Sales",
                    "Id")

            .WithColumn("ReceivedByUserId").AsGuid().Nullable()
                .ForeignKey(
                    "FK_Payments_Users_ReceivedByUserId",
                    "Users",
                    "Id");

        Create.Index("IX_Payments_SaleId")
            .OnTable("Payments")
            .OnColumn("SaleId").Ascending();
    }
}
