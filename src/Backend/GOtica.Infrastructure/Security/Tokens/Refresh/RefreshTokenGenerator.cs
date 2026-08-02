using GOtica.Domain.Security.Tokens;
using System.Security.Cryptography;

namespace GOtica.Infrastructure.Security.Tokens.Refresh;

internal sealed class RefreshTokenGenerator : IRefreshTokenGenerator
{
    public string Generate()
        => Convert.ToBase64String(RandomNumberGenerator.GetBytes(32));
}
