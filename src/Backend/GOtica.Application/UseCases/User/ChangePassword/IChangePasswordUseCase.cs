using GOtica.Communication.Requests;

namespace GOtica.Application.UseCases.User.ChangePassword;

public interface IChangePasswordUseCase
{
    Task Execute(RequestChangePassword request);
}
