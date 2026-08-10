using GOtica.Communication.Requests;

namespace GOtica.Application.UseCases.User.Update;

public interface IUpdateUserUseCase
{
    Task Execute(RequestUpdateUser request);
}
