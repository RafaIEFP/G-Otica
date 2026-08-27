using GOtica.Communication.Response.Product;

namespace GOtica.Application.UseCases.Product.Update;

public interface IUpdateProductUseCase
{
    Task Execute(Guid opticalStoreId, Guid productId, RequestUpdateProduct request);
}
