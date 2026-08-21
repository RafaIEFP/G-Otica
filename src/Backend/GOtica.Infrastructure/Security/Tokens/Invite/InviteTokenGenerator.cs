using GOtica.Domain.Security.Tokens.Invite;
using Microsoft.AspNetCore.WebUtilities;
using System.Security.Cryptography;
using System.Text;

namespace GOtica.Infrastructure.Security.Tokens.Invite;

internal sealed class InviteTokenGenerator : IInviteTokenGenerator
{
    public (string token, string tokenHash) Generate()
    {
        var token = WebEncoders.Base64UrlEncode(RandomNumberGenerator.GetBytes(32));

        var bytes = Encoding.UTF8.GetBytes(token);

        var tokenHash = Convert.ToHexString(SHA256.HashData(bytes));

        return (token, tokenHash);
    }
}
