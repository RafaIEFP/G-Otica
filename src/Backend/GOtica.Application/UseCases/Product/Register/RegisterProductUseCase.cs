using GOtica.Communication.Requests;
using GOtica.Communication.Requests.Product;
using GOtica.Communication.Response.Product;
using GOtica.Domain.Enums;
using GOtica.Domain.Repositories;
using GOtica.Domain.Repositories.Product;
using GOtica.Domain.Repositories.StockMovement;
using GOtica.Domain.Services;
using GOtica.Exceptions.ExceptionsBase;
using GOtica.Exceptions.Resources;
using Mapster;

namespace GOtica.Application.UseCases.Product.Register;

public class RegisterProductUseCase : IRegisterProductUseCase
{
    private readonly ILoggedUser _loggedUser;
    private readonly IStockMovementWriteOnlyRepository _stockMovementWriteOnlyRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IProductWriteOnlyRepository _productWriteOnlyRepository;
    private readonly IProductReadOnlyRepository _productReadOnlyRepository;
    public RegisterProductUseCase(
        ILoggedUser loggedUser,
        IStockMovementWriteOnlyRepository stockMovementWriteOnlyRepository,
        IUnitOfWork unitOfWork,
        IProductWriteOnlyRepository productWriteOnlyRepository,
        IProductReadOnlyRepository productReadOnlyRepository)
    {
        _loggedUser = loggedUser;
        _stockMovementWriteOnlyRepository = stockMovementWriteOnlyRepository;
        _unitOfWork = unitOfWork;
        _productWriteOnlyRepository = productWriteOnlyRepository;
        _productReadOnlyRepository = productReadOnlyRepository;
    }

    public async Task<ResponseRegisterProduct> Execute(Guid opticalStoreId, RequestRegisterProduct request)
    {
        request = request.Normalize();

        var loggedUser = await _loggedUser.Get();

        Validate(request);

        var productAlreadyRegistered = await _productReadOnlyRepository.ProductAlreadyAtOpticalStore(request.ProductCode, opticalStoreId);

        if (productAlreadyRegistered)
            throw new ConflictException(ResourceMessagesException.PRODUCT_ALREADY_REGISTERED);

        var product = request.Adapt<Domain.Entities.Product>();

        product.OpticalStoreId = opticalStoreId;

        await _unitOfWork.ExecuteInTransaction(async () =>
        {
            await _productWriteOnlyRepository.Add(product);

            if (product.StockQuantity > 0)
            {
                var stockMovement = new Domain.Entities.StockMovement
                {
                    QuantityChange = product.StockQuantity,
                    Type = StockMovementType.InitialStock,
                    ProductId = product.Id,
                    UserId = loggedUser.Id
                };

                await _stockMovementWriteOnlyRepository.Add(stockMovement);
            }
        });

        return product.Adapt<ResponseRegisterProduct>();
    }

    private static void Validate(RequestRegisterProduct request)
    {
        var result = new RegisterProductValidator().Validate(request);

        if (!result.IsValid)
            throw new ErrorOnValidationException(result.Errors.Select(e => e.ErrorMessage).ToList());
    }
}
