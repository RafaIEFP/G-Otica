using GOtica.Domain.Dtos;
using GOtica.Domain.Security.Tokens.Invite;

namespace GOtica.Application.Sevices.Invite;

public class InviteTokenService(IInviteTokenGenerator inviteTokenGenerator) : IInviteTokenService
{
    public InviteTokensDto GenerateTokens()
    {
        (var token, var tokenHash) = inviteTokenGenerator.Generate();

        return new InviteTokensDto
        {
            Token = token,
            TokenHash = tokenHash
        };
    }
}
