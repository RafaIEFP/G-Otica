using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace GOtica.Infrastructure.Security.Tokens.Access;

internal static class JwtSecurityKeyFactory
{
    public static SymmetricSecurityKey Create(string signingKey)
        => new (Encoding.UTF8.GetBytes(signingKey));
}
