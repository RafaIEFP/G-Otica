using GOtica.Application.Sevices.Invite;
using GOtica.Communication.Requests;
using GOtica.Domain.Repositories;
using GOtica.Domain.Repositories.Invite;
using GOtica.Domain.Repositories.User;
using GOtica.Domain.Repositories.UserOpticalStore;
using GOtica.Domain.Services;
using GOtica.Exceptions.ExceptionsBase;
using GOtica.Exceptions.Resources;

namespace GOtica.Application.UseCases.Invite.Accept;

public class AcceptInviteUseCase : IAcceptInviteUseCase
{
    private readonly ILoggedUser _loggedUser;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IInviteTokenService _inviteTokenService;
    private readonly IInviteReadOnlyRepository _inviteReadOnlyRepository;
    private readonly IInviteUpdateOnlyRepository _inviteUpdateOnlyRepository;
    private readonly IUserReadOnlyRepository _userReadOnlyRepository;
    private readonly IUserOpticalStoreReadOnlyRepository _userOpticalStoreReadOnlyRepository;
    private readonly IUserOpticalStoreWriteOnlyRepository _userOpticalStoreWriteOnlyRepository;
    public AcceptInviteUseCase(
        ILoggedUser loggedUser,
        IUnitOfWork unitOfWork,
        IInviteTokenService inviteTokenService,
        IInviteReadOnlyRepository inviteReadOnlyRepository,
        IInviteUpdateOnlyRepository inviteUpdateOnlyRepository,
        IUserReadOnlyRepository userReadOnlyRepository,
        IUserOpticalStoreReadOnlyRepository userOpticalStoreReadOnlyRepository,
        IUserOpticalStoreWriteOnlyRepository userOpticalStoreWriteOnlyRepository)
    {
        _loggedUser = loggedUser;
        _unitOfWork = unitOfWork;
        _inviteTokenService = inviteTokenService;
        _inviteReadOnlyRepository = inviteReadOnlyRepository;
        _inviteUpdateOnlyRepository = inviteUpdateOnlyRepository;
        _userReadOnlyRepository = userReadOnlyRepository;
        _userOpticalStoreReadOnlyRepository = userOpticalStoreReadOnlyRepository;
        _userOpticalStoreWriteOnlyRepository = userOpticalStoreWriteOnlyRepository;
    }

    public async Task Execute(RequestAcceptInvite request)
    {
        var loggedUser = await _loggedUser.Get();

        if (string.IsNullOrWhiteSpace(request.Token))
            throw new NotFoundException(ResourceMessagesException.VALID_INVITE_NOT_FOUND);

        var tokenHash = _inviteTokenService.GenerateHash(request.Token);

        var invite = await _inviteReadOnlyRepository.GetValidInviteByTokenHash(tokenHash)
            ?? throw new NotFoundException(ResourceMessagesException.VALID_INVITE_NOT_FOUND);

        if (!string.Equals(
                invite.GuestEmail, 
                loggedUser.Email,
                StringComparison.OrdinalIgnoreCase))
        {
            throw new ForbiddenException(ResourceMessagesException.INVITE_DOES_NOT_BELONGS_USER);
        }

        var userBelongsToOptical = await _userOpticalStoreReadOnlyRepository
                                            .UserBelongsToOptical(loggedUser.Id, invite.OpticalStoreId);

        if (userBelongsToOptical)
            throw new ConflictException(ResourceMessagesException.USER_ALREADY_MEMBER_OF_OS);

        var userOpticalStore = new Domain.Entities.UserOpticalStore
        {
            EntranceDate = DateOnly.FromDateTime(DateTime.UtcNow),
            Role = invite.Role,
            UserId = loggedUser.Id,
            OpticalStoreId = invite.OpticalStoreId,
        };

        await _unitOfWork.ExecuteInTransaction(async () =>
        {
            await _userOpticalStoreWriteOnlyRepository.Add(userOpticalStore);
            await _inviteUpdateOnlyRepository.UpdateStatusToAccepted(invite.Id);
        });
    }
}
