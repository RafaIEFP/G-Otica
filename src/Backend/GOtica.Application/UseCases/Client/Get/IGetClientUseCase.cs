using GOtica.Communication.Response.Client;

namespace GOtica.Application.UseCases.Client.Get;

public interface IGetClientUseCase
{
    Task<ResponseGetClient> Execute(Guid opticalStoreId, Guid clientId);
}
