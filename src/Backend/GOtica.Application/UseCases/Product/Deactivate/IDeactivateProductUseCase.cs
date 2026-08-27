namespace GOtica.Application.UseCases.Product.Deactivate;

public interface IDeactivateProductUseCase
{
    Task Execute(Guid opticalStoreId, Guid productId);
}
