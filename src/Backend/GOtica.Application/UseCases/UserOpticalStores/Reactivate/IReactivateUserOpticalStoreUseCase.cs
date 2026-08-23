namespace GOtica.Application.UseCases.UserOpticalStores.Reactivate;

public interface IReactivateUserOpticalStoreUseCase
{
    Task Execute(Guid opticalStoreId, Guid userId);
}
