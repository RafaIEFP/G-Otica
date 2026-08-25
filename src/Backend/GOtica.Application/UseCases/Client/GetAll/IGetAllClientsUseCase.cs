using GOtica.Communication.Requests.Client;
using GOtica.Communication.Response;
using GOtica.Communication.Response.Client;

namespace GOtica.Application.UseCases.Client.GetAll;

public interface IGetAllClientsUseCase
{
    Task<ResponsePaged<ResponseGetAllClients>> Execute(Guid opticalStoreId, RequestGetAllClients request);
}
