namespace GOtica.Domain.Security.Tokens;

public interface IAccessTokenValidator
{
    void Validate(string token);
    long GetUserIdentifier(string token);
    Guid GetAccessTokenIdentifier(string token);
}
