using FluentMigrator;
using GOtica.Domain.Enums;
using GOtica.Infrastructure.Migrations.Services;

namespace GOtica.Infrastructure.Migrations.Versions;

[Migration(DatabaseVersions.TABLE_INVITE, "Create table invite")]
public class Version00003 : ForwardOnlyMigration
{
    public override void Up()
    {
        Create.Table("Invites")
            .WithColumn("Id").AsGuid().PrimaryKey().NotNullable()
            .WithColumn("GuestEmail").AsString(255).NotNullable()
            .WithColumn("Role").AsString(50).NotNullable()
            .WithColumn("TokenHash").AsString(2000).NotNullable()
            .WithColumn("Status").AsInt32().NotNullable().WithDefaultValue((int)InviteStatus.Pending)
            .WithColumn("CreatedAt").AsUtcDateTime().NotNullable().WithDefault(SystemMethods.CurrentUTCDateTime)
            .WithColumn("ExpiresAt").AsUtcDateTime().NotNullable()
            .WithColumn("OpticalStoreId").AsGuid().NotNullable().ForeignKey("FK_Invites_OpticalStores_OpticalStoreId", "OpticalStores", "Id")
            .WithColumn("InvitedByUserId").AsGuid().NotNullable().ForeignKey("FK_Invites_Users_InvitedByUserId", "Users", "Id");
    }
}
