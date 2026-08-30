using FluentMigrator;
using GOtica.Infrastructure.Migrations.Services;

namespace GOtica.Infrastructure.Migrations.Versions;

[Migration(DatabaseVersions.TABLE_PRESCRIPTION, "Create prescriptions table")]
public class Version00009 : ForwardOnlyMigration
{
    public override void Up()
    {
        Create.Table("Prescriptions")
            .WithColumn("Id").AsGuid().PrimaryKey().NotNullable()

            .WithColumn("DoctorName").AsString(255).NotNullable()
            .WithColumn("DoctorRegistration").AsString(20).NotNullable()

            .WithColumn("PrescriptionDate").AsDate().NotNullable()
            .WithColumn("ExpirationDate").AsDate().NotNullable()

            .WithColumn("RightEyeSphere").AsDecimal(5, 2).Nullable()
            .WithColumn("LeftEyeSphere").AsDecimal(5, 2).Nullable()

            .WithColumn("RightEyeCylinder").AsDecimal(5, 2).Nullable()
            .WithColumn("LeftEyeCylinder").AsDecimal(5, 2).Nullable()

            .WithColumn("RightEyeAxis").AsInt32().Nullable()
            .WithColumn("LeftEyeAxis").AsInt32().Nullable()

            .WithColumn("RightEyeVisualAcuity").AsString(20).Nullable()
            .WithColumn("LeftEyeVisualAcuity").AsString(20).Nullable()

            .WithColumn("Addition").AsDecimal(5, 2).Nullable()
            .WithColumn("NearVisualAcuity").AsString(20).Nullable()

            .WithColumn("RecommendedReturnDate").AsDate().Nullable()
            .WithColumn("Notes").AsString(1000).Nullable()

            .WithColumn("ClientId").AsGuid().NotNullable()
                .ForeignKey(
                    "FK_Prescriptions_Clients_ClientId",
                    "Clients",
                    "Id");

        Create.Index("IX_Prescriptions_ClientId_PrescriptionDate")
            .OnTable("Prescriptions")
            .OnColumn("ClientId").Ascending()
            .OnColumn("PrescriptionDate").Descending();
    }
}
