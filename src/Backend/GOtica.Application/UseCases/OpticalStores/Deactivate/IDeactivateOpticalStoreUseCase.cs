namespace GOtica.Application.UseCases.OpticalStores.Deactivate;

public interface IDeactivateOpticalStoreUseCase
{
    Task Execute(Guid opticalStoreId);
}
