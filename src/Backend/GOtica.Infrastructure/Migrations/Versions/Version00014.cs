using FluentMigrator;
using GOtica.Infrastructure.Migrations.Services;

namespace GOtica.Infrastructure.Migrations.Versions;

[Migration(DatabaseVersions.TABLE_ITEM_LENS, "Create item lenses table")]
public class Version00014 : ForwardOnlyMigration
{
    public override void Up()
    {
        Create.Table("ItemLenses")
            .WithColumn("Id").AsGuid().PrimaryKey().NotNullable()
            .WithColumn("EyeSide").AsInt32().NotNullable()
            .WithColumn("PupillaryDistance").AsDecimal(5, 2).Nullable()
            .WithColumn("NasoPupillaryDistance").AsDecimal(5, 2).Nullable()
            .WithColumn("LensType").AsInt32().NotNullable()
            .WithColumn("RefractiveIndex").AsDecimal(5, 2).NotNullable()
            .WithColumn("Material").AsInt32().NotNullable()
            .WithColumn("Color").AsString(100).Nullable()
            .WithColumn("Diameter").AsDecimal(5, 2).NotNullable()

            .WithColumn("SaleItemId").AsGuid().NotNullable()
                .ForeignKey(
                    "FK_ItemLenses_SaleItems_SaleItemId",
                    "SaleItems",
                    "Id");

        Create.Index("UX_ItemLenses_SaleItemId")
            .OnTable("ItemLenses")
            .OnColumn("SaleItemId").Ascending()
            .WithOptions().Unique();
    }
}
