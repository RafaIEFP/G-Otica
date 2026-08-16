using GOtica.Communication.Response.User;

namespace GOtica.Application.UseCases.User.Profile;

public interface IGetUserProfileUseCase
{
    Task<ResponseUserProfile> Execute();
}
