namespace GOtica.Domain.Security.Tokens.Access;

public interface ITokenProvider
{
    string TokenOnRequest();
}
