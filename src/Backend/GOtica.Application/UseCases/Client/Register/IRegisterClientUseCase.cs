using GOtica.Communication.Requests.Client;
using GOtica.Communication.Response.Client;

namespace GOtica.Application.UseCases.Client.Register;

public interface IRegisterClientUseCase
{
    Task<ResponseRegisterClient> Execute(Guid opticalStoreId, RequestRegisterClient request);
}
