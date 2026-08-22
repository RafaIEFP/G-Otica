using GOtica.Domain.Services;

namespace GOtica.API.Providers.Url;

internal sealed class ValidateInviteUrlProvider(
    IHttpContextAccessor httpContextAccessor,
    LinkGenerator linkGenerator) : IValidateInviteUrlProvider
{
    public string GenerateLink(string token)
    {
        var httpContext = httpContextAccessor.HttpContext;

        return linkGenerator.GetUriByName(
            httpContext!,
            "ValidateInvite",
            new { token })!;
    }
}
