using GOtica.Communication.Response.Product;
using GOtica.Domain.Repositories.Product;
using GOtica.Exceptions.ExceptionsBase;
using GOtica.Exceptions.Resources;
using Mapster;

namespace GOtica.Application.UseCases.Product.Get;

public class GetProductUseCase : IGetProductUseCase
{
    private readonly IProductReadOnlyRepository _productReadOnlyRepository;
    public GetProductUseCase(IProductReadOnlyRepository productReadOnlyRepository)
    {
        _productReadOnlyRepository = productReadOnlyRepository;
    }

    public async Task<ResponseGetProduct> Execute(Guid opticalStoreId, Guid productId)
    {
        var product = await _productReadOnlyRepository.GetById(productId, opticalStoreId)
        ?? 
        throw new NotFoundException(ResourceMessagesException.PRODUCT_NOT_FOUND);

        return product.Adapt<ResponseGetProduct>();
    }
}
