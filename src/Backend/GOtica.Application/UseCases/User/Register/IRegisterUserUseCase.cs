using GOtica.Communication.Requests;
using GOtica.Communication.Response;

namespace GOtica.Application.UseCases.User.Register;

public interface IRegisterUserUseCase
{
    Task<ResponseRegisteredUser> Execute(RequestRegisterUser request);
}
