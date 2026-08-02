namespace GOtica.Domain.Security.Tokens;

public interface IRefreshTokenGenerator
{
    string Generate();
}
