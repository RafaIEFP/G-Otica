using GOtica.API.Handlers.Requirements;
using GOtica.Domain.Repositories.UserOpticalStore;
using Microsoft.AspNetCore.Authorization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace GOtica.API.Handlers;

public sealed class OwnerHandler : AuthorizationHandler<OwnerRequirement>
{
    private readonly IUserOpticalStoreReadOnlyRepository _userOpticalStoreReadOnlyRepository;
    public OwnerHandler(IUserOpticalStoreReadOnlyRepository userOpticalStoreReadOnlyRepository)
        => _userOpticalStoreReadOnlyRepository = userOpticalStoreReadOnlyRepository;


    protected override async Task HandleRequirementAsync(
        AuthorizationHandlerContext context, 
        OwnerRequirement requirement)
     {
        var userId = context.User.FindFirstValue(JwtRegisteredClaimNames.NameId);

        if (string.IsNullOrWhiteSpace(userId))
        {
            context.Fail();
            return;
        }

        if (!TryGetOpticalIdFromRoute(context, out var opticalStoreId))
        {
            context.Fail();
            return;
        }

        var userIsOwner = await _userOpticalStoreReadOnlyRepository.UserIsOwnerOfOpticalStore(Guid.Parse(userId), opticalStoreId);

        if (!userIsOwner)
        {
            context.Fail();
            return;
        }

        context.Succeed(requirement);
    }

    private static bool TryGetOpticalIdFromRoute(AuthorizationHandlerContext context, out Guid opticalStoreId)
    {
        opticalStoreId = default;

        if (context.Resource is not HttpContext httpContext)
            return false;

        var value = httpContext.Request.RouteValues["opticalStoreId"];

        return value is not null &&
               Guid.TryParse(value.ToString(), out opticalStoreId);
    }
}
