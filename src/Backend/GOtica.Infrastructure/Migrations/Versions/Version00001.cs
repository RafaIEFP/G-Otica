using FluentMigrator;

namespace GOtica.Infrastructure.Migrations.Versions;

[Migration(DatabaseVersions.TABLE_USER_DOMAIN, "Create initial user and optical store schema")]
public class Version00001 : ForwardOnlyMigration
{
    public override void Up()
    {
        Create.Table("Users")
            .WithColumn("Id").AsInt64().PrimaryKey().Identity().NotNullable()
            .WithColumn("Name").AsString(255).NotNullable()
            .WithColumn("Email").AsString(255).NotNullable().Unique()
            .WithColumn("Password").AsString(2000).NotNullable()
            .WithColumn("IsActive").AsBoolean().NotNullable().WithDefaultValue(true)
            .WithColumn("CreatedAt").AsDateTime().NotNullable().WithDefault(SystemMethods.CurrentUTCDateTime)
            .WithColumn("UpdatedAt").AsDateTime().Nullable();

        Create.Table("OpticalStores")
            .WithColumn("Id").AsInt64().PrimaryKey().Identity().NotNullable()
            .WithColumn("Name").AsString(255).NotNullable()
            .WithColumn("Email").AsString(255).NotNullable()
            .WithColumn("PhoneNumber").AsString(20).NotNullable()
            .WithColumn("TaxNumber").AsString(50).NotNullable().Unique()
            .WithColumn("IsActive").AsBoolean().NotNullable().WithDefaultValue(true);


        // Create the UserOpticalStores table with a composite primary key

        Create.Table("UserOpticalStores")
            .WithColumn("UserId").AsInt64().NotNullable()
            .WithColumn("OpticalStoreId").AsInt64().NotNullable()
            .WithColumn("EntranceDate").AsDate().NotNullable()
            .WithColumn("Role").AsString(100).NotNullable()
            .WithColumn("IsActive").AsBoolean().NotNullable().WithDefaultValue(true);

        Create.PrimaryKey("PK_UserOpticalStores")
            .OnTable("UserOpticalStores")
            .Columns("UserId", "OpticalStoreId");

        Create.ForeignKey("FK_UserOpticalStores_Users")
            .FromTable("UserOpticalStores").ForeignColumn("UserId")
            .ToTable("Users").PrimaryColumn("Id");

        Create.ForeignKey("FK_UserOpticalStores_OpticalStores")
            .FromTable("UserOpticalStores").ForeignColumn("OpticalStoreId")
            .ToTable("OpticalStores").PrimaryColumn("Id");

        // ---------------------------------------------------------------------
    }
}
