using GOtica.Domain.Repositories;
using GOtica.Domain.Repositories.Refresh;
using GOtica.Domain.Repositories.User;
using GOtica.Domain.Repositories.UserOpticalStore;
using GOtica.Domain.Services;
using GOtica.Exceptions.ExceptionsBase;

namespace GOtica.Application.UseCases.User.DeleteAccount;

public class DeleteAccountUseCase : IDeleteAccountUseCase
{
    private readonly ILoggedUser _loggedUser;
    private readonly IUserOpticalStoreReadOnlyRepository _userOpticalStoreReadOnlyRepository;
    private readonly IUserUpdateOnlyRepository _userUpdateOnlyRepository;
    private readonly IRefreshTokenWriteOnlyRepository _refreshTokenWriteOnlyRepository;
    private readonly IUserOpticalStoreUpdateOnlyRepository _userOpticalStoreUpdateOnlyRepository;
    private readonly IUnitOfWork _unitOfWork;
    public DeleteAccountUseCase(
        ILoggedUser loggedUser,
        IUserOpticalStoreReadOnlyRepository userOpticalStoreReadOnlyRepository,
        IUserUpdateOnlyRepository userUpdateOnlyRepository,
        IRefreshTokenWriteOnlyRepository refreshTokenWriteOnlyRepository,
        IUserOpticalStoreUpdateOnlyRepository userOpticalStoreUpdateOnlyRepository,
        IUnitOfWork unitOfWork)
    {
        _loggedUser = loggedUser;
        _userOpticalStoreReadOnlyRepository = userOpticalStoreReadOnlyRepository;
        _userUpdateOnlyRepository = userUpdateOnlyRepository;
        _refreshTokenWriteOnlyRepository = refreshTokenWriteOnlyRepository;
        _userOpticalStoreUpdateOnlyRepository = userOpticalStoreUpdateOnlyRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task Execute()
    {
        var loggedUser = await _loggedUser.Get();

        var userIsOwner = await _userOpticalStoreReadOnlyRepository.UserIsOwner(loggedUser.Id);

        if (userIsOwner)
            throw new UserCannotDeactivateAccountException();

        await _unitOfWork.ExecuteInTransaction(async () =>
        {
            await _userUpdateOnlyRepository.DeactivateAccount(loggedUser.Id);

            await _userOpticalStoreUpdateOnlyRepository.DeactivateByUser(loggedUser.Id);

            await _refreshTokenWriteOnlyRepository.DeleteUserRefresh(loggedUser.Id);
        });
    }
}
