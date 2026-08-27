namespace GOtica.Application.UseCases.Product.Reactivate;

public interface IReactivateProductUseCase
{
    Task Execute(Guid opticalStoreId, Guid productId);
}
