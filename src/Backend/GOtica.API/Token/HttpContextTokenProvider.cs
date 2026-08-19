using GOtica.Domain.Security.Tokens.Access;

namespace GOtica.API.Token;

public class HttpContextTokenProvider : ITokenProvider
{
    private readonly IHttpContextAccessor _contextAccessor;
    public HttpContextTokenProvider(IHttpContextAccessor httpContextAccessor)
        => _contextAccessor = httpContextAccessor;

    public string TokenOnRequest()
    {
        var token = _contextAccessor.HttpContext!.Request.Headers.Authorization.ToString();

        return token["Bearer ".Length..].Trim();
    }
}
