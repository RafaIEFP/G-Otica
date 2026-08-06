using GOtica.Communication.Requests;
using GOtica.Communication.Response;

namespace GOtica.Application.UseCases.Token.RefreshToken;

public interface IRefreshTokenUseCase
{
    Task<ResponseTokens> Execute(RequestNewToken request);
}
