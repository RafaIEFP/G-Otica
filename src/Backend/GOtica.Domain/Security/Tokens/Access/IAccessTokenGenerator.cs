using GOtica.Domain.Entities;

namespace GOtica.Domain.Security.Tokens.Access;

public interface IAccessTokenGenerator
{
    (string token, Guid accessTokenIdentifier) Generate(User user);
}
