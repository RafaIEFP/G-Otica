using GOtica.Domain.Repositories.Product;
using GOtica.Exceptions.ExceptionsBase;
using GOtica.Exceptions.Resources;

namespace GOtica.Application.UseCases.Product.Reactivate;

public class ReactivateProductUseCase : IReactivateProductUseCase
{
    private readonly IProductUpdateOnlyRepository _productUpdateOnlyRepository;
    public ReactivateProductUseCase(
        IProductUpdateOnlyRepository productUpdateOnlyRepository)
    {
        _productUpdateOnlyRepository = productUpdateOnlyRepository;
    }

    public async Task Execute(Guid opticalStoreId, Guid productId)
    {
        var reactivated = await _productUpdateOnlyRepository.Reactivate(productId, opticalStoreId);

        if (!reactivated)
            throw new NotFoundException( ResourceMessagesException.PRODUCT_NOT_FOUND);
    }
}
