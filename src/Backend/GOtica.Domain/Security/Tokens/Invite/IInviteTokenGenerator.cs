namespace GOtica.Domain.Security.Tokens.Invite;

public interface IInviteTokenGenerator
{
    (string token, string tokenHash) Generate();
    string GenerateHash(string token);
}
