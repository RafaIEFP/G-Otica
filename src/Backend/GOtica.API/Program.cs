using GOtica.API.Filters;
using GOtica.API.Handlers;
using GOtica.API.Handlers.Requirements;
using GOtica.API.OpenApi;
using GOtica.Application;
using GOtica.Infrastructure;
using GOtica.Infrastructure.Extensions;
using GOtica.Infrastructure.Migrations;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers(op => op.Filters.Add<ExceptionFilter>());

builder.Services.AddOpenApi(options =>
{
    options.AddDocumentTransformer<BearerSecuritySchemeTransformer>();
});

builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddApplication();

builder.Services.AddRouting(config => config.LowercaseUrls = true);

builder.Services.AddScoped<IAuthorizationHandler, AuthenticatedUserHandler>();

builder.Services.AddAuthentication(op =>
{
    op.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    op.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearerValidated(builder.Configuration);

builder.Services.AddAuthorization(op =>
{
    op.AddPolicy("AuthenticatedUser", policy => policy.Requirements.Add(new AuthenticatedUserRequirement()));
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference(options => options
        .WithTitle("G-Otica API")
        .WithTheme(ScalarTheme.DeepSpace)
        .AddPreferredSecuritySchemes("Bearer"));
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

await MigrateDatabase();

app.Run();

async Task MigrateDatabase()
{
    await using var scope = app.Services.CreateAsyncScope();
    var connectionString = builder.Configuration.GetDefaultConnectionString();
    
    DatabaseMigrator.Migrate(scope.ServiceProvider);
}
