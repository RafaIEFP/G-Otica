using FluentMigrator.Runner;
using GOtica.Domain.Repositories;
using GOtica.Domain.Security.Cryptography;
using GOtica.Domain.Security.Tokens.Access;
using GOtica.Domain.Security.Tokens.Invite;
using GOtica.Domain.Security.Tokens.Refresh;
using GOtica.Domain.Services;
using GOtica.Infrastructure.DataAccess;
using GOtica.Infrastructure.DataAccess.Repositories;
using GOtica.Infrastructure.Extensions;
using GOtica.Infrastructure.Security.Cryptography;
using GOtica.Infrastructure.Security.Tokens.Access;
using GOtica.Infrastructure.Security.Tokens.Invite;
using GOtica.Infrastructure.Security.Tokens.Refresh;
using GOtica.Infrastructure.Services.Email;
using GOtica.Infrastructure.Services.LoggedUser;
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
            AddEmailSender(services);
        }

        public void AddRepositoriesFromAssembly<T>()
        {
            services.Scan(scan => scan
                .FromAssemblyOf<T>()
                .AddClasses(classes => classes.InNamespaces("GOtica.Infrastructure.DataAccess.Repositories"), publicOnly: false)
                .AsImplementedInterfaces()
                .WithScopedLifetime());
        }
    }

    private static void AddRepositories(IServiceCollection services)
    {
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<ILoggedUser, LoggedUser>();

        services.AddRepositoriesFromAssembly<RefreshTokenRepository>();
        services.AddRepositoriesFromAssembly<UserRepository>();
        services.AddRepositoriesFromAssembly<UserOpticalStoreRepository>();
        services.AddRepositoriesFromAssembly<OpticalStoreRepository>();
        services.AddRepositoriesFromAssembly<InviteRepository>();
        services.AddRepositoriesFromAssembly<ClientRepository>();
        services.AddRepositoriesFromAssembly<ProductRepository>();
        services.AddRepositoriesFromAssembly<StockMovementRepository>();
        services.AddRepositoriesFromAssembly<SupplierRepository>();
        services.AddRepositoriesFromAssembly<PurchaseRepository>();
        services.AddRepositoriesFromAssembly<PrescriptionRepository>();
        services.AddRepositoriesFromAssembly<SaleRepository>();
        services.AddRepositoriesFromAssembly<PaymentRepository>();
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
        services.AddScoped<IInviteTokenGenerator, InviteTokenGenerator>();

        services.AddScoped<IAccessTokenValidator>(option => new JwtTokenValidator(signingKey, issuer, audience));
        services.AddScoped<IAccessTokenGenerator>(option => new JwtTokenGenerator(expirationTimeMinutes, signingKey, issuer, audience));
    }

    private static void AddEmailSender(IServiceCollection services)
    {
        services.AddOptions<EmailSettings>().BindConfiguration("SmtpSettings");
        services.AddScoped<IEmailSender, EmailSender>();
    }
}
