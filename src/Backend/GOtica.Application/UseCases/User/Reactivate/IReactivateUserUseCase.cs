using GOtica.Communication.Requests.User;
using GOtica.Communication.Response.User;

namespace GOtica.Application.UseCases.User.Reactivate;

public interface IReactivateUserUseCase
{
    Task<ResponseRegisteredUser> Execute(RequestReactivateUser request);
}
