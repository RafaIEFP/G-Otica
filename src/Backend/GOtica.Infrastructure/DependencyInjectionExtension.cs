using FluentMigrator.Runner;
using GOtica.Infrastructure.DataAccess;
using GOtica.Infrastructure.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace GOtica.Infrastructure;

public static class DependencyInjectionExtension
{
    extension(IServiceCollection services)
    {
        public void AddInfrastructure(IConfiguration configuration)
        {
            AddGOticaDbContext(services, configuration);
            AddFluentMigrator(services, configuration);
        }
    }

    private static void AddGOticaDbContext(IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetDefaultConnectionString();

        services.AddDbContext<GOticaDbContext>(options =>
        {
            options.UseNpgsql(connectionString);
        });
    }

    private static void AddFluentMigrator(IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetDefaultConnectionString();

        services.AddFluentMigratorCore()
            .ConfigureRunner(rb =>
            {
                rb.AddPostgres().
                WithGlobalConnectionString(connectionString).
                ScanIn(Assembly.Load("GOtica.Infrastructure"))
                .For.All();
            });
    }
}
