using GOtica.Communication.Requests.User;
using GOtica.Communication.Response.User;

namespace GOtica.Application.UseCases.User.Register;

public interface IRegisterUserUseCase
{
    Task<ResponseRegisteredUser> Execute(RequestRegisterUser request);
}
