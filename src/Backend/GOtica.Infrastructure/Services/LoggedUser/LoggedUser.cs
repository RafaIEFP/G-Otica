using GOtica.Domain.Entities;
using GOtica.Domain.Security.Tokens.Access;
using GOtica.Domain.Services;
using GOtica.Infrastructure.DataAccess;
using Microsoft.EntityFrameworkCore;
using System.IdentityModel.Tokens.Jwt;

namespace GOtica.Infrastructure.Services.LoggedUser;

internal class LoggedUser : ILoggedUser
{
    private readonly GOticaDbContext _dbContext;
    private readonly ITokenProvider _tokenProvider;
    public LoggedUser(GOticaDbContext dbContext, ITokenProvider tokenProvider)
    {
        _dbContext = dbContext;
        _tokenProvider = tokenProvider;
    }

    public async Task<User> Get()
    {
        var token = _tokenProvider.TokenOnRequest();

        var tokenHandler = new JwtSecurityTokenHandler();

        var jwtSecurityToken = tokenHandler.ReadJwtToken(token);

        var identifier = jwtSecurityToken.Claims.First(claim => claim.Type == JwtRegisteredClaimNames.NameId).Value;

        return await _dbContext.Users.AsNoTracking().FirstAsync(u => u.Id == Guid.Parse(identifier));
    }
}
