using GOtica.Domain.Security.Tokens.Refresh;
using Microsoft.AspNetCore.WebUtilities;
using System.Security.Cryptography;

namespace GOtica.Infrastructure.Security.Tokens.Refresh;

internal sealed class RefreshTokenGenerator : IRefreshTokenGenerator
{
    public string Generate()
        => WebEncoders.Base64UrlEncode(RandomNumberGenerator.GetBytes(32));
}
