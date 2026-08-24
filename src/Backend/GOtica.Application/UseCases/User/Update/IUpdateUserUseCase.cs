using GOtica.Communication.Requests.User;

namespace GOtica.Application.UseCases.User.Update;

public interface IUpdateUserUseCase
{
    Task Execute(RequestUpdateUser request);
}
