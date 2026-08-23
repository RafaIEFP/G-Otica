namespace GOtica.Application.UseCases.UserOpticalStores.Deactivate;

public interface IDeactivateUserOpticalStoreUseCase
{
    Task Execute(Guid opticalStoreId, Guid userId);
}
