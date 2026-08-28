using GOtica.Communication.Requests.Product;

namespace GOtica.Application.UseCases.Product.AdjustStock;

public interface IAdjustProductStockUseCase
{
    Task Execute(Guid opticalStoreId, Guid productId, RequestAdjustProductStock request);
}
