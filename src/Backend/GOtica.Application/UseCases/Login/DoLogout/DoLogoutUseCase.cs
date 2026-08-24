using GOtica.Domain.Repositories.Refresh;
using GOtica.Domain.Services;

namespace GOtica.Application.UseCases.Login.DoLogout;

public class DoLogoutUseCase : IDoLogoutUseCase
{
    private readonly ILoggedUser _loggedUser;
    private readonly IRefreshTokenWriteOnlyRepository _refreshTokenWriteOnlyRepository;

    public DoLogoutUseCase(
        ILoggedUser loggedUser,
        IRefreshTokenWriteOnlyRepository refreshTokenWriteOnlyRepository)
    {
        _loggedUser = loggedUser;
        _refreshTokenWriteOnlyRepository = refreshTokenWriteOnlyRepository;
    }

    public async Task Execute()
    {
        var loggedUser = await _loggedUser.Get();

        await _refreshTokenWriteOnlyRepository.DeleteUserRefresh(loggedUser.Id);
    }
}
