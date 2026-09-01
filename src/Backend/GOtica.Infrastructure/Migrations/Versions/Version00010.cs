using FluentMigrator;
using GOtica.Infrastructure.Migrations.Services;

namespace GOtica.Infrastructure.Migrations.Versions;

[Migration(DatabaseVersions.TABLE_SALE, "Create sales table")]
public class Version00010 : ForwardOnlyMigration
{
    public override void Up()
    {
        Create.Table("Sales")
            .WithColumn("Id").AsGuid().PrimaryKey().NotNullable()
            .WithColumn("CreatedAt").AsDateTime().NotNullable()
            .WithColumn("Status").AsInt32().NotNullable()
            .WithColumn("TotalAmount").AsDecimal(10, 2).NotNullable()

            .WithColumn("OpticalStoreId").AsGuid().NotNullable()
                .ForeignKey(
                    "FK_Sales_OpticalStores_OpticalStoreId",
                    "OpticalStores",
                    "Id")

            .WithColumn("ClientId").AsGuid().NotNullable()
                .ForeignKey(
                    "FK_Sales_Clients_ClientId",
                    "Clients",
                    "Id")

            .WithColumn("UserId").AsGuid().NotNullable()
                .ForeignKey(
                    "FK_Sales_Users_UserId",
                    "Users",
                    "Id")

            .WithColumn("PrescriptionId").AsGuid().Nullable()
                .ForeignKey(
                    "FK_Sales_Prescriptions_PrescriptionId",
                    "Prescriptions",
                    "Id");

        Create.Index("IX_Sales_OpticalStoreId_CreatedAt")
            .OnTable("Sales")
            .OnColumn("OpticalStoreId").Ascending()
            .OnColumn("CreatedAt").Descending();
    }
}
