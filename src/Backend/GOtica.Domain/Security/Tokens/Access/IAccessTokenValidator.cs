namespace GOtica.Domain.Security.Tokens.Access;

public interface IAccessTokenValidator
{
    void Validate(string token);
    Guid GetUserIdentifier(string token);
    Guid GetAccessTokenIdentifier(string token);
}
