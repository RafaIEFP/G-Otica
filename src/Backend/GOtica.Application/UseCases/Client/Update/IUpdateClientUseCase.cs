using GOtica.Communication.Requests.Client;

namespace GOtica.Application.UseCases.Client.Update;

public interface IUpdateClientUseCase
{
    Task Execute(Guid opticalStoreId, Guid clientId, RequestUpdateClient request);
}
