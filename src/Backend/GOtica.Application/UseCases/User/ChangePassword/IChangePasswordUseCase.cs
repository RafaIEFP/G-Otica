using GOtica.Communication.Requests.User;

namespace GOtica.Application.UseCases.User.ChangePassword;

public interface IChangePasswordUseCase
{
    Task Execute(RequestChangePassword request);
}
