using GOtica.API.Handlers.Requirements;
using GOtica.Domain.Repositories.Refresh;
using GOtica.Domain.Repositories.User;
using Microsoft.AspNetCore.Authorization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace GOtica.API.Handlers;

public class AuthenticatedUserHandler : AuthorizationHandler<AuthenticatedUserRequirement>
{
    private readonly IUserReadOnlyRepository _userReadOnlyRepository;
    private readonly IRefreshTokenReadOnlyRepository _refreshTokenReadOnlyRepository;

    public AuthenticatedUserHandler(
        IUserReadOnlyRepository userReadOnlyRepository,
        IRefreshTokenReadOnlyRepository refreshTokenReadOnlyRepository)
    {
        _userReadOnlyRepository = userReadOnlyRepository;
        _refreshTokenReadOnlyRepository = refreshTokenReadOnlyRepository;
    }
    protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, AuthenticatedUserRequirement requirement)
    {
        var userId = context.User.FindFirstValue(JwtRegisteredClaimNames.NameId);
        var accessTokenIdentifier = context.User.FindFirstValue(JwtRegisteredClaimNames.Jti);

        if (string.IsNullOrWhiteSpace(userId) || string.IsNullOrWhiteSpace(accessTokenIdentifier))
        {
            context.Fail();
            return;
        }

        var user = await _userReadOnlyRepository.GetUserById(Guid.Parse(userId));

        if (user is null || !user.IsActive)
        {
            context.Fail();
            return;
        }

        var hasRefreshToken = await 
            _refreshTokenReadOnlyRepository.HasRefresTokenAssociated(user.Id, Guid.Parse(accessTokenIdentifier));

        if (!hasRefreshToken)
        {
            context.Fail();
            return;
        }

        context.Succeed(requirement);
    }
}
