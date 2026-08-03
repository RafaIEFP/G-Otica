using GOtica.Communication.Requests;
using GOtica.Communication.Response;

namespace GOtica.Application.UseCases.Login.DoLogin;

public interface IDoLoginUseCase
{
    Task<ResponseRegisteredUser> Execute(RequestLogin request);
}
