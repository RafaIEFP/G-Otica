using GOtica.Communication.Response.Product;

namespace GOtica.Application.UseCases.Product.Get;

public interface IGetProductUseCase
{
    Task<ResponseGetProduct> Execute(Guid opticalStoreId, Guid productId);
}
