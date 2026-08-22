namespace GOtica.Domain.Services;

public interface IValidateInviteUrlProvider
{
    string GenerateLink(string token);
}
