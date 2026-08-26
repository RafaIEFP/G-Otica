namespace GOtica.Application.UseCases.Client.Deactivate;

public interface IDeactivateClientUseCase
{
    Task Execute(Guid opticalStoreId, Guid clientId);
}
