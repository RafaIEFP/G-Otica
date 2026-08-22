using GOtica.Application.Sevices.Invite;
using GOtica.Communication.Requests;
using GOtica.Domain.Repositories;
using GOtica.Domain.Repositories.Invite;
using GOtica.Domain.Repositories.UserOpticalStore;
using GOtica.Domain.Services;
using GOtica.Exceptions.ExceptionsBase;
using GOtica.Exceptions.Resources;
using Microsoft.Extensions.Options;

namespace GOtica.Application.UseCases.UserOpticalStore.Invite.Create;

public class CreateInviteUseCase : ICreateInviteUseCase
{
    private readonly ILoggedUser _loggedUser;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IInviteReadOnlyRepository _inviteReadOnlyRepository;
    private readonly IInviteWriteOnlyRepository _inviteWriteOnlyRepository;
    private readonly IInviteTokenService _inviteTokenService;
    private readonly InviteTokenSettings _inviteTokenSettings;
    private readonly IUserOpticalStoreReadOnlyRepository _userOpticalStoreReadOnlyRepository;
    private readonly IEmailSender _emailSender;
    private readonly IValidateInviteUrlProvider _validateInviteUrlProvider;
    public CreateInviteUseCase(
        ILoggedUser loggedUser,
        IUnitOfWork unitOfWork,
        IInviteReadOnlyRepository inviteReadOnlyRepository,
        IInviteWriteOnlyRepository inviteWriteOnlyRepository,
        IInviteTokenService inviteTokenService,
        IOptions<InviteTokenSettings> inviteTokenSettings,
        IUserOpticalStoreReadOnlyRepository userOpticalStoreReadOnlyRepository,
        IEmailSender emailSender,
        IValidateInviteUrlProvider validateInviteUrlProvider)
    {
        _loggedUser = loggedUser;
        _unitOfWork = unitOfWork;
        _inviteReadOnlyRepository = inviteReadOnlyRepository;
        _inviteWriteOnlyRepository = inviteWriteOnlyRepository;
        _inviteTokenService = inviteTokenService;
        _inviteTokenSettings = inviteTokenSettings.Value;
        _userOpticalStoreReadOnlyRepository = userOpticalStoreReadOnlyRepository;
        _emailSender = emailSender;
        _validateInviteUrlProvider = validateInviteUrlProvider;
    }

    public async Task Execute(Guid opticalStoreId, RequestInvite request)
    {
        var inviter = await _loggedUser.Get();

        Validate(request);

        var userBelongsToOptical = await _userOpticalStoreReadOnlyRepository
                                            .UserBelongsToOpticalByEmail(request.GuestEmail, opticalStoreId);

        if (userBelongsToOptical)
            throw new ConflictException(ResourceMessagesException.USER_ALREADY_MEMBER_OF_OS);

        var pendingInvitationExists = await _inviteReadOnlyRepository.ExistsPendingInvite(request.GuestEmail, opticalStoreId);

        if (pendingInvitationExists)
            throw new ConflictException(ResourceMessagesException.PENDING_INVITE_ALREADY_EXISTS);

        var tokens = _inviteTokenService.GenerateTokens();

        var invite = new Domain.Entities.Invite
        {
            GuestEmail = request.GuestEmail,
            Role = request.Role,
            TokenHash = tokens.TokenHash,
            ExpiresAt = DateTime.UtcNow.AddDays(_inviteTokenSettings.InviteValidityDays),
            OpticalStoreId = opticalStoreId,
            InvitedByUserId = inviter.Id,
        };

        await _inviteWriteOnlyRepository.Add(invite);

        await _unitOfWork.Commit();

        await _emailSender.Send(inviter.Name, request.GuestEmail, _validateInviteUrlProvider.GenerateLink(tokens.Token));
    }

    private void Validate(RequestInvite request)
    {
        var result = new CreateInviteValidator().Validate(request);

        if (!result.IsValid)
            throw new ErrorOnValidationException(result.Errors.Select(e => e.ErrorMessage).ToList());
    }
}
