using GOtica.Domain.Repositories.Product;
using GOtica.Exceptions.ExceptionsBase;
using GOtica.Exceptions.Resources;

namespace GOtica.Application.UseCases.Product.Deactivate;

public class DeactivateProductUseCase : IDeactivateProductUseCase
{
    private readonly IProductUpdateOnlyRepository _productUpdateOnlyRepository;
    public DeactivateProductUseCase(
        IProductUpdateOnlyRepository productUpdateOnlyRepository)
    {
        _productUpdateOnlyRepository = productUpdateOnlyRepository;
    }

    public async Task Execute(Guid opticalStoreId, Guid productId)
    {
        var deactivated = await _productUpdateOnlyRepository.Deactivate(productId, opticalStoreId);

        if (!deactivated)
            throw new NotFoundException(ResourceMessagesException.PRODUCT_NOT_FOUND);
    }
}
