using GOtica.Domain.Dtos;

namespace GOtica.Application.Sevices.Invite;

public interface IInviteTokenService
{
    InviteTokensDto GenerateTokens();
}
