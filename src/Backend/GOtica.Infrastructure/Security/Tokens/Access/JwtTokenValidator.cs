using GOtica.Domain.Security.Tokens;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;

namespace GOtica.Infrastructure.Security.Tokens.Access;

internal sealed class JwtTokenValidator : IAccessTokenValidator
{
    private readonly string _signingKey;
    private readonly string _issuer;
    private readonly string _audience;
    public JwtTokenValidator(string signingKey, string issuer, string audience)
    {
        _signingKey = signingKey;
        _issuer = issuer;
        _audience = audience;
    }

    public Guid GetAccessTokenIdentifier(string token)
        => Guid.Parse(GetClaimValue(token, JwtRegisteredClaimNames.Jti));

    public long GetUserIdentifier(string token)
        => long.Parse(GetClaimValue(token, JwtRegisteredClaimNames.NameId));

    public void Validate(string token)
    {
        var validationParameters = new TokenValidationParameters()
        {
            ValidateAudience = true,
            ValidAudience = _audience,

            ValidateIssuer = true,
            ValidIssuer = _issuer,

            ValidateIssuerSigningKey = true,
            IssuerSigningKey = JwtSecurityKeyFactory.Create(_signingKey),
            ClockSkew = TimeSpan.Zero
        };

        var tokenHandler = new JwtSecurityTokenHandler();

        tokenHandler.ValidateToken(token, validationParameters, out _);
    }

    private static string GetClaimValue(string token, string claimType)
    {
        var tokenHandler = new JwtSecurityTokenHandler();

        var jwtToken = tokenHandler.ReadJwtToken(token);

        return jwtToken.Claims.First(claim => claim.Type == claimType).Value;
    }
}
