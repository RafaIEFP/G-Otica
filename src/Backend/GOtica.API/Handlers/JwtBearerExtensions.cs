using Microsoft.AspNetCore.Authentication;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace GOtica.API.Handlers;

public static class JwtBearerExtensions
{
    public static AuthenticationBuilder AddJwtBearerValidated(this AuthenticationBuilder authenticationBuilder, IConfiguration configuration)
    {
        var signingKey = configuration.GetValue<string>("Settings:Jwt:SigningKey")!;
        var issuer = configuration.GetValue<string>("Settings:Jwt:Issuer")!;
        var audience = configuration.GetValue<string>("Settings:Jwt:Audience")!;

        authenticationBuilder.AddJwtBearer(op =>
        {
            op.MapInboundClaims = false;
            op.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = true,
                ValidAudience = audience,

                ValidateIssuer = true,
                ValidIssuer = issuer,

                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(signingKey!)),
                ClockSkew = TimeSpan.Zero
            };
        });

        return authenticationBuilder;
    }
}
