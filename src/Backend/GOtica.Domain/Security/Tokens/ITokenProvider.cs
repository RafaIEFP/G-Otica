namespace GOtica.Domain.Security.Tokens;

public interface ITokenProvider
{
    string TokenOnRequest();
}
