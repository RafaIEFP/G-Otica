using GOtica.Communication.Requests;
using GOtica.Communication.Requests.Product;
using GOtica.Communication.Response.Product;
using GOtica.Domain.Repositories;
using GOtica.Domain.Repositories.Product;
using GOtica.Exceptions.ExceptionsBase;
using GOtica.Exceptions.Resources;
using Mapster;

namespace GOtica.Application.UseCases.Product.Register;

public class RegisterProductUseCase : IRegisterProductUseCase
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IProductWriteOnlyRepository _productWriteOnlyRepository;
    private readonly IProductReadOnlyRepository _productReadOnlyRepository;
    public RegisterProductUseCase(
        IUnitOfWork unitOfWork,
        IProductWriteOnlyRepository productWriteOnlyRepository,
        IProductReadOnlyRepository productReadOnlyRepository)
    {
        _unitOfWork = unitOfWork;
        _productWriteOnlyRepository = productWriteOnlyRepository;
        _productReadOnlyRepository = productReadOnlyRepository;
    }

    public async Task<ResponseRegisterProduct> Execute(Guid opticalStoreId, RequestRegisterProduct request)
    {
        request = request.Normalize();

        Validate(request);

        var productAlreadyRegistered = await _productReadOnlyRepository.ProductAlreadyAtOpticalStore(request.ProductCode, opticalStoreId);

        if (productAlreadyRegistered)
            throw new ConflictException(ResourceMessagesException.PRODUCT_ALREADY_REGISTERED);

        var product = request.Adapt<Domain.Entities.Product>();

        product.OpticalStoreId = opticalStoreId;

        await _productWriteOnlyRepository.Add(product);

        await _unitOfWork.Commit();

        return product.Adapt<ResponseRegisterProduct>();
    }

    private static void Validate(RequestRegisterProduct request)
    {
        var result = new RegisterProductValidator().Validate(request);

        if (!result.IsValid)
            throw new ErrorOnValidationException(result.Errors.Select(e => e.ErrorMessage).ToList());
    }
}
