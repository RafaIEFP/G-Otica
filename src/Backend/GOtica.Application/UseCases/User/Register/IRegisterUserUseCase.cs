using GOtica.Communication.Requests;
using GOtica.Communication.Response;

namespace GOtica.Application.UseCases.User.Register;

public interface IRegisterUserUseCase
{
    Task<ResponseRegisterUser> Execute(RequestRegisterUser request);
}
