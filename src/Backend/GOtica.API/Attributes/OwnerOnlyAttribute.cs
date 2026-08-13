using Microsoft.AspNetCore.Authorization;

namespace GOtica.API.Attributes;

public sealed class OwnerOnlyAttribute : AuthorizeAttribute
{
    public OwnerOnlyAttribute() => Policy = "OwnerOnly";
}
