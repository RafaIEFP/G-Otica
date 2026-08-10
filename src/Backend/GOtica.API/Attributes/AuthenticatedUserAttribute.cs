using Microsoft.AspNetCore.Authorization;

namespace GOtica.API.Attributes;

public sealed class AuthenticatedUserAttribute : AuthorizeAttribute
{
    public AuthenticatedUserAttribute() => Policy = "AuthenticatedUser";
}
