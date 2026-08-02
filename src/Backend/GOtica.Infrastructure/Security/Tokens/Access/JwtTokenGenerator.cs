using GOtica.Domain.Entities;
using GOtica.Domain.Security.Tokens;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace GOtica.Infrastructure.Security.Tokens.Access;

internal sealed class JwtTokenGenerator : IAccessTokenGenerator
{
    private readonly uint _expiresMinutes;
    private readonly string _signingKey;
    private readonly string _issuer;
    private readonly string _audience;
    public JwtTokenGenerator(uint expiresMinutes, string signingKey, string issuer, string audience)
    {
        _expiresMinutes = expiresMinutes;
        _signingKey = signingKey;
        _issuer = issuer;
        _audience = audience;
    }

    public (string token, Guid accessTokenIdentifier) Generate(User user)
    {
        var accessTokenIdentifier = Guid.CreateVersion7();

        var tokenHandler = new JwtSecurityTokenHandler();

        var claims = new List<Claim>
        {
            new (JwtRegisteredClaimNames.Jti, accessTokenIdentifier.ToString()),
            new (JwtRegisteredClaimNames.NameId, user.Id.ToString())
        };

        var tokenDescriptor = new SecurityTokenDescriptor()
        {
            Expires = DateTime.UtcNow.AddMinutes(_expiresMinutes),
            Subject = new ClaimsIdentity(claims),
            SigningCredentials = new SigningCredentials(JwtSecurityKeyFactory.Create(_signingKey), SecurityAlgorithms.HmacSha256Signature),
            Issuer = _issuer,
            Audience = _audience
        };

        var securityToken = tokenHandler.CreateToken(tokenDescriptor);

        return (tokenHandler.WriteToken(securityToken), accessTokenIdentifier);
    }
}
