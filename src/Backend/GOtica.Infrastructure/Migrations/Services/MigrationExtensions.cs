using FluentMigrator.Builders.Create.Table;

namespace GOtica.Infrastructure.Migrations.Services;

internal static class MigrationExtensions
{
    extension(ICreateTableColumnAsTypeSyntax column)
    {
        internal ICreateTableColumnOptionOrWithColumnSyntax AsUtcDateTime()
            => column.AsCustom("timestamp with time zone");
    }
}
