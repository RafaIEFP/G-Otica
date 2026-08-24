using GOtica.Application.Sevices.Invite;
using GOtica.Communication.Response.Invite;
using GOtica.Domain.Repositories;
using GOtica.Domain.Repositories.Invite;
using GOtica.Domain.Repositories.User;
using GOtica.Exceptions.ExceptionsBase;
using GOtica.Exceptions.Resources;

namespace GOtica.Application.UseCases.UserOpticalStore.Invite.Validade;

public class ValidateInviteUseCase : IValidateInviteUseCase
{
    private readonly IUserReadOnlyRepository _userReadOnlyRepository;
    private readonly IInviteReadOnlyRepository _inviteReadOnlyRepository;
    private readonly IInviteTokenService _inviteTokenService;
    public ValidateInviteUseCase(
        IUserReadOnlyRepository userReadOnlyRepository,
        IInviteReadOnlyRepository inviteReadOnlyRepository,
        IInviteTokenService inviteTokenService)
    {
        _userReadOnlyRepository = userReadOnlyRepository;
        _inviteReadOnlyRepository = inviteReadOnlyRepository;
        _inviteTokenService = inviteTokenService;
    }

    public async Task<ResponseValidateInvite> Execute(string token)
    {
        if (string.IsNullOrWhiteSpace(token))
            throw new NotFoundException(ResourceMessagesException.VALID_INVITE_NOT_FOUND);

        var tokenHash = _inviteTokenService.GenerateHash(token);

        var invite = await _inviteReadOnlyRepository.GetValidInviteByTokenHash(tokenHash)
            ?? throw new NotFoundException(ResourceMessagesException.VALID_INVITE_NOT_FOUND);

        var user = await _userReadOnlyRepository.GetActiveUserByEmail(invite.GuestEmail);

        return new ResponseValidateInvite
        {
            RequiresRegistration = user is null,
            RequiresReactivation = user is not null && !user.IsActive
        };
    }
}
