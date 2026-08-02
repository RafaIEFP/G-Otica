using FluentMigrator.Runner;
using GOtica.Domain.Repositories;
using GOtica.Domain.Repositories.Refresh;
using GOtica.Domain.Repositories.User;
using GOtica.Domain.Security.Cryptography;
using GOtica.Domain.Security.Tokens;
using GOtica.Infrastructure.DataAccess;
using GOtica.Infrastructure.DataAccess.Repositories;
using GOtica.Infrastructure.Extensions;
using GOtica.Infrastructure.Security.Cryptography;
using GOtica.Infrastructure.Security.Tokens.Access;
using GOtica.Infrastructure.Security.Tokens.Refresh;
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
            AddPasswordEncyptor(services);
            AddRepositories(services);
            AddTokenHandlers(services, configuration);
        }
    }

    private static void AddRepositories(IServiceCollection services)
    {
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        services.AddScoped<IRefreshTokenReadOnlyRepository, RefreshTokenRepository>();
        services.AddScoped<IRefreshTokenWriteOnlyRepository, RefreshTokenRepository>();
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

    private static void AddPasswordEncyptor(IServiceCollection services) 
        => services.AddScoped<IPasswordEncryptor, PasswordEncryptor>();

    private static void AddTokenHandlers(IServiceCollection services, IConfiguration configuration)
    {
        var expirationTimeMinutes = configuration.GetValue<uint>("Settings:Jwt:ExpiresMinutes");
        var signingKey = configuration.GetValue<string>("Settings:Jwt:SigningKey")!;
        var issuer = configuration.GetValue<string>("Settings:Jwt:Issuer")!;
        var audience = configuration.GetValue<string>("Settings:Jwt:Audience")!;

        services.AddScoped<IRefreshTokenGenerator, RefreshTokenGenerator>();

        services.AddScoped<IAccessTokenValidator>(option => new JwtTokenValidator(signingKey, issuer, audience));
        services.AddScoped<IAccessTokenGenerator>(option => new JwtTokenGenerator(expirationTimeMinutes, signingKey, issuer, audience));
    }
}
