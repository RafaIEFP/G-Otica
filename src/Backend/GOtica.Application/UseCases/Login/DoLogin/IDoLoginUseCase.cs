using GOtica.Communication.Requests.User;
using GOtica.Communication.Response.User;

namespace GOtica.Application.UseCases.Login.DoLogin;

public interface IDoLoginUseCase
{
    Task<ResponseRegisteredUser> Execute(RequestLogin request);
}
