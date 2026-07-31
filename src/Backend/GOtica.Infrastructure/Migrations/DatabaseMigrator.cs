using FluentMigrator.Runner;
using Microsoft.Extensions.DependencyInjection;

namespace GOtica.Infrastructure.Migrations;

public static class DatabaseMigrator
{
    public static void Migrate(IServiceProvider serviceProvider)
    {
        var runner = serviceProvider.GetRequiredService<IMigrationRunner>();

        runner.ListMigrations();
        runner.MigrateUp();
    }
}
