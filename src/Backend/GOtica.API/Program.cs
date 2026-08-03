using GOtica.API.Filters;
using GOtica.API.OpenApi;
using GOtica.Application;
using GOtica.Infrastructure;
using GOtica.Infrastructure.Extensions;
using GOtica.Infrastructure.Migrations;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddOpenApi(options =>
{
    options.AddDocumentTransformer<BearerSecuritySchemeTransformer>();
});

builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddApplication();

builder.Services.AddMvc(options => options.Filters.Add<ExceptionFilter>());

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
