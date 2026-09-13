using FluentMigrator;
using GOtica.Infrastructure.Migrations.Services;

namespace GOtica.Infrastructure.Migrations.Versions;

[Migration(DatabaseVersions.TABLE_ITEM_LENS_TREATMENT, "Create item lens treatments table")]
public class Version00015 : ForwardOnlyMigration
{
    public override void Up()
    {
        Create.Table("ItemLensTreatments")
            .WithColumn("ItemLensId").AsGuid().NotNullable()
                .ForeignKey(
                    "FK_ItemLensTreatments_ItemLenses_ItemLensId",
                    "ItemLenses",
                    "Id")

            .WithColumn("TreatmentId").AsGuid().NotNullable()
                .ForeignKey(
                    "FK_ItemLensTreatments_Treatments_TreatmentId",
                    "Treatments",
                    "Id")

            .WithColumn("UnitPrice").AsDecimal(10, 2).NotNullable();

        Create.PrimaryKey("PK_ItemLensTreatments")
            .OnTable("ItemLensTreatments")
            .Columns("ItemLensId", "TreatmentId");

        Create.Index("IX_ItemLensTreatments_TreatmentId")
            .OnTable("ItemLensTreatments")
            .OnColumn("TreatmentId").Ascending();
    }
}
