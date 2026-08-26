namespace GOtica.Application.UseCases.Client.Reactivate;

public interface IReactivateClientUseCase
{
    Task Execute(Guid opticalStoreId, Guid clientId);
}
