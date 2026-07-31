using Microsoft.Extensions.Configuration;

namespace GOtica.Infrastructure.Extensions;

public static class ConfigurationExtensions
{
    extension (IConfiguration configuration)
    {
        public string GetDefaultConnectionString() => configuration.GetConnectionString("DefaultConnection")!;
    }
}
