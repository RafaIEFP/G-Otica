using FluentMigrator;

namespace GOtica.Infrastructure.Migrations.Versions;

[Migration(DatabaseVersions.TABLE_REFESH_TOKEN, "Create user refresh tokens table")]
public class Version00002 : ForwardOnlyMigration
{
    public override void Up()
    {
        Create.Table("RefreshTokens")
            .WithColumn("Id").AsGuid().PrimaryKey().NotNullable()
            .WithColumn("Token").AsString(2000).NotNullable()
            .WithColumn("CreatedAt").AsDateTime().NotNullable().WithDefault(SystemMethods.CurrentUTCDateTime)
            .WithColumn("ExpiresAt").AsDateTime().NotNullable()
            .WithColumn("AccessTokenId").AsGuid().NotNullable()
            .WithColumn("UserId").AsGuid().NotNullable().ForeignKey("FK_RefreshTokens_User_Id", "Users", "Id");
    }
}
