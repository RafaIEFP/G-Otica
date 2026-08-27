using GOtica.Communication.Requests;
using GOtica.Communication.Response.Product;
using GOtica.Domain.Repositories;
using GOtica.Domain.Repositories.Product;
using GOtica.Exceptions.ExceptionsBase;
using GOtica.Exceptions.Resources;
using Mapster;

namespace GOtica.Application.UseCases.Product.Update;

public class UpdateProductUseCase : IUpdateProductUseCase
{
    private readonly IProductUpdateOnlyRepository _productUpdateOnlyRepository;
    private readonly IProductReadOnlyRepository _productReadOnlyRepository;
    private readonly IUnitOfWork _unitOfWork;
    public UpdateProductUseCase(
        IProductUpdateOnlyRepository productUpdateOnlyRepository,
        IProductReadOnlyRepository productReadOnlyRepository,
        IUnitOfWork unitOfWork)
    {
        _productUpdateOnlyRepository = productUpdateOnlyRepository;
        _productReadOnlyRepository = productReadOnlyRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task Execute(Guid opticalStoreId, Guid productId, RequestUpdateProduct request)
    {
        request = request.Normalize();

        Validate(request);

        var product = await _productUpdateOnlyRepository.GetActiveInOpticalStore(productId, opticalStoreId)
            ?? 
            throw new NotFoundException(ResourceMessagesException.PRODUCT_NOT_FOUND);

        if (product.ProductCode != request.ProductCode)
        {
            var productCodeAlreadyRegistered = await _productReadOnlyRepository
                .ProductCodeAlreadyAtOpticalStore(
                        request.ProductCode,
                        opticalStoreId,
                        productId);

            if (productCodeAlreadyRegistered)
                throw new ConflictException(ResourceMessagesException.PRODUCT_ALREADY_REGISTERED);
        }

        request.Adapt(product);

        await _unitOfWork.Commit();
    }

    private static void Validate(RequestUpdateProduct request)
    {
        var result = new UpdateProductValidator().Validate(request);

        if (!result.IsValid)
        {
            var errorMessages = result.Errors.Select(error => error.ErrorMessage).ToList();

            throw new ErrorOnValidationException(errorMessages);
        }
    }
}
