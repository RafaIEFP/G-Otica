using GOtica.Communication.Response;

namespace GOtica.Application.UseCases.User.Profile;

public interface IGetUserProfileUseCase
{
    Task<ResponseUserProfile> Execute();
}
